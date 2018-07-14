using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour {
	private Material mat;
	private Camera cam;
	public float speed=0.1f;

	void Start(){
		mat = gameObject.GetComponent<MeshRenderer> ().material;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		mat.mainTextureOffset = cam.transform.position * speed;
	}
}
