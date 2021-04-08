using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeat : MonoBehaviour {

	private GameObject[] MonsterLeft;

	public void BossIsDeath () {

		FindObjectOfType <AudioManager> ().PlayAudio ("Boss Death");
		FindObjectOfType <EndGame> ().LevelComplete();
		MonsterLeft = GameObject.FindGameObjectsWithTag ("Enemy");;

		foreach (GameObject monster in MonsterLeft) {

			if (monster.name.StartsWith ("Blue Flame"))
				monster.GetComponent <BlueFlameBehaviour> ().Invoke ("BFDeath", Random.Range (0.5f, 2f));
			else if (monster.name.StartsWith ("Cacodemon"))
				monster.GetComponent <CacoBehaviour> ().Invoke ("CacoDeath", Random.Range (0.5f, 2f));
			
		}

	}

}
