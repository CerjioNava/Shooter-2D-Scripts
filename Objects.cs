using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour {

	public int objectNumber;
	public float heartValue, healValue;
	public Animator anim;

	void Awake () {

		if (gameObject.name.StartsWith ("Heart")) {

			objectNumber = 1;
			heartValue = 100f;

		} else if (gameObject.name.StartsWith ("Heal")) {

			objectNumber = 2;
			healValue = 20f;

		} else if (gameObject.name.StartsWith ("Yellow Key"))
			objectNumber = 3;
		
		else if (gameObject.name.StartsWith ("Blue Key"))
			objectNumber = 4;
		
		else if (gameObject.name.StartsWith ("Red Key"))
			objectNumber = 5;

		anim = GetComponent <Animator> ();

	}
	
	void OnTriggerEnter2D (Collider2D collision) {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) {
			
			if (objectNumber == 1) {		//Heart

				FindObjectOfType<AudioManager> ().PlayAudio ("Healing");
				FindObjectOfType<OtherStatus> ().EnableStatus (9);
				collision.GetComponentInParent <PlayerStats> ().UpdateHealth (heartValue);
				Destroy (gameObject, 0.833f);

			} else if (objectNumber == 2) {		//Heal

				FindObjectOfType<AudioManager> ().PlayAudio ("Healing");
				FindObjectOfType<OtherStatus> ().EnableStatus (8);
				collision.GetComponentInParent <PlayerStats> ().UpdateHealth (healValue);
				Destroy (gameObject, 0.833f);

			} else if (objectNumber == 3) {		//Yellow Key

				FindObjectOfType<AudioManager> ().PlayAudio ("Get Key");
				FindObjectOfType<OtherStatus> ().EnableStatus (5);
				collision.GetComponentInParent <PlayerStats> ().GetKey ("Yellow Key", true);
				Invoke ("DisableObject", 0.833f);
			
			} else if (objectNumber == 4) { 	//Blue Key

				FindObjectOfType<AudioManager> ().PlayAudio ("Get Key");
				FindObjectOfType<OtherStatus> ().EnableStatus (6);
				collision.GetComponentInParent <PlayerStats> ().GetKey ("Blue Key", true);
				Invoke ("DisableObject", 0.833f);

			} else if (objectNumber == 5) {		//Red Key

				FindObjectOfType<AudioManager> ().PlayAudio ("Get Key");
				FindObjectOfType<OtherStatus> ().EnableStatus (7);
				collision.GetComponentInParent <PlayerStats> ().GetKey ("Red Key", true);
				Invoke ("DisableObject", 0.833f);
			
			}

			anim.SetBool ("Taken", true);
					
		}

	}


	void DisableObject () {
		
		gameObject.SetActive (false);

	}

}
