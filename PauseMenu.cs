using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPause = false;

	public GameObject pauseMenuUI, SaveMenuUI;

	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (GameIsPause)
				Resume ();
			else
				Pause ();
			
		}

	}

	public void Resume () {

		Cursor.visible = false;
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPause = false;

	}


	public void Pause () {

		Cursor.visible = true;
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPause = true;

	}

	public void LoadMenu () {
		
		GameIsPause = false;
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
		FindObjectOfType<AudioManager> ().SetIndex (0);
		FindObjectOfType<AudioManager> ().StageTheme ();
		Time.timeScale = 1f;
		SceneManager.LoadScene (0);

	}


	public void QuitGame () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
		Application.Quit ();

	}


	public void SoundOff () {
		
		FindObjectOfType<AudioManager> ().PauseAudio ();
		
	}


	//SAVE MENU
	public void SavePlayerStat () {

		SaveSystem.SavePlayer (PlayerStats.player);
		Time.timeScale = 1f;

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
		FindObjectOfType<OtherStatus> ().EnableStatus (10);
		SaveMenuUI.SetActive (false);

		Selection.instance.SaveFile = true;

	}

	public void DontSavePlayerStat() {

		Time.timeScale = 1f;
		SaveMenuUI.SetActive (false);

	}


}