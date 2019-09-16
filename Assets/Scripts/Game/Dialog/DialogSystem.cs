using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogSystem : BaseSystem {
    private static float _RATE = 0.05f; // seconds per character
    private TextMeshProUGUI _tmp;
    private bool _getNext;
    private bool _waiting;

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
            }
        }
    }

    public override void OnComponentRemoved(BaseComponent c) {
        if (c is DialogComponent) {
            _tmp.text = "";
            GameController.Instance.dialogStateMachine.SetTrigger("GoodTrigger");
        }
    }

    private void DisplayDialog(DialogComponent dc) {
        GameController.Instance.HandleCoroutine(InternalDisplayDialog(dc));
    }

    private IEnumerator InternalDisplayDialog(DialogComponent dc) {
        int len = 0;
        _tmp.text = dc.dialog;
        _tmp.maxVisibleCharacters = 0;
        while (_tmp.maxVisibleCharacters < dc.dialog.Length) {
            len += 1;
            _tmp.maxVisibleCharacters = len;
            yield return new WaitForSeconds(_RATE);
            if (_tmp.maxVisibleCharacters > _tmp.textInfo.pageInfo[_tmp.pageToDisplay - 1].lastCharacterIndex) {
                _waiting = true;
                yield return new WaitUntil(HasTouched);
                _waiting = false;
                _getNext = false;
                _tmp.pageToDisplay = Mathf.Min(_tmp.textInfo.pageCount, _tmp.pageToDisplay + 1);
            }
        }
        OnComplete(dc);
    }

    protected virtual void OnComplete(DialogComponent dc) {
        GameObject.Destroy(dc);
    }

    private bool HasTouched() {
        return _getNext;
    }
}

/*
 * Match dialog system doesn't destroy, match system does.
 * Match system gets the component, does the calculation, plays animation, then when it's done allows for continue (does this by setting _getNext to false, or leaving it false, and then setting it to true
 * -- maybe move all the internal stuff to update to make that easier, or leave in update of match dialog system
 * 
 */