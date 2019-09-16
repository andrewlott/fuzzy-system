using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MatchSystem : BaseSystem {
    private static float _animationRate = 0.075f;
    private static int _maxAnimationTimes = 15;
    private static int _animatingCounter;

    public override void Start() {
        Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
        //int sides = 2;
        //for (int i = 1; i <= sides; i++) {
        //    MatchComponent mc = BaseObject.AddComponent<MatchComponent>();
        //    mc.dice = new List<int> { sides };
        //    mc.threshold = i;
        //}
        //sides = 6;
        //for (int i = 1; i <= sides; i++) {
        //    MatchComponent mc = BaseObject.AddComponent<MatchComponent>();
        //    mc.dice = new List<int> { sides };
        //    mc.threshold = i;
        //}
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
            AnimateDice(mc);
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

        mc.rolledDice = new List<int>(mc.dice);
        int diff = totalSides - number;
        for (int i = 0; i < diff; i++) {
            int index = Utils.RandomInt(mc.rolledDice.Count);
            mc.rolledDice[index]--;
        }
        Debug.Log(string.Format("Win: {0}", mc.win));
        Debug.Log("Dice:");
        foreach (int n in mc.rolledDice) {
            Debug.Log(string.Format(" {0}", n));
        }
    }

    private void AnimateDice(MatchComponent mc) {
        for (int i = 0; i < mc.dice.Count; i++) {
            int sides = mc.dice[i];
            int outcome = mc.rolledDice[i];
            GameObject prefab = GameController.Instance.dicePrefabs[sides];
            GameObject die = GameObject.Instantiate(prefab, GameController.Instance.diceHolder.transform);
            GameController.Instance.HandleCoroutine(AnimateDie(die, sides, outcome, mc));
        }
    }

    private IEnumerator AnimateDie(GameObject die, int sides, int outcome, MatchComponent mc) {
        _animatingCounter++;
        // TODO: Use target transform and bounce to it
        float counter = 0;
        while (counter < _maxAnimationTimes) {
            // TODO: Change faces
            die.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("{0}", 1 + Utils.RandomInt(sides));
            yield return new WaitForSeconds(counter * _animationRate);
            counter++;
        }
        // TODO: Change face one last time
        die.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("{0}", outcome);
        _animatingCounter--;
        if (_animatingCounter == 0) {
            OnComplete(mc);
        }
    }


    private void OnComplete(MatchComponent mc) {
        GameObject.Destroy(mc);
    }
}
