using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Behaviour : MonoBehaviour {

	public GameObject arrowPrefab, magicPrefab;	
	public Transform firePoint;
	private Animator playerAnim;
	private Camera cam;
	private Vector3 aimingDir, characterScale, directionMov, shotPosition;									
	private Rigidbody2D rb, rbEnemy;			

	private float horizontalMov, verticalMov;						
	private float aimingAngle;										
	private bool isAttacking = false, isDeath = false, specialEnable, activeSpecial = false, isHurting = false;				
	private float playerHealth = 150f, maxHealth = 150f, shootTime = 0.75f, moveSpeed = 3f;
	private float takeDamage;

	private PlayerControl control;

	void Awake () {
		
		rb = GetComponent <Rigidbody2D> ();
		playerAnim = GetComponent <Animator> ();							//Asignación de animator

		control = GetComponent <PlayerControl> ();

		playerHealth = GetComponent <PlayerStats> ().playerHealth;
		maxHealth = GetComponent <PlayerStats> ().maxHealth;
		shootTime = GetComponent <PlayerStats> ().shootTime;
		moveSpeed = GetComponent <PlayerStats> ().moveSpeed;

	}

	void Update () {

		horizontalMov = control.MoveDirection ().x;			//Asignación de movimiento horizontal	
		verticalMov = control.MoveDirection ().y;				//Asignación de movimiento vertical

		if (playerAnim.GetBool ("Preparing") && !playerAnim.GetBool ("Ready")) {	//Si esta sacando la espada y aun no esta ready

			horizontalMov = 0f;
			verticalMov = 0f;

		}

		WeaponChange ();			//Se verifica un cambio de arma especial

	}

	void FixedUpdate () {

		if (!playerAnim.GetBool ("Hurt") && !playerAnim.GetBool ("Die")) {

			Flip ();						//Se verifica el volteo del personaje

			if (!playerAnim.GetBool ("Attack1") && !playerAnim.GetBool ("Bow") && !playerAnim.GetBool ("Cast")) 
				PlayerMovement ();				//Se verifica el movimiento del personaje

			if (control.Action ("Attack"))
				Attack ();						//Se verifica la acción de disparo

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
			&& !isHurting && !playerAnim.GetBool ("Die")) {

			if (collision.GetComponentInParent <AttackDamage> ().GetDamage () != 0) {

				playerAnim.SetBool ("Hurt", true); 

				FindObjectOfType<AudioManager> ().PlayAudio ("Player Hurt");

				rbEnemy = collision.GetComponentInParent <Rigidbody2D> ();

				rb.velocity = new Vector3 (1.5f * -characterScale.x, rbEnemy.velocity.y, 0f);

				Invoke ("StopHurt", 0.4f);
				isHurting = true;

				takeDamage = collision.GetComponentInParent <AttackDamage> ().GetDamage ();
				playerHealth -= takeDamage;

				GetComponent <PlayerStats> ().LoseHealth (playerHealth);

			}

		} 

		if (playerHealth <= 0 && !isDeath) {

			isDeath = true;
			Invoke("Death", 0.35f);

		}

	}


	//---------------------------------------------------------------------------------------------------------------------

	//Movimiento del personaje
	void PlayerMovement() {

		directionMov = new Vector3 (horizontalMov*moveSpeed*Time.deltaTime, verticalMov*moveSpeed*Time.deltaTime, 0f);  
		transform.Translate (directionMov);		//Traslación
		Run ();									//Correr

	}

	public Vector3 GetDirection () {
		return directionMov;
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Volteo del personaje
	void Flip () {

		characterScale = transform.localScale;							//Escala inicial

		if (!playerAnim.GetBool ("Attack1") && !playerAnim.GetBool ("Bow") && !playerAnim.GetBool ("Cast")) {

			if (horizontalMov > 0.1f && characterScale.x == -1.5f) {				//Si se mueve a la derecha y la escala es izquierda

				characterScale.x = 1.5f;										//Se asigna escala derecha
				transform.localScale = characterScale;						//a la escala local

				firePoint.transform.Rotate (0f, 180f, 0f);			//Y se devuelve la rotación del firePoint

			} else if (horizontalMov < -0.1f && characterScale.x == 1.5f) {		//Si se mueve a la izquierda y la escala es derecha

				characterScale.x = -1.5f;										//Se asigna escala izquierda
				transform.localScale = characterScale;						//a la escala local

				firePoint.transform.Rotate (0f, -180f, 0f);			//Y se rota el firePoint para que apunte a la izquierda

			}

		}
			
		if (control.AttackDirection ().x < -0.1f && characterScale.x == 1.5f) {			//Si se dispara y apunta a la izquierda
																				//mientras está en la derecha
			characterScale.x = -1.5f;															//se asigna escala izquierda
			transform.localScale = characterScale;												//a la escala local
			firePoint.transform.Rotate (0f, -180f, 0f);						//Y se devuelve la rotación del firePoint

		} else if (control.AttackDirection ().x > 0.1f && characterScale.x == -1.5f) {		//Si se dispara y apunta a la derecha
																				//mientras está en la izquierda
			characterScale.x = 1.5f;															//se asigna la escala derecha 
			transform.localScale = characterScale;												//a la escala local
			firePoint.transform.Rotate (0f, 180f, 0f);						//Y se devuelve la rotación del firePoint

		}

	}


	//---------------------------------------------------------------------------------------------------------------------

	//El personaje corre
	void Run () {

		if ( (Mathf.Abs (horizontalMov) > 0.1f || Mathf.Abs (verticalMov) > 0.1f) && !playerAnim.GetBool ("Run")) {		//Si se mueve en cualquier dirección y
																								//no estaba corriendo
			playerAnim.SetBool ("Run", true);												//Habilita el parámetro de correr

			if (playerAnim.GetBool ("Ready"))				//Si ya estaba preparado, detiene el stop ready
				CancelInvoke ("StopReady");

		} else if ( (Mathf.Abs (horizontalMov) < 0.1f && Mathf.Abs (verticalMov) < 0.1f) && playerAnim.GetBool ("Run")) { //Si ya no se mueve y estaba corriendo

			playerAnim.SetBool ("Run", false);												//Deshabilita el parámetro

			if (playerAnim.GetBool ("Ready"))				//Si ya estaba preparado y deja de correr, activa el stop ready
				Invoke ("StopReady", 3f);

		}


	}

	//---------------------------------------------------------------------------------------------------------------------

	//El personaje dispara
	void Attack () {

		if ( control.Action ("Normal Weapon") && !playerAnim.GetBool ("Bow") && !playerAnim.GetBool ("Cast")) {

			if (!playerAnim.GetBool ("Preparing")) {			//Saca la espada y entra en posición de batalla

				playerAnim.SetBool ("Preparing", true);
				Invoke ("ActiveReady", 0.333f);
				Invoke ("StopReady", 3f);

			} else if (playerAnim.GetBool ("Ready")) {			//Preparado y listo para atacar
				
				if (!playerAnim.GetBool ("Attack1") && !isAttacking) 
					IsAttacking ("Attack1");
				else if (!playerAnim.GetBool ("Attack2") && !isAttacking) 
					IsAttacking ("Attack2");
				else if (!playerAnim.GetBool ("Attack3") && !isAttacking) 
					IsAttacking ("Attack3");
				
				Invoke ("StopAttack", 0.5f);

			}

		}

		if ( (control.Action ("Special Weapon") || control.Action ("Charged Weapon")) && control.AttackDirection ().x != 0 
			&& !playerAnim.GetBool ("Attack1") && !playerAnim.GetBool ("Bow") && !playerAnim.GetBool ("Cast")) {

			Shoot ();

		}
			

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Vuelve a atacar
	void IsAttacking (string attackNumber) {

		FindObjectOfType<AudioManager> ().PlayAudio ("Meele Attack");
		playerAnim.SetBool (attackNumber, true);
		isAttacking = true;
		Invoke ("IsNotAttacking", 0.35f);
		CancelInvoke ("StopAttack");
		CancelInvoke ("StopReady");

	}

	//Ya no ataca
	void IsNotAttacking () {

		isAttacking = false;

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Disparo
	void Shoot() {

		CancelInvoke ("StopAttack");
		StopAttack ();
		CancelInvoke ("StopReady");
		CancelInvoke ("ActiveReady");

		if (!activeSpecial) {
			
			playerAnim.SetBool ("Bow", true);
			Invoke ("ArrowInstance", 0.6f); 
			Invoke ("StopAttack", 0.75f);

		} else {

			playerAnim.SetBool ("Cast", true);

			if (!playerAnim.GetBool ("CastLoop"))
				Invoke ("ActiveLoop", 0.333f);

			Invoke ("MagicInstance", 0.3f); 
			Invoke ("StopAttack", 0.75f);
			
		}

	}
		
	//---------------------------------------------------------------------------------------------------------------------

	//Detiene el ataque
	void StopAttack () {
		
		if (!playerAnim.GetBool ("Bow")) {

			if (playerAnim.GetBool ("Attack3"))
				playerAnim.SetBool ("Attack3", false);
		
			if (playerAnim.GetBool ("Attack2"))
				playerAnim.SetBool ("Attack2", false);

			if (playerAnim.GetBool ("Attack1"))
				playerAnim.SetBool ("Attack1", false);
			
		} else if (playerAnim.GetBool ("Bow")) {
			
			playerAnim.SetBool ("Bow", false);
			playerAnim.SetBool ("Ready", false);
			playerAnim.SetBool ("Preparing", true);
			Invoke ("ActiveReady", 0.333f);
		
		}

		if (playerAnim.GetBool ("Cast")) {

			playerAnim.SetBool ("Cast", false);

			if (!Input.GetKey (KeyCode.Space))
				playerAnim.SetBool ("CastLoop", false);

			playerAnim.SetBool ("Ready", false);
			playerAnim.SetBool ("Preparing", true);
			Invoke ("ActiveReady", 0.333f);

		}

		Invoke ("StopReady", 3f);

	}

	//---------------------------------------------------------------------------------------------------------------------

	void ActiveReady () {
		playerAnim.SetBool ("Ready", true);
	}

	void StopReady () {
		playerAnim.SetBool ("Ready", false);
		Invoke ("StopPreparing", 0.35f);
	}

	//---------------------------------------------------------------------------------------------------------------------

	void ActiveLoop () {
		playerAnim.SetBool ("CastLoop", true);
	}

	//---------------------------------------------------------------------------------------------------------------------

	void StopPreparing() {
		playerAnim.SetBool ("Preparing", false);
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Detener el daño
	void StopHurt () {

		Invoke ("NotHurt", 0.25f);
		playerAnim.SetBool ("Hurt", false);
		rb.velocity = new Vector3 (0f, 0f, 0f);

	}

	void NotHurt () {
		isHurting = false;
		rb.velocity = new Vector3 (0f, 0f, 0f);
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Muerte del player
	void Death () {

		playerAnim.SetBool ("Die", true);
		FindObjectOfType<AudioManager> ().PlayAudio ("Player Death");
		StopHurt ();
		Invoke ("DeathScene", 1f); 

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Escena de muerte del player
	void DeathScene () {

		GetComponent <SpriteRenderer> ().enabled = true;
		playerAnim.SetBool ("Die", false);
		isDeath = false;
		gameObject.SetActive (false);
		FindObjectOfType <EndGame> ().Death ();

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Habilita las balas especial
	public void SpecialEnable(bool enable) {

		specialEnable = enable;

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Cambio de balas
	void WeaponChange () {

		if (control.Action ("Charged Weapon") && specialEnable && !activeSpecial) {			//Si se pulsa el '2' se cambia a una bala mas mamalona

			FindObjectOfType<SpecialFill> ().InvokeUnfill ();
			activeSpecial = true;

		} 

		if ((control.Action ("Special Weapon") || !specialEnable) && activeSpecial) {
			activeSpecial = false;
		}

	}

	//---------------------------------------------------------------------------------------------------------------------

	void ArrowInstance () {
		
		Instantiate (arrowPrefab, firePoint.position, firePoint.rotation);

	}

	//---------------------------------------------------------------------------------------------------------------------

	void MagicInstance () {

		Instantiate (magicPrefab, firePoint.position, firePoint.rotation);

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Otorga vida al player
	public void SetHealth ( float Health ) {
		playerHealth = Health;
	}

}
