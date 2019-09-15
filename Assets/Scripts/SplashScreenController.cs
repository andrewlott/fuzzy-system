using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {

	void Start () {

    }

	public void EnterGame() {
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}
}
