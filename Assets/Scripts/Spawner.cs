using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Spawner : MonoBehaviour {
	private Transform[] spawnPointNear;
	private Transform[] spawnPointFar;
	public GameObject spawnNear;
	public GameObject spawnFar;
	private float spawnRate;
	private Renderer nearRen, farRen;

	//****Keeps list of asteroids****
	public static List<GameObject> asteroidList = new List<GameObject>();
	//*******************************

	public GameObject[] canSpawn;
	public GameObject mainAsteroid;


	GameObject UIManager;

	// Use this for initialization
	void Start () {
		UIManager = GameObject.FindGameObjectWithTag ("UIManager");
		spawnRate = GameSettings.spawnRate;
		spawnPointNear = spawnNear.GetComponentsInChildren<Transform> ();
		spawnPointFar = spawnFar.GetComponentsInChildren<Transform> ();
		nearRen = spawnNear.GetComponent<Renderer> ();
		farRen = spawnFar.GetComponent<Renderer> ();
		Debug.Log ("spawnrate:" +spawnRate);
		//asteroids = new List<Asteroid> ();
		StartCoroutine (spawnFunction ());
		StartCoroutine (Destroyer ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene (0);
			if(UIManager!=null)
				Destroy (UIManager);
		}
	}

	private IEnumerator spawnFunction(){
		Rigidbody2D mainrigidbody = mainAsteroid.GetComponent<Rigidbody2D> ();
		GameObject g=null;
		while (true) {
			if (!nearRen.isVisible) {
				Transform t = spawnPointNear [Random.Range (0, spawnPointNear.Length)];
				g = Instantiate (canSpawn [Random.Range (0, canSpawn.Length - 1)], t.position, Quaternion.identity);
				Rigidbody2D rb = g.GetComponent<Rigidbody2D> ();
				float scalefactor = Random.Range (0.5f*mainAsteroid.transform.localScale.x, mainAsteroid.transform.localScale.x*0.8f);
				g.transform.localScale = new Vector2 (scalefactor, scalefactor);
				rb.mass = scalefactor * mainrigidbody.mass;
				rb.AddTorque (Random.Range (-5f*scalefactor, 5f*scalefactor));
				Vector2 direction = new Vector2 (mainAsteroid.transform.position.x - t.position.x, mainAsteroid.transform.position.y - t.position.y);
				direction = new Vector2 (Random.Range(direction.x-5,direction.x+5), Random.Range(direction.y-5,direction.y+5));
				direction.Normalize ();
				rb.velocity = new Vector2 (direction.x , direction.y);
				asteroidList.Add (g);
			} else {

			}

			//asteroids.Add(new Asteroid(g));
			yield return new WaitForSeconds (0.5f/spawnRate);
		}
	}

	/*
	private IEnumerator Monitor(){
		float cosTheta;
		while (true) {
			foreach (Asteroid a in asteroids) {
				if (a.previousPos != Vector2.zero) {
					cosTheta = Mathf.Cos (Vector2.Angle (a.previousPos - (Vector2)a.gameObj.transform.position, (Vector2)mainAsteroid.transform.position - (Vector2)a.gameObj.transform.position));
					if (cosTheta < 0 && !a.gameObj.GetComponent<Renderer> ().isVisible) {
						a.DeleteAsteroid ();
						asteroids.Remove (a);
					}
				}
			}
			yield return null;
		}
	}
	*/

	private IEnumerator Destroyer(){
		float maxDistance = spawnFar.transform.position.magnitude;
		while (true) {
			for(int i=0; i<asteroidList.Count; i++){
				if (asteroidList [i] == null) {
					asteroidList.RemoveAt (i);
					continue;
				}
				if (((Vector2)asteroidList[i].transform.position - (Vector2)mainAsteroid.transform.position).magnitude > maxDistance) {
					GameObject d = asteroidList[i];
					asteroidList.Remove (asteroidList[i]);
					if(d!=null)
						Destroy (d);
				}
				yield return null;
			}
		}
	}
}

