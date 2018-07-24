using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {
	public GameObject mainAsteroid;
	private float scale, defaultScale;
	private int currentRatio;
	public int CurrentRatio{
		get{ 
			return currentRatio;
		}
		set{ 
			currentRatio = value;
		}
	}
	private Camera cam;

	//For Shake-----------
	public float shakeTime = 1.0f;
	public float shakeAmount = 3.0f;
	public float shakeSpeed = 2.0f;
	//---------------------

	public float maxvelocity = 2f;
	private float temp;
	private float t;

	// Use this for initialization
	void Start () {
		cam = gameObject.GetComponent<Camera> ();
		if (GetComponent<Camera>() == null) {
			Debug.Log ("no camera");
		}
		defaultScale = (float)mainAsteroid.transform.localScale.x/cam.orthographicSize;
		currentRatio = 1;
		t=0f;
	}

	void Update(){
		if (mainAsteroid.transform.localScale.x > 2.0f && currentRatio == 1) {
			if (t < 1f) {
				if (t == 0f) {
					temp = cam.orthographicSize;
				}
				cam.orthographicSize = Mathf.Lerp (temp, 2f *temp, t);
				t += Time.deltaTime;
				return;
			}
			currentRatio = 2;
			t = 0f;
		} else if (mainAsteroid.transform.localScale.x < 1.8f && currentRatio == 2) {
			if (t < 1f) {
				if (t == 0f) {
					temp = cam.orthographicSize;
				}
				cam.orthographicSize = Mathf.Lerp (temp, 0.5f * temp, t);
				t += Time.deltaTime;
				return;
			}
			currentRatio = 1;
			t = 0f;
		}
		else if(mainAsteroid.transform.localScale.x >3.0f && currentRatio == 2){
			if(t<1f){
				if(t==0f){
					temp = cam.orthographicSize;
				}
				cam.orthographicSize = Mathf.Lerp(temp, 2*temp, t);
				t+= Time.deltaTime;
				return;
			}
			currentRatio = 3;
			t = 0f;
		}
		else if(mainAsteroid.transform.localScale.x <2.8f && currentRatio == 3){
			if(t<1f){
				if(t==0f){
					temp = cam.orthographicSize;
				}
				cam.orthographicSize = Mathf.Lerp(temp, 0.5f*temp, t);
				t+= Time.deltaTime;
				return;
			}
			currentRatio = 2;
			t = 0f;
		}
		else if(mainAsteroid.transform.localScale.x >4.0f && currentRatio == 3){
			if(t<1f){
				if(t==0f){
					temp = cam.orthographicSize;
				}
				cam.orthographicSize = Mathf.Lerp(temp, 2f*temp, t);
				t+= Time.deltaTime;
				return;
			}
			currentRatio = 4;
			t = 0f;
		}
		else if(mainAsteroid.transform.localScale.x < 3.8f && currentRatio == 4){
			if(t<1f){
				if(t==0f){
					temp = cam.orthographicSize;
				}
				cam.orthographicSize = Mathf.Lerp(temp, 0.5f*temp, t);
				t+= Time.deltaTime;
				return;
			}
			currentRatio = 3;
			t = 0f;
		}

		//scale = (float)mainAsteroid.transform.localScale.x/cam.orthographicSize;
		/*
		if (camera.orthographicSize<=temp) {
			camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, 2f * camera.orthographicSize, t);
			t = Time.deltaTime;
		}*/
		/*
		if (temp <= 0.5f * scale) {
			scale = temp;
			camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, 0.5f * camera.orthographicSize, 1.0f);
		}*/
	}


	public void shakeCamera(){
		StartCoroutine (Shake ());
	}

	// Update is called once per frame

	public IEnumerator Shake(){
		Vector3 origPosition = gameObject.transform.localPosition;
		float elapsedTime = 0.0f;

		Handheld.Vibrate ();

		while (elapsedTime <= shakeTime) {
			Vector3 randomPoint = origPosition + Random.insideUnitSphere * shakeAmount;
			gameObject.transform.localPosition = Vector3.Lerp (gameObject.transform.localPosition, randomPoint, Time.deltaTime * shakeSpeed);
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		gameObject.transform.localPosition = origPosition;
	}

}