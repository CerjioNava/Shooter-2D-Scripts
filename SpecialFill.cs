using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialFill : MonoBehaviour {

	public Image special1;
	public bool isFilled;
	public GameObject player;

	public void Awake () {
		player = FindObjectOfType<LookAt> ().GetTarget ();
	}

	public void FillSpecial1 (float fill) {

		if (!isFilled) {

			if (special1.fillAmount < 1f)
				special1.fillAmount += fill;

			if (special1.fillAmount == 1f) {
		
				special1.color = new Color32 (0, 255, 255, 255);
				isFilled = true;

				if (player.name.StartsWith ("PlayerShooter"))
					player.GetComponent <PlayerBehaviour> ().SpecialEnable (true);
				else if (player.name.StartsWith ("PlayerAdventurer"))
					player.GetComponent <Player2Behaviour> ().SpecialEnable (true);
				
				FindObjectOfType<AudioManager> ().PlayAudio ("Special Charge");
				FindObjectOfType<OtherStatus> ().EnableStatus (1);

			}

		}

	}

	public void UnfillSpecial1 () {

		if (special1.fillAmount > 0f)
			special1.fillAmount -= 0.01f;
		else {
			special1.color = new Color32 (0, 255, 255, 120);
			isFilled = false;

			FindObjectOfType<Weapons> ().SetWeapon (1);

			if (player.name.StartsWith ("PlayerShooter"))
				player.GetComponent <PlayerBehaviour> ().SpecialEnable (false);
			else if (player.name.StartsWith ("PlayerAdventurer"))
				player.GetComponent <Player2Behaviour> ().SpecialEnable (false);
			
			CancelInvoke ();
		}

	}

	public void InvokeUnfill() {

		InvokeRepeating ("UnfillSpecial1", Time.deltaTime, 0.2f);		//20 segundos

	}

}
