using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour {
	private Vector2 ray;
	private RaycastHit2D hit;
	public GameObject mycam;
	private Camera cam;
	public float force = 30f;
	public float maxforce=15f;
	public GameObject genAsteroid;
	public GameObject visibleRenderer;
	public GameObject mainAsteroid;
	// Use this for initialization
	void Start () {
		StartCoroutine (controlMovement ());
		cam = mycam.GetComponent<Camera> ();
		force = GameSettings.force;
	}

	// Update is called once per frame
	void Update () {
		ray = new Vector2 (cam.ScreenToWorldPoint (Input.mousePosition).x, cam.ScreenToWorldPoint (Input.mousePosition).y);
		hit = Physics2D.Raycast(ray, Vector2.zero, 0f);
		//if ((hit.rigidbody)!=null) {
		//Debug.Log ("we hit " + hit.transform.name);
		//}
	}

	private IEnumerator controlMovement(){
		bool ok = false;
		Vector2 start=Vector2.zero, end=Vector2.zero;
		Rigidbody2D rb=null;
		bool forGenAst = false;
		float starttime=0, endtime=0;
		while (true) {
			if (Input.GetMouseButtonDown (0)) {
				//Debug.Log ("MousePos: " + Input.mousePosition);
				if ((rb = hit.rigidbody) != null) {
					start = normalizedPosition ();
					starttime = Time.time;
					ok = true;
				} else {
					if (PlaySettings.asteroidGenSelect) {
						forGenAst = true;
						start.x = cam.ScreenToWorldPoint(Input.mousePosition).x;
						start.y = cam.ScreenToWorldPoint (Input.mousePosition).y;
						starttime = Time.time;
						ok = true;
						//Debug.Log ("Start: " + start.ToString ());
					}
				}
			}
			if (Input.GetMouseButtonUp (0) && ok) {
				if (forGenAst) {
					end.x = cam.ScreenToWorldPoint (Input.mousePosition).x;
					end.y = cam.ScreenToWorldPoint (Input.mousePosition).y;
					//Debug.Log ("End : " + end.ToString ());
					endtime = Time.time;

					Vector2 pos = new Vector2(0,0);
					int i = 1;
					while (true) {
						pos.x = start.x - i * end.x;
						pos.y = start.y - i * end.y;
						GameObject g = Instantiate<GameObject> (visibleRenderer, pos, Quaternion.identity);
						if (g.GetComponent<Renderer> () == null) {
							//Debug.Log ("No mesh renderer");
						}
						if (!g.GetComponent<Renderer> ().isVisible) {
							Destroy (g);
							break;
						}
						Destroy (g);
						i++;
					}
					//Debug.Log ("Position: "+pos.ToString());
					Vector2 forceDirn = new Vector2(end.x - start.x, end.y - start.y).normalized;
					//Debug.Log ("force: " + force.ToString ());
					GameObject spawnAsteroid = Instantiate<GameObject>(genAsteroid, pos, Quaternion.identity);
					spawnAsteroid.transform.localScale = mainAsteroid.transform.localScale * Random.Range (0.4f, 0.8f);
					spawnAsteroid.GetComponent<Rigidbody2D> ().velocity = forceDirn * 10f * cam.GetComponent<CameraMover> ().CurrentRatio;
					/*
					float slope = (end.y - start.y) / (end.x - start.x);
					int inc;
					if (slope < 1) {
						if((end.x - start.x)>0){
							//inc x by -1
							Vector2 pos = start;
							while(true){
								pos.x -= 1;
								pos.y = functionY (slope, start, pos.x);

							}
						}
						else{
							//inc x by +1
						}
					} else {
						if ((end.y - start.y) > 0) {
							//inc y by -1
						} else {
							//inc y by +1
						}
					}
					*/
					PlaySettings.asteroidGenSelect = false;
					forGenAst = false;
				} else {				
					end = normalizedPosition ();
					endtime = Time.time;
					float totaltime = endtime - starttime;
					Vector2 dirn = end - start;	
					if (dirn.magnitude > 0.01f) {
						dirn = dirn * force / totaltime;
						dirn.Set (Mathf.Clamp (dirn.x, -maxforce, maxforce), Mathf.Clamp (dirn.y, -maxforce, maxforce));
						//Debug.Log ("Force: " + dirn.ToString ());
						if (rb != null) {
							if (rb.gameObject.transform.tag != "Asteroid" && rb.gameObject.transform.tag != "Comet" && rb.gameObject.transform.tag != "BlackHole") {
								if (rb.gameObject.GetComponent<SizeIncreaser> ().touched == false) {
									rb.velocity = dirn;
									rb.gameObject.GetComponent<SizeIncreaser> ().touched = true;
								}
							}
						}
						//Debug.Log ("Velocity: " + rb.velocity.ToString ());
						ok = false;
					}
				}
			}
			yield return null;
		}
	}

	private Vector2 normalizedPosition(){
		float posX, posY;
		posX = (float)((cam.ScreenToWorldPoint(Input.mousePosition).x / Screen.width) - 0.5) * 2;
		posY = (float)((cam.ScreenToWorldPoint(Input.mousePosition).y / Screen.height) - 0.5) * 2;

		Vector2 pos = new Vector2 (posX, posY);
		return pos;
	}

	private float functionY(float slope, Vector2 startpos, float x){
		return ((slope * x) - (slope * startpos.x) + startpos.y);
	}

	private float functionX(float slope, Vector2 startpos, float y){
		return ((y + slope * startpos.x - startpos.y) / slope);
	}

}
