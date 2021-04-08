using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDoor : MonoBehaviour {

	public string doorColor;
	private Animator anim;

	void Awake () {

		anim = GetComponent <Animator> ();

		if (gameObject.name.StartsWith ("Magic Door YELLOW"))
			doorColor = "Yellow";
		else if (gameObject.name.StartsWith ("Magic Door BLUE"))
			doorColor = "Blue";
		else if (gameObject.name.StartsWith ("Magic Door RED"))
			doorColor = "Red";
		
	}

	void OnCollisionEnter2D(Collision2D collision) {
	
		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask("Player"))) {

			if (collision.collider.GetComponentInParent <PlayerStats> ().ReturnKey (doorColor)) {

				FindObjectOfType<AudioManager> ().PlayAudio ("Door Open");
				anim.SetBool ("Open", true);
				Destroy (gameObject, 0.833f);				
			
			} else {
			
				FindObjectOfType<AudioManager> ().PlayAudio ("No Way");

				if (doorColor == "Yellow")
					FindObjectOfType<OtherStatus> ().EnableStatus (2);
				else if (doorColor == "Blue")
					FindObjectOfType<OtherStatus> ().EnableStatus (3);
				else if (doorColor == "Red")
					FindObjectOfType<OtherStatus> ().EnableStatus (4);


			}
		}

	}

}
