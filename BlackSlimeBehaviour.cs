using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSlimeBehaviour : MonoBehaviour {

	public GameObject Player;
	private Vector3 enemyScale, initialPosition, distance;
	private	Rigidbody2D rb;
	private Animator slimeAnim;

	private float magnitud, magnitudHit, range;
	public float slimeHealth, moveDirection = 1f, rangeDistance, scaleX;
	private bool isDeath = false, isAttacking = false;
	private int attackNumber;


	void Awake () {

		initialPosition = transform.position;
		scaleX = transform.localScale.x;

		slimeHealth = GetComponent <EnemyStats> ().GetHealth ();
		magnitudHit = GetComponent <EnemyStats> ().GetVision ();

		rb = GetComponent <Rigidbody2D> ();	
		slimeAnim = GetComponent <Animator> ();	

		Player = FindObjectOfType<LookAt> ().GetTarget ();

	}

	void Update () {
	
		if (Player == null)
			Player = FindObjectOfType<LookAt> ().GetTarget ();

		distance = Player.transform.position - transform.position - new Vector3 (0f, -0.5f, 0f);
		magnitud = Mathf.Sqrt (Mathf.Pow (distance.x, 2) + Mathf.Pow (distance.y, 2));

	}

	void FixedUpdate () {

		if (!isDeath && !slimeAnim.GetBool ("Hurt")) {

			if (!isAttacking) {
			
				SlimeMove ();

				if (magnitud < magnitudHit) {

					isAttacking = true;
					rb.velocity = new Vector2 (distance.x, distance.y);
					Invoke ("Attacks", 0.25f);

				}

			}

		}

	}

	//Al ser atacado con un proyectil o arma
	void OnTriggerEnter2D(Collider2D collision) {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Proyectil", "Weapon"))) {

			slimeHealth = GetComponent <EnemyStats> ().GetHealth ();

			slimeAnim.SetBool ("Hurt", true);
			rb.velocity = new Vector2 (0f, 0f);

			FindObjectOfType<AudioManager> ().PlayAudio ("Caco Hurt");
			Invoke ("StopHurt", 0.25f);

		} else if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) 
			FindObjectOfType<AudioManager> ().PlayAudio ("Claws");

		if (slimeHealth <= 0 && !isDeath) {

			CancelInvoke ();
			isDeath = true;
			Invoke ("SlimeDeath", Time.deltaTime); 

		}

	}

	//MOVIMIENTO Y ATAQUES---------------------------------------------------------------------------------------------------------------------

	void SlimeMove () {

		range = transform.position.x - initialPosition.x;

		if (range <= -rangeDistance && moveDirection == -1f) {
			moveDirection = 1f;
			Flip ();
		} else if (range >= rangeDistance && moveDirection == 1f) {
			moveDirection = -1f;
			Flip ();
		}

		if (transform.position.y != initialPosition.y) {
			rb.velocity = new Vector2 (moveDirection*2f, (initialPosition.y-transform.position.y)*1f);
		} else
			rb.velocity = new Vector2 (moveDirection*2f, 0f);

	}

	void Flip () {

		enemyScale = transform.localScale;							

		if (moveDirection > 0 && enemyScale.x == scaleX) {				

			enemyScale.x = -scaleX;										
			transform.localScale = enemyScale;						

		} else if (moveDirection < 0 && enemyScale.x == -scaleX) {		

			enemyScale.x = scaleX;										
			transform.localScale = enemyScale;		

		}

	}

	//Ataques
	void Attacks () {

		if (moveDirection * distance.x < 0) {

			moveDirection *= -1;
			Flip ();

		}

		//Seleccion de ataque
		if (Mathf.Abs (distance.x) > magnitudHit)
			attackNumber = 2;
		else if (Mathf.Abs (distance.x) < 0.25f)
			attackNumber = 3;
		else
			attackNumber = 1;

		//Ataque
		if (attackNumber == 1) {
			
			slimeAnim.SetBool ("Attack 1", true);
			Invoke ("StopAttacks", 0.833f);

		} else if (attackNumber == 2) {

			slimeAnim.SetBool ("Attack 2", true);
			Invoke ("StopAttacks", 0.917f);
			rb.velocity = new Vector2 (moveDirection * 3f, 0f);

		} else if (attackNumber == 3) {

			slimeAnim.SetBool ("Attack 3", true);
			Invoke ("StopAttacks", 0.667f);

		}


	}

	void StopAttacks () {

		rb.velocity = new Vector2 (0f, 0f);
		isAttacking = false;
		slimeAnim.SetBool ("Attack 1", false);
		slimeAnim.SetBool ("Attack 2", false);
		slimeAnim.SetBool ("Attack 3", false);

	}


	void SlimeDeath () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Slime Death");
		rb.velocity = new Vector2 (0f, 0f);
		GetComponent <CapsuleCollider2D> ().enabled = false;
		slimeAnim.SetBool ("Death", true);
		Destroy (gameObject, 0.42f);

	}

	//Vuelve a la normalidad
	void StopHurt () {

		if (!isDeath) {
			slimeAnim.SetBool ("Hurt", false);
		}

	}

}
