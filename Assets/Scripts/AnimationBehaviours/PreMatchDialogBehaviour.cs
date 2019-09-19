using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PreMatchDialogBehaviour : DialogBehaviour {
    public string item;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameController.Instance.opponentLuck.item = item;
        GameController.Instance.DisplayPreMatchDialog(dialogId);
        if (enterFunction != null) {
            enterFunction.Invoke();
        }
    }
}
