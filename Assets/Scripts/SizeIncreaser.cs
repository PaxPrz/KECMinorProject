using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SizeIncreaser : MonoBehaviour {
	public GameObject sound;
	public GameObject PlayerExplosion;
	public float divratio = 5f;
	private Vector2 impactForce;
	public GameObject bgMovement;
	private Rigidbody2D bgMovementVel=null;
	public bool touched = false;
	public bool wasVisible = false;
	public GameObject gameManager;
	private Spawner spawn;
	// Use this for initialization

	void Start(){
		if (gameObject.tag == "Asteroid" && SceneManager.GetActiveScene()==SceneManager.GetSceneByName("Game")) {
			bgMovementVel = bgMovement.GetComponent<Rigidbody2D> ();
			spawn = gameManager.GetComponent<Spawner> ();
		}
	}

	void Update(){
		if (!wasVisible) {
			if (gameObject.GetComponent<SpriteRenderer> ().isVisible) {
				wasVisible = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (gameObject.tag == "Asteroid") {
			impactForce = col.relativeVelocity;
			Debug.Log ("imapact: " + impactForce.ToString ());
			impactForce = col.gameObject.transform.localScale.x * impactForce ;

			for (int i = 0; i < Spawner.asteroidList.Count; i++) {
				if (Spawner.asteroidList [i] == null) {
					Spawner.asteroidList.RemoveAt (i);
					continue;
				}
				Rigidbody2D rb = (Spawner.asteroidList [i].GetComponent<Rigidbody2D> ());
				if (Spawner.asteroidList [i].tag == "BlackHole") {
					rb.velocity = resultantVector (rb.velocity, -0.5f * impactForce);	
				} else {
					rb.velocity = resultantVector (rb.velocity, -impactForce);
				}
			}
			if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Game")) {
				bgMovementVel.velocity = resultantVector (bgMovementVel.velocity, impactForce);
			}
			/*
			if (spawn.blackHole != null) {
				if (spawn.blackHole.GetComponent<Rigidbody2D> () != null) {
					spawn.blackHole.GetComponent<Rigidbody2D> ().velocity = resultantVector (spawn.blackHole.GetComponent<Rigidbody2D> ().velocity, -impactForce);
					Debug.Log ("New B vel: " + spawn.blackHole.GetComponent<Rigidbody2D> ().velocity.ToString ());
				}
			}
			*/
		} 

		if (col.gameObject.tag == "Asteroid") {
			//Do nothing
		}else if (gameObject.tag == col.gameObject.tag) {
			

			if (gameObject.transform.localScale.x > col.gameObject.transform.localScale.x) {
				gameObject.transform.localScale *= (1 + col.gameObject.transform.localScale.x / divratio);
				callExplosion (col);

				Destroy (col.gameObject);
				touched = false;
				//StartCoroutine (ExplosionDestroyer (gameObject.transform));
			}

		} else if(gameObject.transform.tag!="Asteroid" && col.gameObject.transform.tag!="Asteroid"){
			callExplosion (col);
			Destroy (gameObject);
			Destroy (col.gameObject);

			//StartCoroutine (ExplosionDestroyer (gameObject.transform));
		}
		//for Main Asteroid
		if (col.gameObject.tag == "AsteroidWhite" && gameObject.tag == "Asteroid") {



			gameObject.transform.localScale *= (1 + col.gameObject.transform.localScale.x / divratio);

			callExplosion (col);
			Destroy (col.gameObject);


		} else if (col.gameObject.tag != "AsteroidWhite" && gameObject.tag == "Asteroid") {
			gameObject.transform.localScale *= (1 - col.gameObject.transform.localScale.x / divratio);
			if (GameSettings.isVibrate) {
				Debug.Log ("Must Vibrate");
				Camera.main.GetComponent<CameraMover> ().shakeCamera ();
			}
			callExplosion (col);
			Destroy (col.gameObject);


			//StartCoroutine (ExplosionDestroyer (gameObject.transform));
		}
	}

	void callExplosion(Collision2D col){
		if (col.gameObject.tag != "Comet") {
			if (col.gameObject.GetComponent<SpriteRenderer> ().isVisible) {
				GameObject explosion = Instantiate (PlayerExplosion, transform.position, transform.rotation);
				explosion.transform.localScale = col.gameObject.transform.localScale / 5f;
				Instantiate (sound);
			}
		} else {
			GameObject explosion = Instantiate (PlayerExplosion, transform.position, transform.rotation);
			explosion.transform.localScale = col.gameObject.transform.localScale / 5f;
			Instantiate (sound);
		}
	}

	private Vector2 resultantVector(Vector2 prev, Vector2 force){
		return new Vector2 (prev.x + force.x, prev.y + force.y);
	}
}
