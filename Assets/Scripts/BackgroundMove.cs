using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour {
	private Material mat;
	public float speed=0.05f;
	public GameObject bgMovement;

	void Start(){
		mat = gameObject.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		mat.mainTextureOffset = bgMovement.transform.position * speed;
	}
}
