using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Spawner : MonoBehaviour {
	private Transform[] spawnPointNear;
	private Transform[] spawnPointFar;
	private Transform[] spawnPointVeryFar;
	public GameObject spawnNear;
	public GameObject spawnFar;
	public GameObject spawnVeryFar;
	private float spawnRate;
	private Renderer nearRen, farRen, veryFarRen;

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
	public GameObject cam;
	public GameObject danger;
	private bool callDanger=false;

	public float superNovaGenTime = 60f;
	public GameObject superNova;
	public float throwVelocity = 10f;
	//**********************

	// Use this for initialization
	void Start () {
		UIManager = GameObject.FindGameObjectWithTag ("UIManager");
		spawnRate = GameSettings.spawnRate;
		spawnPointNear = spawnNear.GetComponentsInChildren<Transform> ();
		spawnPointFar = spawnFar.GetComponentsInChildren<Transform> ();
		spawnPointVeryFar = spawnFar.GetComponentsInChildren<Transform> ();
		nearRen = spawnNear.GetComponent<Renderer> ();
		farRen = spawnFar.GetComponent<Renderer> ();
		veryFarRen = spawnVeryFar.GetComponent<Renderer> ();
		//asteroids = new List<Asteroid> ();
		StartCoroutine (spawnFunction ());
		StartCoroutine (Destroyer ());
		StartCoroutine (GenerateBlackHole ());
		StartCoroutine (ExplodeSuperNova ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("UI");
			if (PlaySettings.paused) {
				Time.timeScale = 1.0f;
			}
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
				Debug.Log ("Comet visible");
				asteroidList.Add (c);
				c = null;
			}
		}
			
		if (currentBlackHole != null) {
			if (callDanger) {
				callDanger = false;
				StartCoroutine (DangerSign ());
			}
			if (Vector2.Distance (mainAsteroid.transform.position, currentBlackHole.transform.position) > firstDistance) {
				Destroy (currentBlackHole);
				currentBlackHole = null;
				Debug.Log ("Blackhole destroyed");
			}
		}

		stressMeter.value -= Time.deltaTime*0.02f;
	}

	private IEnumerator DangerSign(){
		GameObject d = Instantiate (danger);
		int ratio = cam.GetComponent<CameraMover> ().CurrentRatio;
		d.transform.localScale *= ratio;
		while (true) {
			if (currentBlackHole == null) {
				Destroy (d);
				break;
			}
			Vector2 pos = currentBlackHole.transform.position - mainAsteroid.transform.position;
			pos.Normalize ();
			d.transform.position = (pos * 5f) * ratio;
			yield return new WaitForSeconds(0.5f);
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
				float scalefactor = Random.Range (0.5f * mainAsteroid.transform.localScale.x, mainAsteroid.transform.localScale.x * 0.8f);
				g.transform.localScale = new Vector2 (scalefactor, scalefactor);
				rb.mass = scalefactor * mainrigidbody.mass;
				rb.AddTorque (Random.Range (-5f * scalefactor, 5f * scalefactor));
				Vector2 direction = new Vector2 (mainAsteroid.transform.position.x - t.position.x, mainAsteroid.transform.position.y - t.position.y);
				direction = new Vector2 (Random.Range (direction.x - 5, direction.x + 5), Random.Range (direction.y - 5, direction.y + 5));
				direction.Normalize ();
				rb.velocity = new Vector2 (direction.x, direction.y);
				asteroidList.Add (g);
			} else if (!farRen.isVisible) {
				Transform t = spawnPointFar [Random.Range (0, spawnPointFar.Length)];
				g = Instantiate (canSpawn [Random.Range (0, canSpawn.Length - 1)], t.position, Quaternion.identity);
				Rigidbody2D rb = g.GetComponent<Rigidbody2D> ();
				float scalefactor = Random.Range (0.5f * mainAsteroid.transform.localScale.x, mainAsteroid.transform.localScale.x * 0.8f);
				g.transform.localScale = new Vector2 (scalefactor, scalefactor);
				rb.mass = scalefactor * mainrigidbody.mass;
				rb.AddTorque (Random.Range (-5f * scalefactor, 5f * scalefactor));
				Vector2 direction = new Vector2 (mainAsteroid.transform.position.x - t.position.x, mainAsteroid.transform.position.y - t.position.y);
				direction = new Vector2 (Random.Range (direction.x - 3, direction.x + 3), Random.Range (direction.y - 3, direction.y + 3));
				direction.Normalize ();
				rb.velocity = new Vector2 (direction.x * 2f, direction.y * 2f);
				asteroidList.Add (g);
			} else {
				Transform t = spawnPointVeryFar [Random.Range (0, spawnPointVeryFar.Length)];
				g = Instantiate (canSpawn [Random.Range (0, canSpawn.Length - 1)], t.position, Quaternion.identity);
				Rigidbody2D rb = g.GetComponent<Rigidbody2D> ();
				float scalefactor = Random.Range (0.5f * mainAsteroid.transform.localScale.x, mainAsteroid.transform.localScale.x * 0.8f);
				g.transform.localScale = new Vector2 (scalefactor, scalefactor);
				rb.mass = scalefactor * mainrigidbody.mass;
				rb.AddTorque (Random.Range (-5f * scalefactor, 5f * scalefactor));
				Vector2 direction = new Vector2 (mainAsteroid.transform.position.x - t.position.x, mainAsteroid.transform.position.y - t.position.y);
				direction = new Vector2 (Random.Range (direction.x - 2, direction.x + 2), Random.Range (direction.y - 2, direction.y + 2));
				direction.Normalize ();
				rb.velocity = new Vector2 (direction.x * 4f, direction.y * 4f);
				asteroidList.Add (g);
			}

			//asteroids.Add(new Asteroid(g));
			yield return new WaitForSeconds (0.5f/spawnRate);
		}
	}


	private IEnumerator Destroyer(){
		float maxDistance = spawnVeryFar.transform.position.magnitude;
		while (true) {
			for(int i=0; i<asteroidList.Count; i++){
				SizeIncreaser s=null;
				if (asteroidList [i] != null) {
					if (asteroidList [i].tag != "BlackHole" && asteroidList[i].tag!="Comet") {
						s = asteroidList [i].GetComponent<SizeIncreaser> ();
					}
				}
				else {
					asteroidList.RemoveAt (i);
					continue;
				}
				if (((Vector2)asteroidList[i].transform.position - (Vector2)mainAsteroid.transform.position).magnitude > maxDistance) {
					GameObject d = asteroidList[i];
					asteroidList.Remove (asteroidList[i]);
					if(d!=null)
						Destroy (d);
					continue;
				}

				if (s != null) {
					if (s.wasVisible && s.touched) {
						if (!asteroidList [i].GetComponent<SpriteRenderer> ().isVisible) {
							stressMeter.value += asteroidList [i].transform.localScale.x * 0.5f / mainAsteroid.transform.localScale.x;
							s.wasVisible = false;
							//Debug.Log ("Stress: " + stressMeter.value+" Increased");	
						}
					}
				}
				if(stressMeter.value==stressMeter.maxValue){
					if (!nearRen.isVisible) {
						Transform t = spawnPointNear [Random.Range (0, spawnPointNear.Length)];
						c = Instantiate<GameObject> (comet, t.position, Quaternion.identity);
						c.GetComponent<CircleCollider2D> ().isTrigger = true;
						c.GetComponent<Rigidbody2D> ().velocity = ((Vector2)mainAsteroid.transform.position - (Vector2)c.transform.position) / 1f;
						Debug.Log ("Comet: " + c.GetComponent<Rigidbody2D> ().velocity.ToString ());
					} else if (!farRen.isVisible) {
						Transform t = spawnPointFar [Random.Range (0, spawnPointFar.Length)];
						c = Instantiate<GameObject> (comet, t.position, Quaternion.identity);
						c.GetComponent<CircleCollider2D> ().isTrigger = true;
						c.GetComponent<Rigidbody2D> ().velocity = ((Vector2)mainAsteroid.transform.position - (Vector2)c.transform.position) / 1f;
					}else {
						Transform t = spawnPointVeryFar [Random.Range (0, spawnPointVeryFar.Length)];
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
				currentBlackHole = Instantiate<GameObject> (blackHole, forbgrigid.velocity*3f*cam.GetComponent<CameraMover>().CurrentRatio, Quaternion.identity);
				currentBlackHole.transform.localScale *= cam.GetComponent<CameraMover>().CurrentRatio;
				firstDistance = Vector2.Distance (mainAsteroid.transform.position, currentBlackHole.transform.position);
				currentBlackHole.GetComponent<Rigidbody2D> ().velocity = -forbgrigid.velocity*mainAsteroid.transform.localScale.x/4f;
				Debug.Log ("BlackHole Created: "+currentBlackHole.transform.position.ToString());
				asteroidList.Add (currentBlackHole);
				callDanger = true;
			}
		}
	}

	private IEnumerator ExplodeSuperNova(){
		GameObject currentSuperNova;
		while (true) {
			yield return new WaitForSeconds (superNovaGenTime + Random.Range (0.0f, 60.0f));
			Vector2 randompos = new Vector2 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));
			float curRatio = cam.GetComponent<CameraMover> ().CurrentRatio;
			randompos.Normalize ();
			currentSuperNova = Instantiate (superNova, randompos*5f*curRatio, Quaternion.identity);
			currentSuperNova.transform.localScale = Vector2.zero;
			randompos = Vector2.zero;
			while (currentSuperNova.transform.localScale.x < curRatio) {
				randompos.x += 0.01f * curRatio;
				randompos.y += 0.01f * curRatio;
				currentSuperNova.transform.localScale = randompos;
				yield return new WaitForSeconds (0.01f);
			}
			int num = Random.Range (30, 50);
			yield return new WaitForSeconds (3.0f);
			for (int i = 0; i < num; i++) {
				GameObject g = Instantiate (canSpawn [Random.Range (0, canSpawn.Length)], currentSuperNova.transform.position, Quaternion.identity);
				g.transform.localScale = mainAsteroid.transform.localScale * Random.Range (0.3f, 1.0f);
				Vector2 pos = new Vector2 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));
				pos.Normalize ();
				g.GetComponent<Rigidbody2D> ().velocity = pos * throwVelocity*curRatio;
				asteroidList.Add (g);
				yield return new WaitForSeconds (0.1f);
			}
			while (currentSuperNova.transform.localScale.x > 0) {
				randompos.x -= 0.05f * curRatio;
				randompos.y -= 0.05f * curRatio;
				currentSuperNova.transform.localScale = randompos;
				yield return new WaitForSeconds (0.01f);
			}
			Destroy (currentSuperNova);
		}
	}
}

