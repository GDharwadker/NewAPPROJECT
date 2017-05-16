using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	double i = 0;
	float j = 0;
	double randomNum = 0;
	bool isCollision = false;
	bool goAttack = false;
	bool Collided = false;
	public bool isHit = false;
	public bool detect; 
	bool fromDown, fromUp, fromLeft, fromRight;
	float charX;
	float charY;
	float enemyX;
	float enemyY;
	float Xdiff;
	float Ydiff;
	float charX1;
	float charY1;
	float enemyX1;
	float enemyY1;
	float Xdiff1; 
	float Ydiff1;
	public float EnemyVelocityX = 0;
	public float EnemyVelocityY = 0;
	public float charVelocityX = 0;
	public float charVelocityY = 0;
	Vector2 positionChar;
	Vector2 positionEnemy;
	Vector2 fromPosition;
	Vector2 toPosition;
	Vector2 direction;
	public float smallY;
	public float smallX;
	private Rigidbody2D rigidB;
	private Rigidbody2D charB;
	public float speed;
	public float magnitude = 0;
	public float seconds = 0.5f;
	public int health = 100;
	public int damageInflicted = 0;
	public int force = 0;
	public int debuff = 0;
	public GUIStyle st = new GUIStyle();
	Animator anim;
	public Color color = Color.white;

	// Use this for initialization
	void Start () {
		randomNum = Mathf.Ceil (Random.value * 4);
		anim = GetComponent<Animator> ();
		rigidB = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		anim.speed = 1;
		if (!detect) {
			if (i == 60) {
				randomNum = Mathf.Ceil (Random.value * 4);
				i = 0;
			}
			//Debug.Log (randomNum);
			if (randomNum == 1) {
				rigidB.MovePosition(rigidB.position + new Vector2(0, EnemyVelocityY) * Time.deltaTime);
				//Debug.Log ("Moved left");
				if (isCollision) {
					randomNum = 3;
					isCollision = false;
					//Debug.Log ("Avoided and turned right");
				}
			}
			if (randomNum == 2) {
				rigidB.MovePosition(rigidB.position + new Vector2(0, -EnemyVelocityY) * Time.deltaTime);
				//Debug.Log ("Moved up");
				if (isCollision) {
					randomNum = 4;
					isCollision = false;
					//Debug.Log ("Avoided and turned down");
				}
			}
			if (randomNum == 3) {
				rigidB.MovePosition(rigidB.position + new Vector2(EnemyVelocityX, 0) * Time.deltaTime);
				//Debug.Log ("Moved right");
				if (isCollision) {
					randomNum = 1;
					isCollision = false;
					//Debug.Log ("Avoided and turned left");
				}
			}
			if (randomNum == 4) {
				rigidB.MovePosition(rigidB.position + new Vector2(-EnemyVelocityX, 0) * Time.deltaTime);
				//Debug.Log ("Moved down");
				if (isCollision) {
					randomNum = 2;	
					isCollision = false;
					//Debug.Log ("Avoided and turned up");
				}
			}
			anim.SetFloat("input_x", rigidB.velocity.x);
			anim.SetFloat("input_y", rigidB.velocity.y);
			anim.SetBool("isWalking", true);
			if (rigidB.velocity.x == 0 && rigidB.velocity.y == 0) {
				anim.SetBool("isWalking", false);
			}
			i++;
			//Debug.Log (i);
			//transform.rotation = Quaternion.identity;
		} 
		Detect ();
		Follow ();
		if (isHit) {
			if (fromDown) {
				rigidB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
			} 
			if (fromUp) {
				rigidB.AddForce(-Vector2.up * force, ForceMode2D.Impulse);
			} 
			if (fromLeft) {
				rigidB.AddForce(-Vector2.right * force, ForceMode2D.Impulse);
			} 
			if (fromRight) {
				rigidB.AddForce(Vector2.right * force, ForceMode2D.Impulse);
			} 
			
			StartCoroutine(WaitForce ());
		}

	}
	// Update is called once per frame
	void Update () {
		//Detect ();
		//rigidB = GetComponent<Rigidbody2D>();
		//detect = GameObject.Find("Trigger_Circle").GetComponent<Detect>().Spotting();
		//Debug.Log (detect);

	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "PathDecals" || col.gameObject.name == "TreeStumps" || col.gameObject.name == "Grass" || col.gameObject.name == "Char") {
			isCollision = true; 
			Collided = true;
			//StartCoroutine (Wait());//FIXME
		}
	}
	void Follow() {

		float xVelocity = rigidB.velocity.x;
		float yVelocity = rigidB.velocity.y;
		if (detect) {
			anim.speed = 3;
			charVelocityX = GameObject.Find ("Char").GetComponent<Rigidbody2D> ().velocity.x;
			charVelocityY = GameObject.Find("Char").GetComponent<Rigidbody2D>().velocity.y;
			charX = GameObject.Find ("Char").GetComponent<Transform> ().position.x;
			charY = GameObject.Find ("Char").GetComponent<Transform> ().position.y;
			enemyX = GetComponent<Transform> ().position.x;
			enemyY = GetComponent<Transform> ().position.y;
			Ydiff = Mathf.Floor(10*(charY - enemyY)) / 10;
			Xdiff = Mathf.Floor(10*(charX - enemyX)) / 10;

			//Debug.Log (Xdiff + "," + Ydiff);
			if (Mathf.Abs(charVelocityX) > 0) {
				if (Xdiff > 0) {
					rigidB.velocity = new Vector2 (speed, 0);
				} 
				if (Xdiff < 0) {
					rigidB.velocity = new Vector2 (-speed, 0);
				}
				if (Ydiff > 0) {
					rigidB.velocity = new Vector2 (0, speed);
				} 
				if (Ydiff < 0) {
					rigidB.velocity = new Vector2 (0, -speed);
				}
				anim.SetFloat("input_x", rigidB.velocity.x);
				anim.SetFloat("input_y", rigidB.velocity.y);
			}
			if (Mathf.Abs(charVelocityY) > 0) {
				if (Ydiff > 0) {
					rigidB.velocity = new Vector2 (0, speed);
				} 
				if (Ydiff < 0) {
					rigidB.velocity = new Vector2 (0, -speed);
				}
				if (Xdiff > 0) {
					rigidB.velocity = new Vector2 (speed, 0);
				} 
				if (Xdiff < 0) {
					rigidB.velocity = new Vector2 (-speed, 0);
				}
				anim.SetFloat("input_x", rigidB.velocity.x);
				anim.SetFloat("input_y", rigidB.velocity.y);
			}
			if(Mathf.Abs(charVelocityY) == 0 && Mathf.Abs(charVelocityX) == 0) {
				if (Ydiff > 0) {
					rigidB.velocity = new Vector2 (0, speed);
				} 
				if (Ydiff < 0) {
					rigidB.velocity = new Vector2 (0, -speed);
				}
				if (Xdiff > 0) {
					rigidB.velocity = new Vector2 (speed, 0);
				} 
				if (Xdiff < 0) {
					rigidB.velocity = new Vector2 (-speed, 0);
				}
				anim.SetFloat("input_x", rigidB.velocity.x);
				anim.SetFloat("input_y", rigidB.velocity.y);
			}
		}
	}

	void Detect() {
		charX1 = GameObject.Find ("Char").GetComponent<Transform> ().position.x;
		charY1 = GameObject.Find ("Char").GetComponent<Transform> ().position.y;
		enemyX1 = GetComponent<Transform> ().position.x;
		enemyY1 = GetComponent<Transform> ().position.y;
		Ydiff1 = (charY1 - enemyY1);
		Xdiff1 = (charX1 - enemyX1);
		direction = new Vector2(-Xdiff1, -Ydiff1);
		RaycastHit2D hit;
		positionChar = GameObject.Find ("Char").GetComponent<Transform> ().position;
		positionEnemy = GetComponent<Transform> ().position;
		fromPosition = positionEnemy;
		Debug.DrawRay (fromPosition, -direction, Color.green);
		hit = Physics2D.Raycast (fromPosition, -direction);
		if (hit.collider.gameObject.name == "Char" && (direction.magnitude <= magnitude) && !Collided) {
			//Debug.Log (direction.magnitude);
			detect = true;

		} 
		if ((direction.magnitude > magnitude)) {
			detect = false;
			rigidB.velocity = new Vector2 (0, 0);
		}
		if (Collided) {
			detect = false;
			//Debug.Log ("Collided");
			rigidB.velocity = new Vector2 (0, 0);
			if (j >= 60) {
			Collided = false;
				j = 0;
			}
			j++;
		}
		//Debug.Log (detect);

	}
	IEnumerator ColorChange() {
		GetComponent<SpriteRenderer>().color = Color.red;
		//Debug.Log ("Waiting");
		yield return new WaitForSeconds((float)seconds);
		//Debug.Log ("Finished");
		GetComponent<SpriteRenderer> ().color = color;
		yield return new WaitForSeconds((float)seconds);
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds((float)seconds);
		GetComponent<SpriteRenderer>().color = color;
	}
									         

	IEnumerator WaitForce() {
		detect = false;
		yield return new WaitForSeconds (0.001f);
		rigidB.velocity = new Vector2(0,0);
		isHit = false;
		fromUp = fromLeft = fromRight = fromDown = false;
		yield return new WaitForSeconds (1);
	}

	void HitByPlayer(string position) {
		StartCoroutine (ColorChange());
		isHit = true;
		if (health > 0) {
			health -= debuff;
		}
		if (position.Equals("fromDown" + gameObject.name)) {
			//Debug.Log ("HIT from Down"+ gameObject.name);
			fromDown = true;
			fromUp = fromLeft = fromRight = false;
		} 
		if (position.Equals("fromUp" + gameObject.name)) {
			//Debug.Log ("HIT from Up"+ gameObject.name);
			fromUp = true;
			fromDown = fromLeft = fromRight = false;
		}
		if (position.Equals("fromLeft" + gameObject.name)) {
			//Debug.Log ("HIT from Left"+ gameObject.name);
			fromLeft = true;
			fromUp = fromDown = fromRight = false;
		}
		if (position.Equals("fromRight" + gameObject.name)) {
			//Debug.Log ("HIT from Right"+ gameObject.name);
			fromRight = true;
			fromUp = fromLeft = fromDown = false;
		}
	}

	public bool isHitten() {
		return isHit;
	}


	void OnGUI() {
		st.fontSize = 50;
		st.normal.textColor = Color.white;

		if (health == 0) {
			st.fontSize = 100;
			Destroy (gameObject);
		}
	}
}
