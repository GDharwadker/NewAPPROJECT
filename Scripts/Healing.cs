using UnityEngine;
using System.Collections;

public class Healing : MonoBehaviour {
	public bool isActive = false;
	Vector2 enemyPosition;
	bool isHitten; //stupid name cuz names were taken
	bool isDetached = false;
	public float randNum = 0f;
	public float randomRange = 0f;
	public float randomTarget = 0f;
	public GameObject parentObject;
	public int i = 0;
	// Use this for initialization
	void Start () {
		isActive = false;
		i = 0;
		//GameObject.Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (parentObject == null) {
			return;
		}
			enemyPosition = parentObject.GetComponent<Transform> ().position;
			isHitten = parentObject.GetComponent<AI> ().isHitten ();
			if (isHitten) {
				randNum = Mathf.Ceil (Random.value * randomRange);
				Debug.Log ("Well Sheeeeet....." + randNum);
				if ((randNum == randomTarget) && (parentObject != null) && (GameObject.Find("Char").GetComponent<Movement>().health != 0)) {
					GameObject.Instantiate (gameObject, enemyPosition, Quaternion.identity);
					Debug.Log ("Done");
					i++;
				}
				
			}
	}
	
}
