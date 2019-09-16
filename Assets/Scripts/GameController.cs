using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : BaseController {
    private static GameController _instance;
    private List<string> _dialogs = new List<string>();

	public TextMeshProUGUI dialogText;
    public GameObject diceHolder;
    public LuckComponent playerLuck;
    public LuckComponent opponentLuck;

    public Animator dialogStateMachine;
    public TextAsset dialogsFile;

    public GameObject d6Prefab;

    public Dictionary<int, GameObject> dicePrefabs = new Dictionary<int, GameObject>();

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
		MatchSystem ms = new MatchSystem();
		AddSystem(ms);

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
        Setup();
    }

    private void ScaleCamera() {
        float sceneWidth = 5.5f;
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        mainCamera.orthographicSize = desiredHalfHeight;
    }

    private void Setup() {
        foreach (string line in dialogsFile.text.Split('\n')) {
            if (line.Length == 0) {
                continue;
            }
            Debug.Log(line);
            int startIndex = line.IndexOf(':');
            int id = System.Int32.Parse(line.Substring(0, startIndex));
            string dialog = line.Substring(startIndex + 2);
            _dialogs.Add(dialog);
        }

        dicePrefabs.Add(2, d6Prefab);
        dicePrefabs.Add(6, d6Prefab);
        dicePrefabs.Add(10, d6Prefab);
        dicePrefabs.Add(20, d6Prefab);
    }

    public void DisplayDialog(int dialogId) {
        gameObject.AddComponent<DialogComponent>().dialog = _dialogs[dialogId];
    }

}
