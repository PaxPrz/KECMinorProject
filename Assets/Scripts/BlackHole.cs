using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {
	public float rotateSpeed = 100f;
	public GameObject gameManager;
	//GameObject c;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, Time.deltaTime * rotateSpeed);
	}

	void OnTriggerEnter2D(Collider2D col){
			StartCoroutine (DeleteObj (col.gameObject));

	}

	private IEnumerator DeleteObj(GameObject col){
		//c = col;
		yield return new WaitUntil (()=> ((col.gameObject.transform.position.x>=gameObject.transform.position.x-0.5f)&&(col.gameObject.transform.position.x<=gameObject.transform.position.x+0.5f)&&(col.gameObject.transform.position.y>=gameObject.transform.position.y-0.5f)&&(col.gameObject.transform.position.y<=gameObject.transform.position.y+0.5f)));
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
