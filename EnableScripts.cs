using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableScripts : MonoBehaviour {

	public GameObject[] Objects;
	public bool BossRoom;
	private int total;

	void OnTriggerEnter2D () {

		if (GetComponent <BoxCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player"))) {

			if (BossRoom) {

				FindObjectOfType <EndGame> ().BossTime ();
				Invoke ("Enable", 5f);

			} else 
				Enable ();
			
		}

	}


	void Enable () {

		if (BossRoom) {
			FindObjectOfType<MinotaurBehaviour> ().GetComponent <MinotaurBehaviour> ().enabled = true;
			FindObjectOfType<MinotaurBehaviour> ().GetComponent <MinotaurBehaviour> ().Awake ();
			FindObjectOfType<EnablePortal> ().SetBoss (FindObjectOfType<MinotaurBehaviour> ().gameObject);
		}

		foreach (GameObject obj in Objects) {

			if (obj.name.StartsWith ("Minotaur"))
				obj.GetComponent <MinotaurBehaviour> ().enabled = true;
			else if (obj.name.StartsWith ("Crystal"))
				obj.GetComponent <CrystalBehaviour> ().enabled = true;
			else if (obj.name.StartsWith ("Blue Flame"))
				obj.GetComponent <BlueFlameBehaviour> ().enabled = true;
			else if (obj.name.StartsWith ("Cacodemon"))
				obj.GetComponent <CacoBehaviour> ().enabled = true;
			else if (obj.name.StartsWith ("Black Slime"))
				obj.GetComponent <BlackSlimeBehaviour> ().enabled = true;

			total++;

			if (total >= Objects.Length)
				Destroy (gameObject);

		}

	}

}
