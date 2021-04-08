using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBehaviour : MonoBehaviour {

	public GameObject crystalParent, laserPrefab;
	public Transform firePoint;

	private Rigidbody2D rb;
	private Animator crystalAnim;
	private GameObject Player;
	public Vector3 direction, enemyScale;

	private float magnitud, magnitudHit = 4f;
	private bool isAttacking;
	public float crystalHealth, angle, shotAngle;

	void Awake () {

		crystalHealth = GetComponent <EnemyStats> ().GetHealth ();
		magnitudHit = GetComponent <EnemyStats> ().GetVision ();

		Player = FindObjectOfType<LookAt> ().GetTarget ();
		rb = GetComponent <Rigidbody2D> ();
		crystalAnim = GetComponent <Animator> ();

	}

	void Update () {

		direction = Player.transform.position - crystalParent.GetComponent <Transform> ().position;
		magnitud = Mathf.Sqrt (Mathf.Pow (direction.x, 2) + Mathf.Pow (direction.y, 2));
		angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg ;	

		Flip ();

		if (magnitud <= magnitudHit && !isAttacking) {
			
			CrystalAttack ();
			isAttacking = true;

		}

	}

	//Al chocar con un proyectil
	void OnTriggerEnter2D () {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Proyectil", "Weapon"))) {

			crystalHealth = GetComponent <EnemyStats> ().GetHealth ();
			FindObjectOfType<AudioManager> ().PlayAudio ("Crystal Hurt");
			crystalAnim.SetBool ("Hurt", true);
			Invoke ("StopHurt", 0.6f);

		} else if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) 
			FindObjectOfType<AudioManager> ().PlayAudio ("Claws");

		if (crystalHealth <= 0) 
			CrystalDeath ();

	}

	void CrystalAttack () {
		
		if (magnitud <= magnitudHit) {

			crystalAnim.SetBool ("Attack", true);
			Invoke ("ShotAngle", 0.9f);
			Invoke ("LaserShot", 1f);
			Invoke ("StopAttack", 1.5f);
			Invoke ("NotAttacking", 3f);
	
		} else if (magnitud > magnitudHit) {
			
			CancelInvoke ("CrystalAttack");

		}

	}

	void ShotAngle() {
		shotAngle = angle;
	}

	void LaserShot () {
	
		firePoint.Rotate (0f, 0f, shotAngle);
		Instantiate (laserPrefab, firePoint.position, firePoint.rotation);
		firePoint.Rotate (0f, 0f, -shotAngle);

	}

	void StopAttack () {
		crystalAnim.SetBool ("Attack", false);
	}

	void NotAttacking () {
		isAttacking = false;
	}

	void StopHurt () {
		crystalAnim.SetBool ("Hurt", false);
	}

	public void CrystalDeath () {

		if (!crystalAnim.GetBool ("Death")) {

			FindObjectOfType<AudioManager> ().PlayAudio ("Object Break");
			crystalAnim.SetBool ("Death", true);
			Destroy (crystalParent, 1f);

		}
	}

	//Volteo del personaje
	void Flip () {

		enemyScale = transform.localScale;

		if (direction.x > 0f && enemyScale.x == 1f) {				

			enemyScale.x = -1f;
			transform.localScale = enemyScale;

		} else if (direction.x < 0f && enemyScale.x == -1f) {

			enemyScale.x = 1f;
			transform.localScale = enemyScale;			

		}

		if (45f < angle && angle < 135f && enemyScale.y == 1f) {	
		
			enemyScale.y = -1f;
			transform.localScale = enemyScale;				

		} else if ((135f < angle || angle < 45f) && enemyScale.y == -1f) {

			enemyScale.y = 1f;
			transform.localScale = enemyScale;				

		}

	}


}
