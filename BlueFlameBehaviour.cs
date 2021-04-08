using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlameBehaviour : MonoBehaviour {

	private Rigidbody2D rb;
	public GameObject Player;
	public GameObject deathPrefab;
	private Vector3 moveDirection;
	private Animator anim;
	private float magnitud, magnitudHit;
	private bool isHurting = false, isAwake = false;
	public float BFHealth, flameSpeed = 1f;
	public bool initialMove = false;

	void Awake () {

		anim = GetComponent <Animator> ();
		rb = GetComponent <Rigidbody2D> ();
		Player = FindObjectOfType<LookAt> ().GetTarget ();

		BFHealth = GetComponent <EnemyStats> ().GetHealth ();
		magnitudHit = GetComponent <EnemyStats> ().GetVision ();

		FlameLaunch ();

	}


	void Update () {

		if (!initialMove && !GetComponent <EnemyStats> ().GetFreeze ()) {

			moveDirection = Player.transform.position - transform.position;

			if (!Player.activeSelf)
				moveDirection = GetComponent <EnemyStats> ().GetInitialPosition () - transform.position;

			magnitud = Mathf.Sqrt (Mathf.Pow (moveDirection.x, 2) + Mathf.Pow (moveDirection.y, 2));

			if (magnitud < magnitudHit) {
				
				if (!isAwake) {

					FindObjectOfType<AudioManager> ().PlayAudio ("Blue Flame Awake");
					isAwake = true;

				}

				moveDirection = moveDirection / magnitud;
				rb.velocity = new Vector2 (moveDirection.x * flameSpeed, moveDirection.y * flameSpeed);
				GetComponent <EnemyStats> ().SetMoveDirection (moveDirection);

			} else 
				rb.velocity = new Vector2 (0f, 0f);
			
		} else if (GetComponent <EnemyStats> ().GetFreeze ())
			rb.velocity = new Vector2 (0f, 0f);

	}


	//Al chocar con un proyectil
	void OnTriggerEnter2D () {
		
		if (GetComponent <CircleCollider2D> ().IsTouchingLayers (LayerMask.GetMask("Proyectil", "Weapon"))) {
			
			if (!isHurting) {

				FindObjectOfType<AudioManager> ().PlayAudio ("Blue Flame Hurt");
				isHurting = true;
				anim.SetBool ("Hurt", true);
				Invoke ("Cry", 0.666f);

			}

			BFHealth = GetComponent <EnemyStats> ().GetHealth ();
			LaunchMove ();

		} else if (GetComponent <CircleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player")))
			FindObjectOfType<AudioManager> ().PlayAudio ("Claws");


		if (BFHealth <= 0) 
			BFDeath ();
					
	}

	//Muerte de la flamita
	public void BFDeath () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Blue Flame Death");
		Destroy (transform.gameObject);
		Instantiate (deathPrefab, transform.position, transform.rotation);

	}

	//Para el grito de daño
	void Cry () {

		anim.SetBool ("Hurt", false);
		isHurting = false;

	}

	//Lanzamiento
	void FlameLaunch () {

		if (initialMove) {

			moveDirection = Player.transform.position - transform.position;
			magnitud = Mathf.Sqrt (Mathf.Pow (moveDirection.x, 2) + Mathf.Pow (moveDirection.y, 2));
			moveDirection = moveDirection / magnitud;

			rb.velocity = new Vector2 (moveDirection.x * 4f, moveDirection.y * 4f);
			Invoke ("NoMove", 1.5f);

			FindObjectOfType<AudioManager> ().PlayAudio ("CacoLord Attack");

		}

	}
		
	//No hay movimiento
	public void NoMove () {
		
		rb.velocity = new Vector2 (0f, 0f);
		Invoke ("MoveAgain", 1f);
	
	}

	//Vuelve al movimiento normal
	public void MoveAgain (){
		initialMove = false;
	}


	//Activa el lanzamiento cuando tiene menos de la mitad de vida
	void LaunchMove () {

		if (!initialMove && BFHealth < 30) {

			rb.velocity = new Vector2 (0f, 0f);
			initialMove = true;
			Invoke ("FlameLaunch", 1f);

		}

	}


}
