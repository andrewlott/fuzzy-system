using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dice : MonoBehaviour {
    public List<RawImage> faces;
    public List<Texture> sides;

    public void SetFace(int index) {
        List<int> usedIndexes = new List<int> { index };
        faces[0].texture = sides[index];
        for (int i = 1; i < faces.Count; i++) {
            int idx = Utils.RandomInt(sides.Count);
            while (usedIndexes.Contains(idx)) {
                idx = Utils.RandomInt(sides.Count);
            }
            faces[i].texture = sides[idx];
            usedIndexes.Add(idx);
        }
    }
}
