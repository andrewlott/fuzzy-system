using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogSystem : BaseSystem {
	private static float _RATE = 0.001f; //0.025f; // seconds per character
    protected TextMeshProUGUI _tmp;
    private bool _getNext;
    private bool _waiting;
    private bool _showing;

    public override void Start() {
        _tmp = GameController.Instance.dialogText;
        Pool.Instance.AddSystemListener(typeof(DialogComponent), this);
        Pool.Instance.AddSystemListener(typeof(TouchComponent), this);
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(DialogComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(TouchComponent), this);
    }

    public override void Update() {

    }

    public override void OnComponentAdded(BaseComponent c) {
        if (c is DialogComponent) {
            DialogComponent dc = c as DialogComponent;
            DisplayDialog(dc);
        } else if (c is TouchComponent) {
            if (_waiting) {
                _getNext = true;
            } else if (_showing) {
                _tmp.maxVisibleCharacters = _tmp.textInfo.pageInfo[_tmp.pageToDisplay - 1].lastCharacterIndex;
            }
        }
    }

    public override void OnComponentRemoved(BaseComponent c) {
        if (c is DialogComponent) {
            DialogComponent dc = c as DialogComponent;
            if (dc.dialogId == 43) {
                GameController.Instance.ClearProgress();
                return;
            }
            _tmp.text = "";
			_tmp.pageToDisplay = 1;
            GameController.Instance.dialogStateMachine.SetTrigger("GoodTrigger");
        }
    }

    protected void DisplayDialog(DialogComponent dc) {
        GameController.Instance.HandleCoroutine(InternalDisplayDialog(dc));
    }

    // bug in dialog system where get stuck on wrong page maybe
    private IEnumerator InternalDisplayDialog(DialogComponent dc) {
        _showing = true;
        _tmp.text = string.Format(dc.dialog, GameController.Instance.playerLuck.item, GameController.Instance.opponentLuck.item);
        _tmp.maxVisibleCharacters = 0;
        while (_tmp.maxVisibleCharacters < _tmp.text.Length) {
            _tmp.maxVisibleCharacters++;
            yield return new WaitForSeconds(_RATE);
            if (_tmp.maxVisibleCharacters > _tmp.textInfo.pageInfo[_tmp.pageToDisplay - 1].lastCharacterIndex) { // condition needs improving
                _waiting = true;
                yield return new WaitUntil(HasTouched);
                _waiting = false;
                _getNext = false;
                if (_tmp.pageToDisplay == _tmp.textInfo.pageInfo.Length) {
                    break;
                }
                _tmp.pageToDisplay = Mathf.Min(_tmp.textInfo.pageInfo.Length, _tmp.pageToDisplay + 1);
            }
        }
        _showing = false;
		OnComplete(dc);
    }

    protected virtual void OnComplete(DialogComponent dc) {
        GameObject.Destroy(dc);
        GameController.Instance.ClearDice();
    }

    protected virtual bool HasTouched() {
        return _getNext;
    }
}

class PreMatchDialogSystem : DialogSystem {
    public override void Start() {
        _tmp = GameController.Instance.dialogText;
        Pool.Instance.AddSystemListener(typeof(PreMatchDialogComponent), this);
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(PreMatchDialogComponent), this);
    }

    public override void OnComponentAdded(BaseComponent c) {
        if (c is PreMatchDialogComponent) {
            PreMatchDialogComponent pmdc = c as PreMatchDialogComponent;
            GameController.Instance.ShowHideLuck(true);
            PlayerPrefs.SetInt("CheckPoint", pmdc.dialogId);
            DisplayDialog(pmdc);
        }
    }

    public override void OnComponentRemoved(BaseComponent c) {
        if (c is PreMatchDialogComponent) {
            //_tmp.text = "";
            _tmp.pageToDisplay = 1;
            GameController.Instance.ShowHideLuck(false);
            GameController.Instance.dialogStateMachine.SetTrigger("GoodTrigger");
        }
    }

    protected override void OnComplete(DialogComponent dc) {
        GameObject.Destroy(dc);
        GameController.Instance.ClearDice();
    }

    protected override bool HasTouched() {
        return GameController.Instance.luckSelected;
    }
}

/*
 * Match dialog system doesn't destroy, match system does.
 * Match system gets the component, does the calculation, plays animation, then when it's done allows for continue (does this by setting _getNext to false, or leaving it false, and then setting it to true
 * -- maybe move all the internal stuff to update to make that easier, or leave in update of match dialog system
 * 
 */