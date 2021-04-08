using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {

	public GameObject player;
	public Transform selection, normalWeapon, specialWeapon, chargedWeapon;
	public GameObject[] ShooterIcons, AdventurerIcons;

	public void Awake () {

		player = FindObjectOfType<LookAt> ().GetTarget ();

		if (player == null)
			return;

		if (player.name.StartsWith ("PlayerShooter")) {

			foreach (GameObject obj in ShooterIcons) {
				obj.SetActive (true);
			}

		} else if (player.name.StartsWith ("PlayerAdventurer")) {

			foreach (GameObject obj in AdventurerIcons) {
				obj.SetActive (true);
			}

		}

		SetWeapon (1);

	}


	public void SetWeapon (int weapon) {

		if (weapon == 1) {
			
			selection.position = normalWeapon.position;
			FindObjectOfType<PlayerControl> ().SetWeapon (weapon);

		} else if (weapon == 2) {
			
			selection.position = specialWeapon.position;
			FindObjectOfType<PlayerControl> ().SetWeapon (weapon);

		} else if (weapon == 3 && FindObjectOfType<SpecialFill> ().isFilled) {
		
			selection.position = chargedWeapon.position;
			FindObjectOfType<PlayerControl> ().SetWeapon (weapon);

		} else if (weapon == 3 && !FindObjectOfType<SpecialFill> ().isFilled)
			FindObjectOfType<OtherStatus> ().EnableStatus (0);

//		FindObjectOfType<AudioManager> ().PlayAudio ("Healing");

	}

}
