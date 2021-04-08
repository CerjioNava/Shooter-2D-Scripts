using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	public GameObject bulletPrefab1, bulletPrefab2, bulletPrefab3, deathPrefab;	//Prefab de las balas que podrán cambiarse y la muerte
	private GameObject bulletPrefab;								//Prefab en el que se seleccinara la bala
	public Transform firePointForward, firePointUp;					//Puntos de disparo
	private Transform firePoint;									//Punto de disparo general
	private Animator playerAnim;									//Animator del player
	private Camera cam;												//Objeto camera de la main camera
	private Vector3 aimingDir, characterScale, directionMov;		//Dirección de apuntado, Escala del player y direccion de movimiento
	private Rigidbody2D rb, rbEnemy;								//RigidBody2D del player

	private float horizontalMov, verticalMov, aimingAngle, takeDamage;			//Movimiento, ángulo de apuntado y daño recibido
	private bool isShooting = false, isDeath = false, specialEnable, activeSpecial = false, shotgun = false, canAttack = true, isHurting = false;			
	private string shootPar = null;									//Parámetro shooting a cambiar (up o forward)
	private float playerHealth, maxHealth;				//Vida en 100 y máximo
	private float shootTime = 0.15f, moveSpeed = 2f;					//velocidad de movimiento del player y disparo
	private int load;

	private PlayerControl control;

		//Método llamado al iniciar el script
	void Awake () {

		rb = GetComponent <Rigidbody2D> ();
		playerAnim = GetComponent <Animator> ();							//Asignación de animator
		cam = GameObject.Find ("Main Camera").GetComponent <Camera> ();		//Asignación de camera
		bulletPrefab = bulletPrefab1;										//Asignación de la bala inicial
		aimingAngle = -90f;
		canAttack = true;

		control = GetComponent <PlayerControl> ();

		playerHealth = GetComponent <PlayerStats> ().playerHealth;
		maxHealth = GetComponent <PlayerStats> ().maxHealth;
		shootTime = GetComponent <PlayerStats> ().shootTime;
		moveSpeed = GetComponent <PlayerStats> ().moveSpeed;

	}
	
		//Método llamado cada frame
	void Update () {

		horizontalMov = control.MoveDirection ().x;			//Asignación de movimiento horizontal	
		verticalMov = control.MoveDirection ().y;				//Asignación de movimiento vertical

		if (control.Action ("Attack")) {		//Si se presiona el botón de disparo

			aimingDir.x = control.AttackDirection ().x;
			aimingDir.y = control.AttackDirection ().y;

			aimingAngle = Mathf.Atan2 (aimingDir.y, aimingDir.x) * Mathf.Rad2Deg;	//Se calcula el ángulo de rotación

		} else 				
			aimingAngle = -90f;

		WeaponChange ();		//Se verifica un cambio de bala
			
	}

		//Método llamado por cada intervalo entre frames
	void FixedUpdate () {
		
		if (!playerAnim.GetBool ("GetHurt") && canAttack) {
		
			Flip ();						//Se verifica el volteo del personaje
			PlayerMovement ();				//Se verifica el movimiento del personaje
			Shoot ();						//Se verifica la acción de disparo

		}

	}


	//---------------------------------------------------------------------------------------------------------------------

		//Si detecta una colisión DAÑO
	void OnTriggerEnter2D(Collider2D collision) {

		PlayerHurt (collision);

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Si detecta una colisión DAÑO
	void OnCollisionEnter2D(Collision2D collision) {

		PlayerHurt (collision.collider);

	}

	//---------------------------------------------------------------------------------------------------------------------

	void OnCollisionStay2D (Collision2D collision) {

		PlayerHurt (collision.collider);

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Daño al player
	void PlayerHurt(Collider2D collision) {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask("ProyectilEnemigo", "Enemy")) 
			&& !isHurting) {

			if (collision.GetComponentInParent <AttackDamage> ().GetDamage () != 0) {

				playerAnim.SetBool ("GetHurt", true); 

				FindObjectOfType<AudioManager> ().PlayAudio ("Player Hurt");

				rbEnemy = collision.GetComponentInParent <Rigidbody2D> ();

				rb.velocity = new Vector3 (1.5f * -characterScale.x, rbEnemy.velocity.y, 0f);

				Invoke ("StopHurt", 0.5f);
				isHurting = true;

				takeDamage = collision.GetComponentInParent <AttackDamage> ().GetDamage ();
				playerHealth -= takeDamage;

				GetComponent <PlayerStats> ().LoseHealth (playerHealth);

			}

		} 

		if (playerHealth <= 0 && !isDeath) {

			isDeath = true;
			Invoke("Death", 0.25f);

		}

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Movimiento del personaje
	void PlayerMovement() {

		directionMov = new Vector3 (horizontalMov*moveSpeed*Time.deltaTime, verticalMov*moveSpeed*Time.deltaTime, 0f);  
		transform.Translate (directionMov);		//Traslación
		Run ();						//Correr

	}

	public Vector3 GetDirection () {
		return directionMov;
	}

	//---------------------------------------------------------------------------------------------------------------------

		//Volteo del personaje
	void Flip () {

		characterScale = transform.localScale;							//Escala inicial

		if (horizontalMov > 0.1f && characterScale.x == -1)  {				//Si se mueve a la derecha y la escala es izquierda
			
			characterScale.x = 1;										//Se asigna escala derecha
			transform.localScale = characterScale;						//a la escala local

			firePointForward.transform.Rotate (0f, 180f, 0f);			//Y se devuelve la rotación del firePoint

		} else if (horizontalMov < -0.1f && characterScale.x == 1) {		//Si se mueve a la izquierda y la escala es derecha

			characterScale.x = -1;										//Se asigna escala izquierda
			transform.localScale = characterScale;						//a la escala local

			firePointForward.transform.Rotate (0f, -180f, 0f);			//Y se rota el firePoint para que apunte a la izquierda

		}
			

		if ( control.AttackDirection ().x < -0.1f && characterScale.x == 1 ) {			//Si se dispara y apunta a la izquierda
																								//mientras está en la derecha
			characterScale.x = -1;																//se asigna escala izquierda
			transform.localScale = characterScale;												//a la escala local
			firePointForward.transform.Rotate (0f, -180f, 0f);			//Y se rota el firePoint para que apunte a la izquierda

		} else if ( control.AttackDirection ().x > 0.1f && characterScale.x == -1) {		//Si se dispara y apunta a la derecha
																								//mientras está en la izquierda
			characterScale.x = 1;																//se asigna la escala derecha 
			transform.localScale = characterScale;												//a la escala local
			firePointForward.transform.Rotate (0f, 180f, 0f);			//Y se devuelve la rotación del firePoint

		}

	}


	//---------------------------------------------------------------------------------------------------------------------

		//El personaje corre
	void Run () {

		if ( (Mathf.Abs (horizontalMov) > 0.1f || Mathf.Abs (verticalMov) > 0.1f) && !playerAnim.GetBool ("Running")) {		//Si se mueve en cualquier dirección y
																								//no estaba corriendo
			playerAnim.SetBool ("Running", true);												//Habilita el parámetro de correr	

		} else if ( (Mathf.Abs (horizontalMov) < 0.1f && Mathf.Abs (verticalMov) < 0.1f) && playerAnim.GetBool ("Running")) { //Si ya no se mueve y estaba corriendo

			playerAnim.SetBool ("Running", false);												//Deshabilita el parámetro

		}


	}

	//---------------------------------------------------------------------------------------------------------------------

		//El personaje dispara
	void Shoot () {

		if (control.Action ("Attack") && canAttack) {

			if (control.Action ("Normal Weapon") || control.Action ("Charged Weapon")) {
			
				if (!playerAnim.GetBool ("Shooting") && !playerAnim.GetBool ("ShootingUp")) {	//Si se presiona el disparo

					Bullets ();												//Se llama la función de balas
								
				} else if (!isShooting) {		//Si se sigue disparando

					isShooting = true;										//Se hace true isShooting

					if (!activeSpecial)
						Invoke ("Bullets", shootTime);						//Se invoca el método bullets tras 0.15 segundos
					else
						Invoke ("Bullets", shootTime * 1.5f);					//Se invoca el método bullets tras 0.15*1.5 segundos
			
				}

			} else if (control.Action ("Special Weapon") && control.AttackDirection ().x != 0
						&& !shotgun && !playerAnim.GetBool ("ShootingUp")) {

				if (!playerAnim.GetBool ("Shooting"))
					playerAnim.SetBool ("Shooting", true);	
			
				Instantiate (bulletPrefab3, firePointForward.position, firePointForward.rotation);
				InvokeRepeating ("Shotgun", 0.8f, 0.4f);

				CancelInvoke ("StopShooting");
				Invoke ("StopShooting", 0.25f);
				Invoke ("Temp", 0.25f);

				CancelInvoke ("Bullets");

				isShooting = true;
				canAttack = false;
				shotgun = true;

				playerAnim.SetBool ("Running", false);

			}

		}

		if (!control.Action ("Attack") && isShooting) {				//Si ya no se dispara

			isShooting = false;
			Invoke ("StopShooting", 0.2f);					//Tras 0.2 segundos se llama al método StopShooting

		}


	}

	//---------------------------------------------------------------------------------------------------------------------

		//Temporal
	void Temp () {

		canAttack = true;
		isShooting = false;

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Shotgun
	void Shotgun () {

		if (load == 0) {
			FindObjectOfType<AudioManager> ().PlayAudio ("Shotgun Load 1");
			load += 1;
		} else if (load == 1) {
			FindObjectOfType<AudioManager> ().PlayAudio ("Shotgun Load 2");
			load += 1;
		} else if (load == 2) {
			FindObjectOfType<AudioManager> ().PlayAudio ("Shotgun Load 3");
			load += 1;
		} else if (load == 3) {
			shotgun = false;
			load = 0;
			playerAnim.SetBool ("Shooting", false);	
			CancelInvoke ("Shotgun");
		}

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Método que maneja los disparos
	void Bullets () {

		Aiming ();																//Llamado al método de apuntado
		Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);		//Se instancia la bala elegida en la posición indicada
		isShooting = false;														//Se hace isShooting false

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Apuntado de disparo
	void Aiming() {
		

		if ( ((60f < aimingAngle) && (aimingAngle < 120f)) ) {			//Si se apunta en el rango de arriba
															
			playerAnim.SetBool ("Shooting", false);						//Se deshabilita el shooting lateral y se
			shootPar = "ShootingUp";									//habilita el shooting ascendente
			firePoint = firePointUp;									//se asigna el nuevo punto de disparo

		} else  {

			playerAnim.SetBool ("ShootingUp", false);					//Se deshabilita el shooting ascendente y se
			shootPar = "Shooting";										//habilita el shooting lateral
			firePoint = firePointForward;								//se asigna el nuevo punto de disparo

		}

		playerAnim.SetBool (shootPar, true);							//Se habilita el nuevo shooting

	}

	//---------------------------------------------------------------------------------------------------------------------

		//Se detiene los disparos
	void StopShooting () {

		playerAnim.SetBool ("Shooting", false);					//Se deshabilitan ambos parámetros de disparo
		playerAnim.SetBool ("ShootingUp", false);

	}

	//---------------------------------------------------------------------------------------------------------------------
		
	//Cambio de balas
	void WeaponChange () {

		if (control.Action ("Charged Weapon") && specialEnable && !activeSpecial) {			//Si se pulsa el '2' se cambia a una bala mas mamalona

			activeSpecial = true;
			FindObjectOfType<SpecialFill> ().InvokeUnfill ();
			bulletPrefab = bulletPrefab2;

		}

		if ((control.Action ("Normal Weapon") || !specialEnable) && bulletPrefab == bulletPrefab2) {
			activeSpecial = false;
			bulletPrefab = bulletPrefab1;
		}

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Habilita las balas especial
	public void SpecialEnable(bool enable) {

		specialEnable = enable;
	
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Detener el daño
	void StopHurt () {

		Invoke ("NotHurt", 0.25f);
		playerAnim.SetBool ("GetHurt", false);
		rb.velocity = new Vector3 (0f, 0f, 0f);

	}

	void NotHurt () {
		isHurting = false;
		rb.velocity = new Vector3 (0f, 0f, 0f);
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Muerte del player
	void Death () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Player Death");
		Instantiate (deathPrefab, transform.position, transform.rotation);
		StopHurt ();
		Invoke ("DeathScene", 1.1f); 
		gameObject.SetActive (false);

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Escena de muerte del player
	void DeathScene () {

		GetComponent <SpriteRenderer> ().enabled = true;
		playerAnim.SetBool ("Die", false);
		isDeath = false;
		FindObjectOfType <EndGame> ().Death ();

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Otorga vida al player
	public void SetHealth ( float Health ) {
		playerHealth = Health;
	}


	public Transform GetFirepoint () {
		return firePoint;
	}

}
