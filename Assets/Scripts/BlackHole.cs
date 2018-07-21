using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {
	public float rotateSpeed = -1000f;
	private GameObject gameManager;
	private float mul;
	private GameObject cam;
	//GameObject c;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");	
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, Time.deltaTime * rotateSpeed);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Asteroid") {
			gameManager.GetComponent<PlaySettings> ().GameOver ();
		} else {
			StartCoroutine (DeleteObj (col.gameObject));
		}
	}

	private IEnumerator DeleteObj(GameObject col){
		//c = col;
		mul = cam.GetComponent<CameraMover>().CurrentRatio;
		yield return new WaitUntil (()=> ((col.gameObject.transform.position.x>=gameObject.transform.position.x-0.5f*mul)&&(col.gameObject.transform.position.x<=gameObject.transform.position.x+0.5f*mul)&&(col.gameObject.transform.position.y>=gameObject.transform.position.y-0.5f*mul)&&(col.gameObject.transform.position.y<=gameObject.transform.position.y+0.5f*mul)));
		if (col != null) {
			if (col.transform.tag == "Asteroid") {
				gameManager.GetComponent<PlaySettings> ().GameOver ();
			} else {
				Destroy (col);
			}
		}
		yield return null;
	}

}
