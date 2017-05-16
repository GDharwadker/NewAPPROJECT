using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public float speed = 10f;
	private bool up, left, right, down;
	private bool up1, left1, right1, down1;
	private int delay = 0;
	public Texture[] textures;
	public int currentTexture = 0;
	float charX2;
	float charY2;
	float enemyX2;
	float enemyY2;
	float Xdiff2;
	float Ydiff2;
	private bool isAttacked = false;
	private bool isAttacking = false;
	private bool isShielded = false;
	public int health = 0;
	public GUIStyle st = new GUIStyle();
	private bool collided = false;
	private Rigidbody2D rBody;
	public Vector2 y = new Vector2(0,-1);
	public Vector2 x = new Vector2(-1,0);
	public Vector2  directiony;
	public Vector2  directionx;
	public int dragNum = 100;
	public double seconds = 0.1f;
	public int debuff = 10;
	float i = 0;
	float j = 0;
	int layerMask = 1 << 8;
	float velocityX;
	float velocityY;
	public float range = 0;
	Animator anim;
	public float speed2 = 0;
	public float sprintSpeed;
	public Color color;


	void Start() {
		health = 100;
		rBody= GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		speed2 = speed;
	}

	void Update () {
	
		if (Input.GetKey (KeyCode.RightShift)) {
			speed = sprintSpeed;
			anim.speed = 2;
		} else {
			speed = speed2;
			anim.speed = 1;
		}

		CheckUpdate ();
		if (Input.GetKey (KeyCode.O)) {
			if (i <= 60) {
				isShielded = true;
				rBody.velocity = new Vector2 (0, 0);
				GetComponent<SpriteRenderer> ().color = Color.gray;
			}
			if (i > 60) {
				isShielded = false;
				GetComponent<SpriteRenderer> ().color = Color.white;
			}
			i++;
			//Debug.Log (i);
		} else if (isShielded && !Input.GetKey (KeyCode.O)) {
			isShielded = false;
			GetComponent<SpriteRenderer> ().color = Color.white;
		} else {
			i = 0;
		}
		//Debug.Log (isShielded);

		if (Input.GetKeyDown (KeyCode.P)) {
			rBody.velocity = new Vector2 (0, 0);
			anim.SetBool("isAttacking", true);
			directiony = new Vector2 (0, j);
			directionx = new Vector2 (j, 0);

			if (up) {
				RaycastHit2D hit = Physics2D.Raycast (GetComponent<Transform> ().position, Vector2.up);
				Debug.DrawRay (GetComponent<Transform> ().position, Vector2.up, Color.green, 0.1f);
				isAttacking = true;
					if (hit.collider.tag == "Enemy" && hit.distance < range){
					hit.collider.SendMessage("HitByPlayer","fromDown" + hit.collider.name);
					}
			}
			if (down) {
				RaycastHit2D hit = Physics2D.Raycast (GetComponent<Transform> ().position, -Vector2.up);
				Debug.DrawRay (GetComponent<Transform> ().position, -Vector2.up, Color.green, 0.1f);
				isAttacking = true;

				if (hit.collider.tag == "Enemy" && hit.distance < range){
					hit.collider.SendMessage("HitByPlayer","fromUp" + hit.collider.name);
				}
				
			}
			if (left) {
				RaycastHit2D hit = Physics2D.Raycast (GetComponent<Transform> ().position, Vector2.right);
				Debug.DrawRay (GetComponent<Transform> ().position, Vector2.right, Color.green, 0.1f);
				isAttacking = true;
				if (hit.collider.tag == "Enemy" && hit.distance < range){
					hit.collider.SendMessage("HitByPlayer","fromRight" + hit.collider.name);
				}
				
			}
			if (right) {
				RaycastHit2D hit = Physics2D.Raycast (GetComponent<Transform> ().position, -Vector2.right);
				Debug.DrawRay (GetComponent<Transform> ().position, -Vector2.right, Color.green, 0.1f);
				isAttacking = true;

				if (hit.collider.tag == "Enemy" && hit.distance < range){
					hit.collider.SendMessage("HitByPlayer","fromLeft" + hit.collider.name);
				}
				
			}
		} else {
			isAttacking = false;
			anim.SetBool("isAttacking", false);
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 movement_vector = new Vector2 (-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if ((movement_vector != Vector2.zero) && !isAttacking) {       
			anim.SetFloat ("input_x", movement_vector.x);
			anim.SetFloat ("input_y", movement_vector.y);        //!isShielded && !isAttacking
			anim.SetBool ("isWalking", true);
		} else {
			anim.SetBool("isWalking", false);
		}
			rBody.MovePosition (rBody.position + movement_vector.normalized * Time.deltaTime * speed);
			Debug.Log (rBody.velocity.magnitude);

	}

	void CheckUpdate() {

		if(Input.GetKey(KeyCode.W)){
			up = true;
			left = right = down = false;
		}
		if(Input.GetKey(KeyCode.S)){
			down = true;
			up = left = right = false;
		}
		if(Input.GetKey(KeyCode.A)){
			left = true;
			up = right = down = false;
		}
		if(Input.GetKey(KeyCode.D)){
			right = true;
			up = left = down = false;
		}


	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Enemy") {
			collided = true;
			if (isShielded == false) {
				StartCoroutine(ColorChange());
			}
			debuff = col.gameObject.GetComponent<AI>().damageInflicted;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Healing" && (col.gameObject.name != "Healing_Object")) {
			StartCoroutine(ColorChangeGreen());
			Destroy (col.gameObject);
		}
	}

	IEnumerator ColorChange() {
		if (health > 0) {
			health -= debuff;
		}
		GetComponent<SpriteRenderer>().color = Color.red;
		//Debug.Log ("Waiting");
		yield return new WaitForSeconds((float)seconds);
		//Debug.Log ("Finished");
		GetComponent<SpriteRenderer>().color = Color.white;
		yield return new WaitForSeconds((float)seconds);
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds((float)seconds);
		GetComponent<SpriteRenderer>().color = Color.white;
	}

	IEnumerator ColorChangeGreen() {
		if (health > 0) {
			health += debuff;
		}
		GetComponent<SpriteRenderer>().color = Color.green;
		//Debug.Log ("Waiting");
		yield return new WaitForSeconds((float)seconds);
		//Debug.Log ("Finished");
		GetComponent<SpriteRenderer>().color = Color.white;
		yield return new WaitForSeconds((float)seconds);
		GetComponent<SpriteRenderer>().color = Color.green;
		yield return new WaitForSeconds((float)seconds);
		GetComponent<SpriteRenderer>().color = Color.white;
	}
	

	void OnGUI() {
		st.fontSize = 50;
		st.normal.textColor = Color.white;
		if (health > 0) {
		GUI.Label(new Rect(0,0,100,100), "Life: " + health.ToString(), st);
		}

		if (health == 0) {
			st.fontSize = 100;
			Time.timeScale = 0;
			GUI.Label(new Rect(0,200,100,100), "GAME OVER: Press Enter to Restart", st);
			if (Input.GetKey(KeyCode.KeypadEnter)) {
				Time.timeScale = 1;
				Application.LoadLevel("Scene_02");
			}
		}

	}
}
