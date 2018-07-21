using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlaySettings : MonoBehaviour {
	[SerializeField]
	public static bool paused;
	public static int highScore;
	public static int score = 0;
	public Canvas pauseCanvas;
	public Canvas gameOverCanvas;
	private GameObject UIManager;
	private GameSettings settings;
	public GameObject mainAsteroid;
	public TextMeshProUGUI hiScoreText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI GOscoreText;
	// Use this for initialization

	void Awake(){
		highScore = PlayerPrefs.GetInt ("highScore", 0);
	}

	void Start () {
		UIManager = GameObject.FindGameObjectWithTag ("UIManager");
		if (UIManager != null) {
			settings = UIManager.GetComponent<GameSettings> ();
		}
		score = 0;
		hiScoreText.text = highScore.ToString ();
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (mainAsteroid.transform.localScale.x <= 0.6f) {
			GameOver ();
		}
		if (!paused) {
			scoreText.text = score.ToString ();
		}
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

	public void GameOver(){
		if (score > highScore) {
			PlayerPrefs.SetInt ("highScore", score);
		}
		paused = true;
		GOscoreText.text = score.ToString ();
		gameOverCanvas.gameObject.SetActive (true);
	}

}
