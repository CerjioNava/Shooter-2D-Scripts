using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	public GameObject LvlComplete, DeathScene, BossScene, Player, Portal;
	public GameObject BossBar;

	public void Awake() {
		Player = FindObjectOfType<LookAt> ().GetTarget ();
	}

	public void Death () {

		Cursor.visible = true;
		DeathScene.SetActive (true);

	}

	public void LevelComplete () {

		Cursor.visible = true;
		LvlComplete.SetActive (true);
		BossBar.SetActive (false);

	}

	public void BossTime () {

		BossScene.SetActive (true);

		if (SceneManager.GetActiveScene ().buildIndex == 1)
			Invoke("DisableBossScene", 2.5f);
		else if (SceneManager.GetActiveScene ().buildIndex == 2)
			Invoke("DisableBossSceneNotSurvival", 2.5f);

	}

	public void DisableBossScene () {
		
		BossScene.SetActive (false);
		BossBar.SetActive (true);
		FindObjectOfType<MonsterRespawn> ().BossRespawn ();
		FindObjectOfType<AudioManager> ().PlayAudio ("Respawn");

		FindObjectOfType<LookAt> ().SetTarget (FindObjectOfType<MonsterRespawn> ().bossPoint.gameObject);
		FindObjectOfType<StopTargets> ().ScriptsState (false);
		FindObjectOfType<StopTargets> ().Invoke ("EnableScripts", 2f);

	}

	public void DisableBossSceneNotSurvival () {

		BossScene.SetActive (false);
		BossBar.SetActive (true);

		FindObjectOfType<BossHealthBar> ().enabled = true;
		Portal.SetActive (true);

	}


	//BUTTONS ------------------------------------------------------------------------------------------------------

	public void LoadMenu () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
		FindObjectOfType<AudioManager> ().SetIndex (0);
		FindObjectOfType<AudioManager> ().StageTheme ();
		SceneManager.LoadScene (0);

	}

	public void NextLevel () {
	
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");

		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			FindObjectOfType<AudioManager> ().SetIndex (SceneManager.GetActiveScene ().buildIndex + 1);
			FindObjectOfType<AudioManager> ().StageTheme ();
		}

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			
	}

	public void RestartLevel() {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");

		if (SceneManager.GetActiveScene ().buildIndex == 1)
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		else if (SceneManager.GetActiveScene ().buildIndex == 2) {

			if (Checkpoint.check != null) {
			
				DeathScene.SetActive (false);
				Checkpoint.check.RespawnPlayer ();
				Cursor.visible = false;

			} else 
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);

		}

	}
	/*
	public void SetCheckpoint (GameObject checkpoint) {
		Checkpoint = checkpoint;
	}
	*/
}
