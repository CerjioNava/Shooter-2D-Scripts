using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Rigidbody2D rb;												//Rigidbody de la bala
	public GameObject impactPrefab;										//Efecto de impacto 
	private Camera cam;													//Camera para el apuntado
	private Vector2 bulletDirection;									//Direcicón de disparo
	private float angulo, magnitud;										//Angulo y mágnitud para el vector de dirección
	public float destroyTime, bulletSpeed = 5f;							//Tiempo de destrucción de la bala luego de aparecer y velocidad
				
	//Método llamado al habilitarse la bala
	void Awake () {

		rb = GetComponent <Rigidbody2D> ();			//Asignacion del rigidbody

		/*
		cam = GameObject.Find ("Main Camera").GetComponent <Camera> ();		//Asignación de camara al iniciar

		bulletDirection = cam.ScreenToWorldPoint (Input.mousePosition);		//Dirección del mouse
		bulletDirection = bulletDirection - transform.position;				//Dirección de disparo respecto a su posición de creación

		*/

		bulletDirection = FindObjectOfType<PlayerControl> ().AttackDirection ();

		if (bulletDirection == new Vector2 (0f, 0f))
			bulletDirection = new Vector2 (FindObjectOfType<PlayerControl> ().transform.localScale.x, 0f);

		magnitud = Mathf.Sqrt (Mathf.Pow (bulletDirection.x, 2) + Mathf.Pow (bulletDirection.y, 2));	//Magnitud del vector dirección
		bulletDirection = bulletDirection / magnitud;						//Vector unitario de la dirección (para velocidad)

		angulo = Mathf.Atan2 (bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg ;		//Angulo de la dirección

		BulletMovement ();			//Llamado al movimiento de la bala

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Al chocar con algo que no sea layer Player, Effects o Proyectil
	void OnTriggerEnter2D () {

		if ( !GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask("Player", "Effects", "Proyectil", "ProyectilEnemigo", "Objects")) ) {

			Invoke ("DestroyBullet", Time.deltaTime);		//Invoca al destruir la bala un frame después

		}

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Enemy")) )
			FindObjectOfType<SpecialFill> ().FillSpecial1 (0.01f);
		
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Movimiento de la bala
	void BulletMovement () {

		rb.rotation = angulo;															//Rotación de la bala
		rb.velocity = new Vector2 (bulletDirection.x * bulletSpeed, bulletDirection.y * bulletSpeed) ;	//Velocidad de la bala

		Invoke ("DestroyBullet", destroyTime); 		//Invoca el método para destruir la bala en el tiempo indicado

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Destruye el objeto para que no sea eternal :v
	void DestroyBullet () {

		Destroy (transform.gameObject);				//Destruye objeto al cual se asigna el script (la bala)
		Instantiate (impactPrefab, transform.position, transform.rotation);		//Instancia el efecto de impacto

	}

	//---------------------------------------------------------------------------------------------------------------------



}
