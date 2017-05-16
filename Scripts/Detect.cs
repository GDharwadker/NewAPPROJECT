using UnityEngine;
using System.Collections;

public class Detect : MonoBehaviour {
	bool isSpotted = false;
	// Use this for initialization
	void Start () {
		//isSpotted = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Char") {
			Debug.Log ("Spotted!");
			isSpotted = true;
		} else {
			isSpotted = false;
		}
	}
	public bool Spotting() {
		return isSpotted;
	}

}
