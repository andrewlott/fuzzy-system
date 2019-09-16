using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dice : MonoBehaviour {
    public List<RawImage> faces;
    public List<Texture> sides;

    public virtual void SetFace(int number) {
        int index = number - 1;
        List<int> usedIndexes = new List<int> { index };
        faces[0].texture = sides[index];
        for (int i = 1; i < faces.Count; i++) {
            while (usedIndexes.Contains(index)) {
                index = Utils.RandomInt(sides.Count);
            }
            faces[i].texture = sides[index];
            usedIndexes.Add(index);
        }
    }

    public virtual void SetOutcomeColors(bool win) {
        faces[0].color = win ? Color.green : Color.red;
    }
}
