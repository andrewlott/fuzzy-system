using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : BaseController {
    private static GameController _instance;

	public TextMeshProUGUI dialogText;

    public Camera mainCamera;

    public static GameController Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.Find("GameController").GetComponent<GameController>();
            }
            return _instance;
        }
    }

    void Start() {
        // Add systems here
        DialogSystem dls = new DialogSystem();
        AddSystem(dls);

		AnimationSystem ans = new AnimationSystem();
        AddSystem(ans);
        TouchSystem ts = new TouchSystem();
        AddSystem(ts);
        UISystem uis = new UISystem();
        AddSystem(uis);
        PauseSystem ps = new PauseSystem();
        AddSystem(ps);
        DestroySystem ds = new DestroySystem();
        AddSystem(ds);

        //AdSystem ads = new AdSystem();
        //AddSystem(ads);

        Enable();
    }

    private void ScaleCamera() {
        float sceneWidth = 5.5f;
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        mainCamera.orthographicSize = desiredHalfHeight;
    }

}
