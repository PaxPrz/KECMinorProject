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

	// Use this for initialization
	void Start () {
		forwardDirection = new Vector2 (mainAsteroid.transform.position.x-gameObject.transform.position.x, mainAsteroid.transform.position.y-gameObject.transform.position.y);
		isMoving = true;
	}

	// Update is called once per frame
	void Update () {
		//forwardRay = new Ray2D (forwardDirection.x, forwardDirection.y);
		//hit = Physics2D.Raycast(forwardRay, 
	}
}
