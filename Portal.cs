using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public GameObject objectToInstance;
	private GameObject player;
	public float distanceX, distanceY;
	public bool Boss;

	private Vector3 position;
	private Animator anim;

	void Awake () {
		
		player = FindObjectOfType<LookAt> ().GetTarget ();
		anim = GetComponent <Animator> ();

		Invoke ("PortalOpen", 0.667f);

		position = transform.position;
		position.x += distanceX;
		position.y += distanceY;

		FindObjectOfType<AudioManager> ().PlayAudio ("Respawn");
		LookAtPortal ();

	}

	void OnTriggerEnter2D (Collider2D collision) {
	
		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) {

			collision.gameObject.SetActive (false);
			FindObjectOfType <EndGame> ().LevelComplete();
			Invoke ("PortalClose", 0.5f);
			FindObjectOfType<AudioManager> ().PlayAudio ("Respawn");

		}
	
	}


	void PortalOpen () {

		anim.SetBool ("Open", true);

		if (Boss) {
		
			Invoke ("PortalClose", 1f);
			Invoke ("InstantiateObject", 0.5f);

		} else
			FindObjectOfType<EndGame> ().BossBar.SetActive (false);

	}

	void PortalClose () {

		anim.SetBool ("Close", true);
		Destroy (gameObject, 0.5f);

	}

	public void LookAtPortal () {

		FindObjectOfType<LookAt> ().SetTarget (gameObject);
		Invoke ("LookAtPlayer", 2f);

	}

	public void LookAtPlayer () {

		FindObjectOfType<LookAt> ().SetTarget (player);

	}

	void InstantiateObject () {

		Instantiate (objectToInstance, position, transform.rotation);
		FindObjectOfType<AudioManager> ().PlayAudio ("Respawn");

	}

}
