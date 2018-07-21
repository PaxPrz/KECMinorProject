using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
	public GameObject sound;
	public GameObject Explosion;
	// Use this for initialization

	void OnCollisionEnter2D(Collision2D col){
		Vector2 pos = col.gameObject.transform.position;
		Destroy (col.gameObject);
		StartCoroutine (ExplosionEffect (pos));
	}

	private IEnumerator ExplosionEffect(Vector2 pos){
		GameObject s = Instantiate (sound);
		GameObject e = Instantiate (Explosion, pos, Quaternion.identity);
		yield return new WaitForSeconds (3.0f);
		if(s!=null)
		Destroy (s);
		if (e != null)
		Destroy (e);
	}

}
