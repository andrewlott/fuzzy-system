﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(TouchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(TouchComponent), this);
	}

    public override void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            BaseObject.AddComponent<TouchComponent>();
        }

    }

    public override void OnComponentAdded(BaseComponent c) {
		if (c is TouchComponent) {
            GameObject.Destroy(c);
        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is TouchComponent) {

		}
	}

    private void OnTouch() {

    }
}