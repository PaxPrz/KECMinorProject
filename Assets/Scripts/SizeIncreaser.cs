﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeIncreaser : MonoBehaviour {
	public GameObject sound;
	public GameObject explosion;
	public float divratio = 5f;
	private Vector2 impactForce;
	public GameObject bgMovement;
	private Rigidbody2D bgMovementVel;
	// Use this for initialization

	void Start(){
		if (gameObject.tag == "Asteroid") {
			bgMovementVel = bgMovement.GetComponent<Rigidbody2D> ();
		}
	}


	void OnCollisionEnter2D(Collision2D col){
		if (gameObject.tag == "Asteroid") {
			impactForce = col.relativeVelocity;
			impactForce = col.gameObject.transform.localScale.x * impactForce ;

			for (int i = 0; i < Spawner.asteroidList.Count; i++) {
				if (Spawner.asteroidList [i] == null) {
					continue;
				}
				Rigidbody2D rb = (Spawner.asteroidList [i].GetComponent<Rigidbody2D> ());
				rb.velocity = resultantVector (rb.velocity, -impactForce);
			}
			bgMovementVel.velocity = resultantVector (bgMovementVel.velocity, impactForce);

		} 

		if (col.gameObject.tag == "Asteroid") {
			//Do nothing
		}else if (gameObject.tag == col.gameObject.tag) {
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

	private Vector2 resultantVector(Vector2 prev, Vector2 force){
		/*
		float cosTheta, mag;
		float prevMag = prev.magnitude;
		float forceMag = force.magnitude;
		cosTheta = Mathf.Cos (Vector2.Angle (prev, force));
		mag = Mathf.Sqrt (Mathf.Pow (prevMag, 2) + Mathf.Pow (forceMag, 2) + 2 * prevMag * forceMag * cosTheta);
		*/
		return new Vector2 (prev.x + force.x, prev.y + force.y);
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
