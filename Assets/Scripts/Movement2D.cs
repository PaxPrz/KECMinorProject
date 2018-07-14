using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour {
	private Vector2 ray;
	private RaycastHit2D hit;
	private Camera camera;
	public float force = 50f;
	public float maxforce=15f;
	// Use this for initialization
	void Start () {
		StartCoroutine (controlMovement ());
		camera = Camera.main;
		force = GameSettings.force;
	}

	// Update is called once per frame
	void Update () {
		ray = new Vector2 (camera.ScreenToWorldPoint (Input.mousePosition).x, camera.ScreenToWorldPoint (Input.mousePosition).y);
		hit = Physics2D.Raycast(ray, Vector2.zero, 0f);
		//if ((hit.rigidbody)!=null) {
		//Debug.Log ("we hit " + hit.transform.name);
		//}
	}

	private IEnumerator controlMovement(){
		bool ok = false;
		Vector2 start=Vector2.zero, end=Vector2.zero;
		Rigidbody2D rb=null;
		float starttime=0, endtime=0;
		while (true) {
			if (Input.GetMouseButtonDown (0)) {
				if ((rb = hit.rigidbody)!=null) {
					start = normalizedPosition ();
					starttime = Time.time;
					ok = true;
				}
			}
			if (Input.GetMouseButtonUp (0) && ok) {
				end = normalizedPosition ();
				endtime = Time.time;
				float totaltime = endtime - starttime;
				Vector2 dirn = end - start;
				dirn = dirn * force / totaltime;
				dirn.Set (Mathf.Clamp (dirn.x, -maxforce, maxforce), Mathf.Clamp (dirn.y, -maxforce, maxforce));
				//Debug.Log ("Force: " + dirn.ToString ());
				if (rb.gameObject.transform.tag != "Asteroid") {
					rb.velocity = dirn;
				}
				Debug.Log ("Velocity: " + rb.velocity.ToString ());
				ok = false;
			}
			yield return null;
		}
	}

	private Vector2 normalizedPosition(){
		float posX, posY;
		posX = (float)((camera.ScreenToWorldPoint(Input.mousePosition).x / Screen.width) - 0.5) * 2;
		posY = (float)((camera.ScreenToWorldPoint(Input.mousePosition).y / Screen.height) - 0.5) * 2;

		Vector2 pos = new Vector2 (posX, posY);
		return pos;
	}

}
