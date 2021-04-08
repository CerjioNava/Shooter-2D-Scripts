using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour {

	public GameObject healPrefab;
	private Animator objectAnim;

	public float objectHealth;
	private float takeDamage;

	void Awake () {

		objectAnim = GetComponent <Animator> ();

	}

	void OnTriggerEnter2D(Collider2D collision) {

		if (GetComponent <Collider2D> ().IsTouchingLayers (LayerMask.GetMask ("Proyectil", "Weapon", "ProyectilEnemigo"))) {

			takeDamage = collision.GetComponentInParent <AttackDamage> ().GetDamage ();
			objectHealth -= takeDamage;

			if (!objectAnim.GetBool ("Hit")) {

				FindObjectOfType<AudioManager> ().PlayAudio ("Object Hit");
				CancelInvoke ("StopHit");
				objectAnim.SetBool ("Hit", true);
				Invoke ("StopHit", 0.25f);

			}

			if (objectHealth <= 0f) {

				FindObjectOfType<AudioManager> ().PlayAudio ("Object Break");
				CancelInvoke ("StopHit");
				objectAnim.SetBool ("Destroy", true);

				if (Random.Range (0, 6) == 5 && (gameObject.name.StartsWith ("Box") || gameObject.name.StartsWith ("Barrel")))
					Instantiate (healPrefab, transform.position, transform.rotation);

			}

		}

	}

	void OnCollisionEnter2D() {
	
		if (GetComponent <Collider2D> ().IsTouchingLayers (LayerMask.GetMask("Player")))
			FindObjectOfType<AudioManager> ().PlayAudio ("No Way");
	
	}

	void StopHit () {

		objectAnim.SetBool ("Hit", false);

	}


}
