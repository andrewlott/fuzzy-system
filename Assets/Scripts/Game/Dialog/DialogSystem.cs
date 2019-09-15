using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogSystem : BaseSystem {
    private static float _RATE = 0.05f; // seconds per character
    private float _startTime;
    private TextMeshProUGUI _tmp;
    private bool _getNext;
    private bool _waiting;

    public override void Start() {
        _tmp = GameController.Instance.dialogText;
        Pool.Instance.AddSystemListener(typeof(DialogComponent), this);
        Pool.Instance.AddSystemListener(typeof(TouchComponent), this);
        BaseObject.AddComponent<DialogComponent>().dialog = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc risus felis, varius in aliquam sit amet, hendrerit eget massa. Morbi porta accumsan varius. Nam sit amet augue ultricies ligula gravida fringilla non ac risus. Fusce dapibus metus urna, rhoncus hendrerit risus sodales in. Phasellus arcu augue, vestibulum nec vestibulum vel, rutrum et lorem. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Fusce venenatis velit ligula, id bibendum lectus malesuada non. Cras in placerat dolor, vitae aliquet nunc. Integer ut lectus bibendum, egestas erat ac, ullamcorper dui.";
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
            _startTime = Time.time;
            DisplayDialog(dc.dialog);
        } else if (c is TouchComponent) {
            if (_waiting) {
                _getNext = true;
            }
        }
    }

    public override void OnComponentRemoved(BaseComponent c) {
        if (c is DialogComponent) {
            _tmp.text = "";
        }
    }

    private void DisplayDialog(string dialog) {
        GameController.Instance.HandleCoroutine(InternalDisplayDialog(dialog));
    }

    private IEnumerator InternalDisplayDialog(string dialog) {
        int len = 0;
        _tmp.text = dialog;
        _tmp.maxVisibleCharacters = 0;
        while (_tmp.maxVisibleCharacters < dialog.Length) {
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
    }

    private bool HasTouched() {
        return _getNext;
    }
}
