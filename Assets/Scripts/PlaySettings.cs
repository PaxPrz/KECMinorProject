using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySettings : MonoBehaviour {
	[SerializeField]
	public static bool paused;
	public Canvas pauseCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onPause(){
		paused = true;
		Time.timeScale = 0f;
		pauseCanvas.gameObject.SetActive (true);
	}
}
