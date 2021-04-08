using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTargets : MonoBehaviour {

	public GameObject[] targetObjects;
	public GameObject player;

	public void Awake () {
		player = FindObjectOfType<LookAt> ().GetTarget ();
	}

	public void ScriptsState (bool choice) {
		
		targetObjects = GameObject.FindGameObjectsWithTag ("Enemy");

		if (player.name.StartsWith ("PlayerShooter"))
			player.GetComponent <PlayerBehaviour> ().enabled = choice;
		else if (player.name.StartsWith ("PlayerAdventurer"))
			player.GetComponent <Player2Behaviour> ().enabled = choice;
		
		foreach (GameObject obj in targetObjects) {
			
			if (obj.name.StartsWith ("Cacodemon")) {

				if (!choice)
					obj.GetComponent <CacoBehaviour> ().CancelInvoke ();

				if (choice) {
					obj.GetComponent <CacoBehaviour> ().enabled = choice;
					obj.GetComponent <CacoBehaviour> ().Awake ();
				}

			} else if (obj.name.StartsWith ("Blue Flame")) 
				obj.GetComponent <BlueFlameBehaviour> ().enabled = choice;
			else if (obj.name.StartsWith ("Black Slime"))
				obj.GetComponent <BlackSlimeBehaviour> ().enabled = choice;
			else if (obj.name.StartsWith ("Minotaur"))
				obj.GetComponent <MinotaurBehaviour> ().enabled = choice;
			else if (obj.name.StartsWith ("Crystal"))
				obj.GetComponent <CrystalBehaviour> ().enabled = choice;

			if (!choice)
				obj.GetComponent <Rigidbody2D> ().velocity = new Vector2 (0f, 0f);


		}

	}

	public void EnableScripts() {
		
		FindObjectOfType<LookAt> ().SetTarget (player);
		ScriptsState (true);

	}


}
