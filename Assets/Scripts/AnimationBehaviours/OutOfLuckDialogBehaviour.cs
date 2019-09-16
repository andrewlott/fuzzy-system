using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLuckDialogBehaviour : DialogBehaviour {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameController.Instance.DisplayDialog(dialogId);
        if (enterFunction != null) {
            enterFunction.Invoke();
        }
        GameController.Instance.OutOfLuck();
    }
}
