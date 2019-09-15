using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchComponent : BaseComponent {
    public List<int> dice; // list with number of sides each
    public int threshold;
    //public bool playerHigh; // TODO: Implement

    public bool win;
}
