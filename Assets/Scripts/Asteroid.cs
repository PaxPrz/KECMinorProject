using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Asteroid:MonoBehaviour{
		public static int swipeMissed=0;
		public bool touched;

		public Asteroid(){
			touched = false;
		}

		public void setTouched(){
			touched = true;
		}
	}

