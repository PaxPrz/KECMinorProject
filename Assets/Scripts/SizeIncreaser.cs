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
	public GameObject cam;
	public float maxBgVel=2f;
	//private Spawner spawn;
	// Use this for initialization

	void Start(){		
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		bgMovement = GameObject.FindGameObjectWithTag ("BgMovement");
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");
		if (gameObject.tag == "Asteroid" && SceneManager.GetActiveScene()==SceneManager.GetSceneByName("Game")) {
			bgMovementVel = bgMovement.GetComponent<Rigidbody2D> ();
			//spawn = gameManager.GetComponent<Spawner> ();
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
			//Debug.Log ("imapact: " + impactForce.ToString ());
			//impactForce = col.gameObject.transform.localScale.x * impactForce ;

			for (int i = 0; i < Spawner.asteroidList.Count; i++) {
				if (Spawner.asteroidList [i] == null) {
					Spawner.asteroidList.RemoveAt (i);
					continue;
				}
				Rigidbody2D rb = (Spawner.asteroidList [i].GetComponent<Rigidbody2D> ());
				if (Spawner.asteroidList [i].tag == "BlackHole") {
					rb.velocity = resultantVector (rb.velocity, -0.5f * impactForce);	
				} else {
					rb.velocity = resultantVector (rb.velocity, -0.3f*impactForce);
				}
			}
			if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Game")) {
				bgMovementVel.velocity = resultantVector (bgMovementVel.velocity, impactForce);
				if (bgMovementVel.velocity.magnitude > maxBgVel) {
					bgMovementVel.velocity = bgMovementVel.velocity.normalized * maxBgVel;
					Debug.Log ("BGVelocity full");
				}
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
				float temp = (col.gameObject.transform.localScale.x / (divratio *cam.GetComponent<CameraMover>().CurrentRatio));
				gameObject.transform.localScale = new Vector2 (gameObject.transform.localScale.x + temp, gameObject.transform.localScale.y + temp);

				callExplosion (col);

				Destroy (col.gameObject);
				touched = false;
				if (!PlaySettings.paused && gameObject.GetComponent<SpriteRenderer>().isVisible) {
					PlaySettings.score += 1;
				}
				//StartCoroutine (ExplosionDestroyer (gameObject.transform));
			}

		} else if(gameObject.transform.tag!="Asteroid" && col.gameObject.transform.tag!="Asteroid"){
			callExplosion (col);
			bool a;
			if ((a=(gameObject.transform.localScale.x > 5f * col.gameObject.transform.localScale.x)) || ((col.gameObject.transform.localScale.x > 5f * gameObject.transform.localScale.x))) {
				if(a){
					Destroy(col.gameObject);
				}
				else{
					Destroy(gameObject);
				}
			} else {
				Destroy (gameObject);
				Destroy (col.gameObject);
			}
			if (!PlaySettings.paused && gameObject.GetComponent<SpriteRenderer>().isVisible) {
				PlaySettings.score += 1;
			}
			//StartCoroutine (ExplosionDestroyer (gameObject.transform));
		}
		//for Main Asteroid
		if (col.gameObject.tag == "AsteroidWhite" && gameObject.tag == "Asteroid") {
			float temp = (col.gameObject.transform.localScale.x / (divratio *cam.GetComponent<CameraMover>().CurrentRatio));
			gameObject.transform.localScale = new Vector2 (gameObject.transform.localScale.x + temp, gameObject.transform.localScale.y + temp);

			callExplosion (col);
			Destroy (col.gameObject);
			if (!PlaySettings.paused) {
				PlaySettings.score += 1;
			}


		} else if (col.gameObject.tag != "AsteroidWhite" && gameObject.tag == "Asteroid") {
			float temp = -(col.gameObject.transform.localScale.x / divratio);
			if (col.gameObject.tag == "Comet") {
				gameObject.transform.localScale = new Vector2 (gameObject.transform.localScale.x + 4f * temp, gameObject.transform.localScale.y + 4f * temp);
			} else {
				gameObject.transform.localScale = new Vector2 (gameObject.transform.localScale.x + temp, gameObject.transform.localScale.y + temp);
			}
			if (GameSettings.isVibrate) {
				cam.GetComponent<CameraMover> ().shakeCamera ();
			}
			callExplosion (col);
			Destroy (col.gameObject);
		}
	}

	void callExplosion(Collision2D col){
		if (col.gameObject.tag != "Comet") {
			if (col.gameObject.GetComponent<SpriteRenderer> ().isVisible) {
				GameObject explosion = Instantiate (PlayerExplosion, transform.position, transform.rotation);
				explosion.transform.localScale = col.gameObject.transform.localScale / 5f;
				GameObject soundObj = Instantiate (sound);
				StartCoroutine (DestroySound (soundObj));

			}
		} else {
			GameObject explosion = Instantiate (PlayerExplosion, transform.position, transform.rotation);
			explosion.transform.localScale = col.gameObject.transform.localScale / 5f;
			GameObject soundObj = Instantiate (sound);
			StartCoroutine (DestroySound (soundObj));
		}
	}

	private Vector2 resultantVector(Vector2 prev, Vector2 force){
		return new Vector2 (prev.x + force.x, prev.y + force.y);
	}

	private IEnumerator DestroySound(GameObject g){
		yield return new WaitForSeconds (2.0f);
		Destroy (g);
		yield return null;
	}
}
