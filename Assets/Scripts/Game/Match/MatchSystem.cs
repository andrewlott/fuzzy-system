using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MatchSystem : BaseSystem {
    /*
     * gets player wagered luck
     * gets opponent wagered luck
     * gets all dice
     * gets win threshold, converts total to 0-1 scale
     * apply luck (add/sub wagered luck points on both sides applied by LUCKPOINTs)
     * calculates total from all dice rolled
     * display win or loss
     */

    public override void Start() {
        Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
        for (int i = 1; i < 7; i++) {
            MatchComponent mc = BaseObject.AddComponent<MatchComponent>();
            mc.dice = new List<int> { 6 };
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
        }
    }

    private void DetermineMatch(MatchComponent mc) {
        float playerSkew = GameController.Instance.playerLuck.luck * LuckSystem.LUCKPOINT;
        float opponentSkew = GameController.Instance.opponentLuck.luck * LuckSystem.LUCKPOINT;
        float totalSkew = playerSkew - opponentSkew;

        int totalSides = mc.dice.Sum();
        int threshold = mc.threshold;
        float winPercent = Mathf.Max(Mathf.Min(((float)threshold) / ((float)totalSides) + totalSkew, 1.0f), 0.0f);
        // TODO: SHOULD BE 0% CHANCE OF WINNING IF THRESHOLD IS 1

        //Debug.Log(string.Format("Player Luck: {0}", playerSkew / LuckSystem.LUCKPOINT));
        //Debug.Log(string.Format("Opponent Luck: {0}", opponentSkew / LuckSystem.LUCKPOINT));
        //Debug.Log(string.Format("Total Skew: {0}", totalSkew));
        Debug.Log(string.Format("Total Sides: {0}", totalSides));
        Debug.Log(string.Format("Dice:"));
        foreach(int n in mc.dice) {
            Debug.Log(string.Format(" {0}", n));
        }
        Debug.Log(string.Format("Threshold: {0}", threshold));
        Debug.Log(string.Format("Win %: {0}", winPercent));

        bool win = Utils.RandomFloat(1.0f) <= winPercent;
        int number = win ?
            1 + Utils.RandomInt(threshold - 1):
            threshold + Utils.RandomInt(totalSides - threshold + 1);
        int diff = totalSides - number;
        for (int i = 0; i < diff; i++) {
            int index = Utils.RandomInt(mc.dice.Count);
            mc.dice[index]--;
        }
        Debug.Log(string.Format("Win: {0}", win));
        Debug.Log("Dice:");
        foreach(int n in mc.dice) {
            Debug.Log(string.Format(" {0}", n));
        }
    }
}
