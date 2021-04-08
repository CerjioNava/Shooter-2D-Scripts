using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurBehaviour : MonoBehaviour {

	public GameObject Player;
	public GameObject Minotaur;
	private Vector3 moveDirection, enemyScale;
	private	Rigidbody2D rb;
	private Animator minotaurAnim;

	private float magnitud, magnitudHit = 4f, scaleX;
	public float minotaurHealth, stamina = 150f;
	private bool isAwake = false, isTired = false, isDeath = false, isAttacking = false, resting = false;
	private int hops = 0, attackNumber;

	public void Awake () {

		minotaurHealth = GetComponent <EnemyStats> ().GetHealth ();
		magnitudHit = GetComponent <EnemyStats> ().GetVision ();
		magnitud = magnitudHit + 1f;

		scaleX = Minotaur.transform.localScale.x;

		rb = Minotaur.GetComponent <Rigidbody2D> ();	
		minotaurAnim = GetComponent <Animator> ();	

		Player = FindObjectOfType<LookAt> ().GetTarget ();

		AboutToMove ();

	}
	

	void Update () {

		if (!isDeath) {

			moveDirection = Player.transform.position - GetComponentInParent <Transform> ().position;

			if (!Player.activeSelf)
				moveDirection = GetComponent <EnemyStats> ().GetInitialPosition () - GetComponentInParent <Transform> ().position;

			magnitud = Mathf.Sqrt (Mathf.Pow (moveDirection.x, 2) + Mathf.Pow (moveDirection.y, 2));

			if (!isTired && !minotaurAnim.GetBool ("Hurt")) {
		
				if (!minotaurAnim.GetBool ("AboutToRun") && !resting)
					AboutToMove ();
			
				if (magnitud < 1f && !isAttacking) {

					isAttacking = true;
					Invoke ("Attacks", 0.3f);

				}

			}

		}

	}


	//Al ser atacado con un proyectil o arma
	void OnTriggerEnter2D(Collider2D collision) {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Proyectil", "Weapon"))) {

			minotaurHealth = GetComponent <EnemyStats> ().GetHealth ();
			FindObjectOfType<AudioManager> ().PlayAudio ("Minotaur Hurt2");

			if (!isTired) {

				if (stamina > 0f && !minotaurAnim.GetBool ("Hurt")) 
					stamina -= collision.GetComponentInParent <AttackDamage> ().GetDamage ();

				else {

					FindObjectOfType<AudioManager> ().PlayAudio ("Minotaur Hurt");
					isTired = true;
					minotaurAnim.SetBool ("Hurt", true);

					stamina = 150f;
					Invoke ("NotTired", 5f);
					rb.velocity = new Vector2 (0f, 0f);
				
				}
			
				FindObjectOfType<BossHealthBar> ().SetBossStamina (stamina, 150f);

			} 

		} else if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) 
			FindObjectOfType<AudioManager> ().PlayAudio ("Claws");

		if (minotaurHealth <= 0 && !isDeath) {

			CancelInvoke ();
			isDeath = true;
			MinotaurDeath ();

		}

	}

	//MOVIMIENTO Y ATAQUES---------------------------------------------------------------------------------------------------------------------

	//Cancela movimiento
	void NoResting () {
		resting = false;
		minotaurAnim.SetBool ("Stand", false);
	}


	//Va a correr
	void AboutToMove () {
		
		minotaurAnim.SetBool ("AboutToRun", true);
		InvokeRepeating ("MinotaurMove", 1.5f, 0.667f);

	}

	//Movimiento del minotauro
	void MinotaurMove () {

		Flip ();

		if (!isTired && !isAttacking && !resting) {
			
			if (magnitud < magnitudHit) {

				if (!isAwake) {

					FindObjectOfType<AudioManager> ().PlayAudio ("Minotaur Awake");
					isAwake = true;

				}

				moveDirection = moveDirection / Mathf.Sqrt (Mathf.Pow (moveDirection.x, 2) + Mathf.Pow (moveDirection.y, 2));
				rb.velocity = new Vector2 (moveDirection.x*2, moveDirection.y*2);

				hops += 1;
				minotaurAnim.SetBool ("Run", true);

				GetComponent <EnemyStats> ().SetMoveDirection (moveDirection);

				if (hops == 5) {
					resting = true;
					minotaurAnim.SetBool ("Stand", true);
					Invoke ("NoResting", 2.3f);
				}

			} else {

				rb.velocity = new Vector2 (0f, 0f);
				hops = 0;
				minotaurAnim.SetBool ("AboutToRun", false);
				minotaurAnim.SetBool ("Run", false);
				CancelInvoke ("MinotaurMove");

			}

		} else {

			rb.velocity = new Vector2 (0f, 0f);
			hops = 0;
			minotaurAnim.SetBool ("AboutToRun", false);
			minotaurAnim.SetBool ("Run", false);
			CancelInvoke ("MinotaurMove");
		
		}



	}

	//Ataques
	void Attacks () {

		Flip ();

		rb.velocity = new Vector2 (0f, 0f);
														//Seleccion de ataque
		if (moveDirection.y > 0.85f)
			attackNumber = 1;
		else if (moveDirection.y < -0.5f)
			attackNumber = 3;
		else
			attackNumber = Random.Range (1, 3);

														//Ataque
		if (attackNumber == 1)
			minotaurAnim.SetBool ("Attack 1", true);
		else if (attackNumber == 2)
			minotaurAnim.SetBool ("Attack 2", true);
		else if (attackNumber == 3)
			minotaurAnim.SetBool ("Attack 3", true);

		FindObjectOfType<AudioManager> ().PlayAudio ("Meele Attack");

		Invoke ("StopAttacks", 0.8f);

	}

	void StopAttacks () {

		isAttacking = false;
		minotaurAnim.SetBool ("Attack 1", false);
		minotaurAnim.SetBool ("Attack 2", false);
		minotaurAnim.SetBool ("Attack 3", false);

	}


	//Muerte del minotauro
	void MinotaurDeath () {

		rb.velocity = new Vector2 (0f, 0f);
		FindObjectOfType<AudioManager> ().PlayAudio ("Minotaur Death");
		GetComponent <CapsuleCollider2D> ().enabled = false;
		minotaurAnim.SetBool ("Hurt", true);
		minotaurAnim.SetBool ("Death", true);

	}

	void Flip () {

		enemyScale = Minotaur.transform.localScale;							

		if (moveDirection.x > 0 && enemyScale.x == -scaleX) {				

			enemyScale.x = scaleX;										
			Minotaur.transform.localScale = enemyScale;						

		} else if (moveDirection.x < 0 && enemyScale.x == scaleX) {		

			enemyScale.x = -scaleX;										
			Minotaur.transform.localScale = enemyScale;		

		}

	}

	//Vuelve a la normalidad
	void NotTired () {

		if (!isDeath) {
			minotaurAnim.SetBool ("Hurt", false);
			isTired = false;
		}

	}

}
