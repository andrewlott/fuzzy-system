using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchComponent : BaseComponent {
    public List<int> dice; // list with number of sides each
    public List<int> rolledDice; // outcome
    public int threshold; // TODO: thresholdS to allow for different dice with different thresholds
    //public bool playerHigh; // TODO: Implement

    public bool win;
}
