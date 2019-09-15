using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LuckSystem : BaseSystem {
    public static float LUCKPOINT = 0.10f; // 10% skew

    public override void Start() {
        Pool.Instance.AddSystemListener(typeof(LuckComponent), this);
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(LuckComponent), this);
    }

    public override void Update() {

    }

    public override void OnComponentAdded(BaseComponent c) {
        if (c is LuckComponent) {

        }
    }

    public override void OnComponentRemoved(BaseComponent c) {
        if (c is LuckComponent) {
        }
    }
}
