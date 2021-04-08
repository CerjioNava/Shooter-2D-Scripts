using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

	private Rigidbody2D rb;

	public bool isBoss;
	public float enemyHealth, maxHealth, vision;
	private float takeDamage, knockback, scaleX, scaleTemp;
	public bool isFreeze, isDeath;
	private Vector3 moveDirection, initialPosition, scale;

	public GameObject HealthBar, healDrop;

	void Awake () {

		rb = GetComponent <Rigidbody2D> ();
		enemyHealth = maxHealth;
		initialPosition = transform.position;
		scale = HealthBar.transform.localScale;
		scaleTemp = scale.x;

	}
	

	void Update () {
	
		if (isFreeze)
			rb.velocity =  new Vector2 (0f, 0f);
		
	}


	//Al chocar con un proyectil
	void OnTriggerEnter2D(Collider2D collision) {

		if (GetComponent <Collider2D> ().IsTouchingLayers (LayerMask.GetMask ("Proyectil", "Weapon"))) {

			Hurt (collision);

			if (isBoss)
				FindObjectOfType<BossHealthBar> ().SetBossHealth (enemyHealth, maxHealth);
			else
				HealthBarEnable ();

		} 

		if (enemyHealth <= 0 && !isDeath) {

			isDeath = true;

			if (!isBoss)
				FindObjectOfType <EnemyLeft> ().EnemyDeath ();
			else
				GetComponent <BossDefeat> ().BossIsDeath ();

			if (Random.Range (0, 6) == 5)
				Instantiate (healDrop, transform.position, transform.rotation);
		
		}

	}

	//Recibe daño y knockback
	void Hurt (Collider2D collision) {

		takeDamage = collision.GetComponentInParent <AttackDamage> ().GetDamage ();
		enemyHealth -= takeDamage;

		if (enemyHealth < 0)
			enemyHealth = 0f;

		knockback = collision.GetComponentInParent <AttackDamage> ().GetKnockback () / GetComponent <AttackDamage> ().GetKnockbackResist ();
		transform.Translate (knockback*-moveDirection.x*Time.deltaTime, knockback*-moveDirection.y*Time.deltaTime, 0f);

		if (collision.GetComponentInParent <AttackDamage> ().GetFreeze ()) {
			isFreeze = true;	
			CancelInvoke ("NotFreeze");
			Invoke ("NotFreeze", collision.GetComponentInParent <AttackDamage> ().GetFreezeTime ());
		}

	}


	//Devuelve isBoss
	public bool GetBoss() {
		return isBoss;
	}


	//Se acaba el congelamiento
	void NotFreeze () {
		isFreeze = false;
	}


	//Devuelve isFreeze
	public bool GetFreeze () {
		return isFreeze;
	}


	//Devuelve Health
	public float GetHealth () {
		return enemyHealth;
	}


	//Devuelve isDeath
	public bool GetDeath () {
		return isDeath;
	}

	//Define el movimiento para knockback
	public void SetMoveDirection (Vector3 moveDir) {
		moveDirection = moveDir;
	}


	//Devuelve la magnitud de vision
	public float GetVision () {
		return vision;
	}



	public Vector3 GetInitialPosition () {

		return initialPosition;

	}

	void HealthBarEnable () {

		if (HealthBar != null) {

			HealthBar.SetActive (true);

			scale.x = scaleTemp * enemyHealth / maxHealth;
			HealthBar.transform.localScale = scale;

			CancelInvoke ("HealthBarDisable");
			Invoke ("HealthBarDisable", 1f);

		}

	}

	void HealthBarDisable () {

		HealthBar.SetActive (false);

	}

}
