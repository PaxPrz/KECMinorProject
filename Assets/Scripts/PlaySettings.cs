using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySettings : MonoBehaviour {
	[SerializeField]
	public static bool paused;
	public Canvas pauseCanvas;
	private GameObject UIManager;
	private GameSettings settings;
	// Use this for initialization
	void Start () {
		UIManager = GameObject.FindGameObjectWithTag ("UIManager");
		if (UIManager != null) {
			settings = UIManager.GetComponent<GameSettings> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onPause(){
		if(!paused){
			paused = true;
			Time.timeScale = 0f;
			pauseCanvas.gameObject.SetActive (true);
		}
	}

	public void onPlay(){
		if (paused) {
			paused = false;
			Time.timeScale = 1.0f;
			pauseCanvas.gameObject.SetActive (false);
		}
	}

	public void onBack(){
		Time.timeScale = 1.0f;
		UIManager.GetComponent<Movement2D> ().enabled = true;
		paused = false;
		SceneManager.LoadScene ("UI");
		if (UIManager != null) {
			Destroy (UIManager);
		}
	}

	public void onVibrate(){
		GameSettings.isVibrate = !GameSettings.isVibrate;
		if (settings == null) {
			Debug.Log ("Settings not available");	
			return;
		}
		settings.PlayVibrate (GameSettings.isVibrate);
	}

	public void onSound(){
		GameSettings.isSound = !GameSettings.isSound;
		if (settings == null) {
			Debug.Log ("Settings not available");
			return;
		}
		settings.PlaySound (GameSettings.isSound);			
	}

	public void onRestart(){
		SceneManager.LoadScene ("Game");
		Time.timeScale = 1.0f;
		paused = false;
	}

}
