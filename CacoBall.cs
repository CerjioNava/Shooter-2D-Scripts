using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacoBall : MonoBehaviour {

	private Rigidbody2D rb;
	public GameObject impactPrefab;
	private GameObject Player;
	private Vector3 ballDirection;
	private Vector3 predictDir;
	private float angle;

	void Awake () {

		rb = GetComponent <Rigidbody2D> ();

		Player = FindObjectOfType<LookAt> ().GetTarget ();

		ballDirection = Player.transform.position - transform.position;

		ballDirection = ballDirection / Mathf.Sqrt (Mathf.Pow (ballDirection.x, 2) + Mathf.Pow (ballDirection.y, 2));

		angle = Mathf.Atan2 (ballDirection.y, ballDirection.x) * Mathf.Rad2Deg ;	

		BallMovement ();

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Al chocar con algo
	void OnTriggerEnter2D () {

		if (!GetComponent <CircleCollider2D> ().IsTouchingLayers (LayerMask.GetMask("Enemy", "ProyectilEnemigo", "Proyectil", "Effects"))) {

			Invoke ("DestroyBall", Time.deltaTime);

		}

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Movimiento de la pelotica
	void BallMovement () {

		rb.rotation = angle;	
		rb.velocity = new Vector2 (ballDirection.x * 3f, ballDirection.y * 3f) ;
		Invoke ("DestroyBall", 1.5f); 

	}
		
	//---------------------------------------------------------------------------------------------------------------------

	//Destruye el objeto para que no sea eternal :v
	void DestroyBall () {

		Destroy (transform.gameObject);
		Instantiate (impactPrefab, transform.position, transform.rotation);

	}

	//---------------------------------------------------------------------------------------------------------------------



}
