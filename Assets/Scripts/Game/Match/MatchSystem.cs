using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MatchSystem : BaseSystem {

    public override void Start() {
        Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
        int sides = 2;
        for (int i = 1; i <= sides; i++) {
            MatchComponent mc = BaseObject.AddComponent<MatchComponent>();
            mc.dice = new List<int> { sides };
            mc.threshold = i;
        }
        sides = 6;
        for (int i = 1; i <= sides; i++) {
            MatchComponent mc = BaseObject.AddComponent<MatchComponent>();
            mc.dice = new List<int> { sides };
            mc.threshold = i;
        }
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
    }

    public override void Update() {

    }

    public override void OnComponentAdded(BaseComponent c) {
        if (c is MatchComponent) {
            MatchComponent mc = c as MatchComponent;
            DetermineMatch(mc);
            GameObject.Destroy(c);
        }
    }

    public override void OnComponentRemoved(BaseComponent c) {
        if (c is MatchComponent) {
            MatchComponent mc = c as MatchComponent;
            if (mc.win) {
                GameController.Instance.dialogStateMachine.SetTrigger("GoodTrigger");
            } else {
                GameController.Instance.dialogStateMachine.SetTrigger("BadTrigger");
            }
        }
    }

    private void DetermineMatch(MatchComponent mc) {
        float playerSkew = GameController.Instance.playerLuck.luck * LuckSystem.LUCKPOINT;
        float opponentSkew = GameController.Instance.opponentLuck.luck * LuckSystem.LUCKPOINT;
        float totalSkew = playerSkew - opponentSkew;

        int totalSides = mc.dice.Sum();
        int threshold = mc.threshold - 1;
        float winPercent = Mathf.Max(Mathf.Min(((float)threshold) / ((float)totalSides) + totalSkew, 1.0f), 0.0f);

        //Debug.Log(string.Format("Dice:"));
        //foreach(int n in mc.dice) {
        //    Debug.Log(string.Format(" {0}", n));
        //}
        //Debug.Log(string.Format("Threshold: {0}", threshold));
        //Debug.Log(string.Format("Win %: {0}", winPercent));

        mc.win = Utils.RandomFloat(1.0f) <= winPercent;
        int number = mc.win ?
            1 + Utils.RandomInt(threshold - 1):
            threshold + Utils.RandomInt(totalSides - threshold + 1);
        int diff = totalSides - number;
        for (int i = 0; i < diff; i++) {
            int index = Utils.RandomInt(mc.dice.Count);
            mc.dice[index]--;
        }
        //Debug.Log(string.Format("Win: {0}", win));
        //Debug.Log("Dice:");
        //foreach(int n in mc.dice) {
        //    Debug.Log(string.Format(" {0}", n));
        //}
    }
}
