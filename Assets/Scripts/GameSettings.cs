using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class GameSettings:MonoBehaviour{
	public static float force = 30f;
		public static bool isSound=true;
		public static bool isVibrate = true;
		public static float spawnRate = 0.5f;
	public Slider spawner;
	private AudioSource source;

	void Start(){
		spawner.value = spawnRate;
	}

		public void PlaySound(bool sound){
			isSound = sound;
			
		if ((source = gameObject.GetComponent<AudioSource>()) != null) {
				source.enabled = isSound;
			}
		}

	public void PlayVibrate(bool vibrate){
			isVibrate = vibrate;
		}

	public void SetSpawnRate(float rate){
			spawnRate = rate;
		Debug.Log ("Spawn rate: " + rate * 100f + "%");
		}

	public void SetForceRate(float rate){
		force = rate;
	}

	}


