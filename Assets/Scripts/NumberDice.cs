using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumberDice : Dice {
    public int totalSides;
    public List<TMPro.TextMeshProUGUI> nFaces;

    public override void SetFace(int number) {
        List<int> usedNumbers = new List<int> { number };
        nFaces[0].text = string.Format("{0}", number);
        for (int i = 1; i < nFaces.Count; i++) {
            while (usedNumbers.Contains(number)) {
                number = Utils.RandomInt(totalSides);
            }
            nFaces[i].text = string.Format("{0}", number);
            usedNumbers.Add(number);
        }
    }

    public override void SetOutcomeColors(bool win) {
        nFaces[0].color = win ? Color.green : Color.red;
    }
}
