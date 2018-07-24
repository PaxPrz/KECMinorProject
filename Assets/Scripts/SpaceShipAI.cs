using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipAI : MonoBehaviour {

	public GameObject mainAsteroid;
	private Vector2 forwardDirection;
	private Vector2 velocity;
	private Ray2D forwardRay;
	private RaycastHit2D hit;
	public bool isMoving;
	public GameObject backFire, leftFire, rightFire;
	bool backFireVisible, leftFireVisible, rightFireVisible;
	Rigidbody2D rb;
	public float mulfactor = 1f;
	public float rotSpeed = 90f;
	public float maxSpeed = 10f;

	//---Shooting Bullets---
	public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);

	public GameObject bulletPrefab;
	public GameObject bulletSound;
	int bulletLayer;

	public float fireDelay = 0.25f;
	float cooldownTimer = 0;
	//-----------------------

	private float rotValue;
	// Use this for initialization
	void Start () {
		forwardDirection = new Vector2 (mainAsteroid.transform.position.x-gameObject.transform.position.x, mainAsteroid.transform.position.y-gameObject.transform.position.y).normalized;
		isMoving = true;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		StartCoroutine (IgnitionFire ());
	}

	// Update is called once per frame
	void Update () {
		Quaternion rot = transform.rotation;

		float z = rot.eulerAngles.z;
		rotValue = Input.GetAxis ("Horizontal") * rotSpeed * Time.deltaTime;
		z -= rotValue;

		rot = Quaternion.Euler (0, 0, z);

		transform.rotation = rot;

		Vector3 pos = gameObject.transform.position;
		Vector3 vel = new Vector3 (0, Input.GetAxis ("Vertical") * maxSpeed * Time.deltaTime, 0);

		pos += rot * vel;

		rb.velocity = ((Vector2)pos - (Vector2)transform.position) / Time.deltaTime;
	
		forwardDirection.x = Mathf.Sin (transform.rotation.z);
		forwardDirection.y = Mathf.Cos(transform.rotation.z);

		cooldownTimer -= Time.deltaTime;

		if( Input.GetKey(KeyCode.Space) && cooldownTimer <= 0 ) {
			// SHOOT!
			cooldownTimer = fireDelay;

			Vector3 offset = transform.rotation * bulletOffset;

			GameObject b = (GameObject)Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
			GameObject s = (GameObject)Instantiate (bulletSound, transform.position, transform.rotation);
			StartCoroutine (DestroyBullet (b,s));
		}
	}

	private IEnumerator DestroyBullet(GameObject g, GameObject s){
		yield return new WaitForSeconds (2f);
		if (g != null) {
			Destroy (g);
		}
		if (s != null) {
			Destroy (s);
		}
	}

	private IEnumerator IgnitionFire(){
		float theta;
		while (true) {
			/*
			if (rb.velocity.magnitude > 0.05f) {
				theta = Mathf.Acos (Vector2.Dot (forwardDirection, rb.velocity) / (forwardDirection.magnitude * rb.velocity.magnitude));
				if (theta > -45 && theta < 45) {
					backFireVisible = true;
				} else {
					backFireVisible = false;
				}
				if (theta > -90 && theta < -30) {
					rightFireVisible = true;
				} else {
					rightFireVisible = false;
				}
				if (theta > 30 && theta < 90) {
					leftFireVisible = true;
				} else {
					leftFireVisible = false;
				}
			}
			*/
			if (rb.velocity.magnitude != 0) {
				backFireVisible = true;
			} else {
				backFireVisible = false;
			}
			if (rotValue > 0) {
				leftFireVisible = true;
			} else {
				leftFireVisible = false;
			}
			if (rotValue < 0) {
				rightFireVisible = true;
			} else {
				rightFireVisible = false;
			}

			if (backFireVisible) {
				backFire.SetActive (true);
			} else {
				backFire.SetActive (false);
			}
			if (rightFireVisible) {
				rightFire.SetActive (true);
			} else {
				rightFire.SetActive (false);
			}
			if (leftFireVisible) {
				leftFire.SetActive (true);
			} else {
				leftFire.SetActive (false);
			}
			yield return null;
		}
	}
}
