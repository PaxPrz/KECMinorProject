using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeIncreaser : MonoBehaviour {
	public GameObject sound;
	public GameObject explosion;
	public float divratio = 10f;
	// Use this for initialization


	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Asteroid") {
			
		} else if (gameObject.tag == col.gameObject.tag) {
			if (gameObject.transform.localScale.x > col.gameObject.transform.localScale.x) {
				gameObject.transform.localScale *= (1 + col.gameObject.transform.localScale.x / divratio);
				Destroy (col.gameObject);
				//StartCoroutine (ExplosionDestroyer (gameObject.transform));
			}

		} else if(gameObject.transform.tag!="Asteroid" && col.gameObject.transform.tag!="Asteroid"){
			Destroy (gameObject);
			Destroy (col.gameObject);

			//StartCoroutine (ExplosionDestroyer (gameObject.transform));
		}
		//for Main Asteroid
		if (col.gameObject.tag == "AsteroidWhite" && gameObject.tag == "Asteroid") {
			gameObject.transform.localScale *= (1 + col.gameObject.transform.localScale.x / divratio);
			Destroy (col.gameObject);
		} else if (col.gameObject.tag != "AsteroidWhite" && gameObject.tag == "Asteroid") {
			gameObject.transform.localScale *= (1 - col.gameObject.transform.localScale.x / divratio);
			Destroy (col.gameObject);

			//StartCoroutine (ExplosionDestroyer (gameObject.transform));
		}
	}
	/*
	private IEnumerator ExplosionDestroyer(Transform t){
		Debug.Log ("Explosion here");
		GameObject s = Instantiate (sound, t);
		GameObject e = Instantiate (explosion, t);
		new WaitForSeconds (2f);
		//Destroy (s);
		Destroy (e);
		yield return null;

			GameObject s = Instantiate (sound, gameObject.transform);
			GameObject e = Instantiate (explosion, gameObject.transform);
	}*/
}
