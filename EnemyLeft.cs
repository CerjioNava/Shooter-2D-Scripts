using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyLeft : MonoBehaviour {

	public int InitialEnemies;
	private int EnemiesLeft;
	public Text conteoEnemy;

	void Awake () {

		EnemiesLeft = FindObjectOfType <MonsterRespawn>().GetMaxRespawn () + InitialEnemies;
		conteoEnemy.text = "Enemies Left: " + EnemiesLeft.ToString ();

	}

	public void EnemyDeath () {

		EnemiesLeft -= 1;

		if (EnemiesLeft >= 0) 
			conteoEnemy.text = "Enemies Left: " + EnemiesLeft.ToString ();

		if (EnemiesLeft == 0) {
			FindObjectOfType <EndGame> ().BossTime ();
			Invoke ("DisableText", 2.5f);
		}

	}

	public void CompleteLvl () {

		FindObjectOfType <EndGame> ().LevelComplete ();

	}

	void DisableText () {

		conteoEnemy.enabled = false;

	}

}
