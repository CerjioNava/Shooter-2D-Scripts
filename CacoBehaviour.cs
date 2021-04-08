using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacoBehaviour : MonoBehaviour {

	public Transform firePoint;
	public GameObject ballPrefab, deathPrefab;
	private GameObject Player;
	private Vector3 moveDirection;
	private Animator cacoAnim;
	private Rigidbody2D rb;
	private float magnitud, magnitudHit = 4f;
	public float cacoHealth, attackTime;
	private bool isAwake = false;


	public void Awake () {

		cacoAnim = GetComponent <Animator> ();

		CacoIdle ();

		cacoHealth = GetComponent <EnemyStats> ().GetHealth ();
		magnitudHit = GetComponent <EnemyStats> ().GetVision ();

		attackTime = 10f;

		if (GetComponent <EnemyStats> ().GetBoss ())
			attackTime = 20f;

		Player = FindObjectOfType<LookAt> ().GetTarget ();
		rb = GetComponent <Rigidbody2D> ();
	
	}

	public void Start () {
		InvokeRepeating ("CacoMove", 1f, Random.Range (5f, 10f)/5f);
	}

	void Update() { }

	//---------------------------------------------------------------------------------------------------------------------

	//Al chocar con un proyectil
	void OnTriggerEnter2D () {

		if (GetComponent <CircleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Proyectil", "Weapon"))) {
		
			cacoHealth = GetComponent <EnemyStats> ().GetHealth ();
			FindObjectOfType<AudioManager> ().PlayAudio ("Caco Hurt");

		} else if (GetComponent <CircleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) 
			FindObjectOfType<AudioManager> ().PlayAudio ("Claws");
		
		if (cacoHealth <= 0) 
			CacoDeath ();
		
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Movimiento del Cacodemon
	void CacoMove () {

		moveDirection = Player.transform.position - transform.position;

		if (!Player.activeSelf)
			moveDirection = GetComponent <EnemyStats> ().GetInitialPosition () - transform.position;

		magnitud = Mathf.Sqrt (Mathf.Pow (moveDirection.x, 2) + Mathf.Pow (moveDirection.y, 2));

		if (magnitud < magnitudHit) {

			if (!isAwake) {

				FindObjectOfType<AudioManager> ().PlayAudio ("Caco Awake");
				isAwake = true;

			}

			moveDirection = moveDirection / Mathf.Sqrt (Mathf.Pow (moveDirection.x, 2) + Mathf.Pow (moveDirection.y, 2));
			rb.velocity = new Vector2 (moveDirection.x * Random.Range (-2f, 10f) / 10, moveDirection.y * Random.Range (-2f, 10f) / 10);
			GetComponent <EnemyStats> ().SetMoveDirection (moveDirection);

		} else {

			rb.velocity = new Vector2 (Random.Range (-1f, 1f) / 10, Random.Range (-1f, 1f) / 10);
			magnitudHit = 3f; 		

		}
			
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Cacodemon sin hacer nada
	void CacoIdle () {

		cacoAnim.SetBool ("CacoAttack", false);
		Invoke ("CacoAttack", Random.Range (attackTime - 10f, attackTime) / 5); 

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Cacodemon ataca
	void CacoAttack () {

		if (magnitud < magnitudHit && !GetComponent <EnemyStats> ().GetFreeze ()) {

			cacoAnim.SetBool ("CacoAttack", true);

			if (isAwake)
				Invoke ("BallAttack", 0.4f); 

		}

		Invoke ("CacoIdle", 0.9f); 
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Instancia la bola de ataque
	void BallAttack () {

		if (GetComponent <EnemyStats> ().GetBoss ()) {

			ballPrefab.GetComponent <BlueFlameBehaviour> ().initialMove = true;
			Instantiate (ballPrefab, firePoint.position, firePoint.rotation);
			ballPrefab.GetComponent <BlueFlameBehaviour> ().initialMove = false;

		} else {
			Instantiate (ballPrefab, firePoint.position, firePoint.rotation);
		}

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Muerte del cacodemon
	public void CacoDeath () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Caco Death");
		Destroy (transform.gameObject);
		Instantiate (deathPrefab, transform.position, transform.rotation);

	}

	//---------------------------------------------------------------------------------------------------------------------


}
