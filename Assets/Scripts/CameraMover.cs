using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {
	private Transform mainAsteroid;
	private float scale;
	private Rigidbody2D marb;

	public float maxvelocity = 2f;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindGameObjectWithTag ("Asteroid");
		if (temp == null) {
			Debug.Log ("No GameObject found");
			return;
		}
		mainAsteroid = temp.GetComponent<Transform>();
		scale = 1;
		marb = temp.GetComponent<Rigidbody2D> ();
	}

	void Update(){
		//This is for mainAsteroid max speed
		if (marb.velocity.x > maxvelocity) {
			marb.velocity = new Vector2 (maxvelocity, marb.velocity.y);
		}
		if (marb.velocity.y > maxvelocity) {
			marb.velocity = new Vector2 (marb.velocity.x,maxvelocity);
		}
		//Debug.Log (marb.velocity.ToString ());
	}

	// Update is called once per frame
	void LateUpdate () {
		
		this.transform.localPosition = new Vector3 (mainAsteroid.position.x, mainAsteroid.position.y, transform.localPosition.z);
		float tempscale = mainAsteroid.transform.localScale.x / scale;
		if (tempscale > 2.0f || tempscale < 0.5f) {
			StartCoroutine (LerpPos (tempscale));
		}
	}

	public IEnumerator LerpPos(float tempscale){
		yield return null;
	}
}
