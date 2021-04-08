using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePortal : MonoBehaviour {

	public GameObject Boss, Portal;	
	public bool isBoss;

	void Update () {

		if (Boss != null) {

			if (isBoss && Boss.GetComponent<EnemyStats> ().GetDeath ()) {
			
				Invoke ("Enable", 1.5f); 
			
			}

		}

	}

	public void SetBoss (GameObject boss) {
		Boss = boss;
	}

	void Enable () {
		Portal.SetActive (true);
	}

}
