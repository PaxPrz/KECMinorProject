using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

	public GameObject comet;

	public Slider stressMeter;

	private GameObject c;
	GameObject UIManager;

	//*** For blackhole ***
	public GameObject blackHole;
	public float minBlackHoleSpawnTime = 120.0f;
	private float randomBlackHoleTime;
	private GameObject currentBlackHole;
	public GameObject forBgMovement;
	private float firstDistance;
	//**********************

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
		StartCoroutine (GenerateBlackHole ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene (0);
			if(UIManager!=null)
				Destroy (UIManager);
		}

		if (c != null) {
			SpriteRenderer r = c.GetComponentInChildren<SpriteRenderer> ();
			if (r == null) {
				Debug.Log ("Error: no sprite comet");
				return;
			}
			if (r.isVisible) {
				c.GetComponent<CircleCollider2D> ().isTrigger = false;
				asteroidList.Add (c);
				c = null;
			}
		}
			
		if (currentBlackHole != null) {
			if (Vector2.Distance (mainAsteroid.transform.position, currentBlackHole.transform.position) > firstDistance+5f) {
				Destroy (currentBlackHole);
				currentBlackHole = null;
				Debug.Log ("Blackhole destroyed");
			}
		}

		stressMeter.value -= Time.deltaTime*0.02f;
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
				SizeIncreaser s=null;
				if (asteroidList [i] != null) {
					 s = asteroidList [i].GetComponent<SizeIncreaser> ();
				}
				if (s != null) {
					if (s.wasVisible && s.touched) {
						if (!asteroidList [i].GetComponent<SpriteRenderer> ().isVisible) {
							stressMeter.value += asteroidList [i].transform.localScale.x * 0.5f / mainAsteroid.transform.localScale.x;
							s.wasVisible = false;
							Debug.Log ("Stress: " + stressMeter.value+" Increased");	
						}
					}
				}
				if(stressMeter.value==stressMeter.maxValue){
					if (!nearRen.isVisible) {
						Transform t = spawnPointNear [Random.Range (0, spawnPointNear.Length)];
						c = Instantiate<GameObject> (comet, t.position, Quaternion.identity);
						c.GetComponent<CircleCollider2D> ().isTrigger = true;
						c.GetComponent<Rigidbody2D> ().velocity = ((Vector2)mainAsteroid.transform.position - (Vector2)c.transform.position) / 1f;
					}
					stressMeter.value = stressMeter.minValue;
				}
				yield return null;
			}
		}
	}

	private IEnumerator GenerateBlackHole(){
		randomBlackHoleTime = Random.Range (0.0f, 60.0f);
		Rigidbody2D forbgrigid = forBgMovement.GetComponent<Rigidbody2D> ();
		while (true) {
			yield return new WaitForSeconds(minBlackHoleSpawnTime+randomBlackHoleTime);
			if (currentBlackHole == null && forbgrigid.velocity.magnitude>1.0f) {
				currentBlackHole = Instantiate<GameObject> (blackHole, forbgrigid.velocity*5f, Quaternion.identity);
				currentBlackHole.transform.localScale = mainAsteroid.transform.localScale;
				firstDistance = Vector2.Distance (mainAsteroid.transform.position, currentBlackHole.transform.position);
				currentBlackHole.GetComponent<Rigidbody2D> ().velocity = -forbgrigid.velocity*mainAsteroid.transform.localScale.x/4f;
				Debug.Log ("BlackHole Created: "+currentBlackHole.transform.position.ToString());
				asteroidList.Add (currentBlackHole);
			}
		}
	}
}

