using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {
	public static GameObject _instance;
	public GameObject[] asteroids = new GameObject[4];
	//private bool showMainCanvas = true;
	//private bool showOptionCanvas = false;
	public GameObject mainCanvas;
	public GameObject optionCanvas;
	private bool stopActivity = false;
	// Use this for initialization


	void Awake(){
		if (_instance == null) {
			_instance = this.gameObject;
			DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (gameObject);
		}
		/*
		if (_instance != null) {
			GameObject temp = this.gameObject;
			Destroy (temp);
			_instance.GetComponent<UIScript>().stopActivity = false;
			_instance.GetComponent<UIScript>().StartCoroutine (InvisibleAsteroid ());
			_instance.GetComponent<Movement2D> ().enabled = true;
			_instance.GetComponent<UIScript>().enabled = true;
		}
		_instance = this.gameObject;
		*/
	}

	void Start () {
		mainCanvas.SetActive (true);
		optionCanvas.SetActive (false);
		Rigidbody2D rb;
		foreach (GameObject g in asteroids) {
			if (g != null) {
				//g.transform.rotation = new Quaternion (Random.Range (-180, 180), 0,0, 0);
				if((rb = g.GetComponent<Rigidbody2D>())!=null){
					rb.AddTorque(Random.Range(-10f,10f));
				}
			}
		}
		StartCoroutine (InvisibleAsteroid ());
	}


	public IEnumerator InvisibleAsteroid(){
		while (true) {
			if (!stopActivity) {
				foreach (GameObject g in asteroids) {
					if (!(g.GetComponent<Renderer> ().isVisible)) {
						g.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);
						DeterminePosition (g);
					}
				}
			}
			yield return null;
		}
	}

	public void GoOptions(){
		Reset ();
		mainCanvas.SetActive (false);
		optionCanvas.SetActive (true);
		showAsteroids ();
	}

	public void GoBack(){
		Reset ();
		optionCanvas.SetActive (false);
		mainCanvas.SetActive (true);
		showAsteroids ();
	}

	public void GoQuit(){
		Application.Quit ();
	}

	public void GoPlay(){
		Reset ();
		stopActivity = true;
		StopCoroutine (InvisibleAsteroid ());
		gameObject.GetComponent<Movement2D> ().enabled = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		gameObject.GetComponent<UIScript> ().enabled = false;
	}

	private void showAsteroids(){
		foreach (GameObject g in asteroids) {
			g.SetActive (!g.activeInHierarchy);
		}
	}

	public void Reset(){
		foreach (GameObject g in asteroids) {
			g.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);

		}
	}

	private void DeterminePosition(GameObject g){
		if (g.CompareTag ("Asteroid")) {
			g.transform.position = new Vector2 (0f, 0f);
		} else if (g.CompareTag ("AsteroidWhite")) {
			g.transform.position = new Vector2 (0f, -3f);
		} else if (g.CompareTag ("AsteroidRed")) {
			g.transform.position = new Vector2 (-4f, -3f);
		} else if (g.CompareTag ("AsteroidGreen")) {
			g.transform.position = new Vector2 (4f, -3f);
		}
	}
}
