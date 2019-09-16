using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dice : MonoBehaviour {
    public RawImage face;
    public List<Texture> sides;

    public void SetFace(int index) {
        face.texture = sides[index];
    }
}
