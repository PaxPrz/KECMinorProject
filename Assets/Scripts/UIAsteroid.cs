using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAsteroid : MonoBehaviour {
	public GameObject UIManager;
	// Use this for initialization

	void OnCollisionEnter2D(Collision2D col){
		Debug.Log ("Coliison occur");
		if (col.transform.CompareTag ("AsteroidWhite")) {
			UIManager.GetComponent<UIScript> ().GoPlay ();
		}
		else if(col.transform.CompareTag ("AsteroidRed")) {
			UIManager.GetComponent<UIScript> ().GoQuit ();
		}
		else if(col.transform.CompareTag ("AsteroidGreen")) {
			Debug.Log ("We are green");
				UIManager.GetComponent<UIScript> ().GoOptions();
		}
	}
}
