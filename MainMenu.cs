using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public static bool GameIsPause = false;
	public GameObject continueSaved, newgame;
	public Text pressEscape, difficulty;
	public GameObject MenuUI, MenuStart, WorldSelect, CharacterSelect, Instructions;		//Menus principales
	public int levelNumber, characterNumber, weaponNumber;
	public GameObject[] ShooterProperties, AdventurerProperties;


	void Awake () {
		MenuStart.SetActive (true);
		Cursor.visible = true;
	}


	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (GameIsPause && MenuUI.activeInHierarchy)
				Resume ();
			else if (!GameIsPause)
				Pause ();

			if (GameIsPause && WorldSelect.activeInHierarchy) 
				BackTo ("Main Menu");
			else if (GameIsPause && CharacterSelect.activeInHierarchy && !Instructions.activeInHierarchy) 
				BackTo ("Stage Selection"); 
			else if (GameIsPause && Instructions.activeInHierarchy) {
				Controls (false);
			}

		}

	}

	//MENU BOTONES---------------------------------------------------------------------------------------------------------------------

	//Resumir
	public void Resume () {

		pressEscape.enabled = true;
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
		MenuUI.SetActive (false);
		GameIsPause = false;

	}

	//Pausa de menu
	public void Pause () {

		pressEscape.enabled = false;
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");
		MenuUI.SetActive (true);
		GameIsPause = true;

	}

	//Salir del juego
	public void QuitGame () {

		Application.Quit ();

	}

	//Quitar la musica
	public void SoundOff () {

		FindObjectOfType<AudioManager> ().PauseAudio ();

	}

	//WORLD SELECT---------------------------------------------------------------------------------------------------------------------

	//Entrada a la selección de mundo
	public void WorldSelection () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");
		EnableAndDisable (WorldSelect, MenuUI);

	}

	//Selección de mundo
	public void WorldNumber (int level) {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");
		levelNumber = level;

		newgame.SetActive (true);

		if (levelNumber == 2 && Selection.instance.SaveFile) 
			continueSaved.SetActive (true);
		else
			continueSaved.SetActive (false);

	}

	public void NewGame () {
		EnableAndDisable (CharacterSelect, WorldSelect);
		Selection.instance.Continue = false;
	}

	public void ContinueGame () {

		PlayerData player = SaveSystem.LoadPlayer ();

		if (player == null)
			return;

		characterNumber = player.playerNumber;
		Selection.instance.SetCharacter (characterNumber);
		Selection.instance.Continue = true;
		PlayLevel ();

	}

	//CHARACTER SELECT---------------------------------------------------------------------------------------------------------------------

	//Selección de personaje
	public void CharacterSelection (int characterNumber) {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");

		if (characterNumber == 1)							//Shooter
			EnableCharacter (ShooterProperties, AdventurerProperties);
		else if (characterNumber == 2) 						//Adventurer
			EnableCharacter (AdventurerProperties, ShooterProperties);

		Selection.instance.SetCharacter (characterNumber);

		this.characterNumber = characterNumber;
		difficulty.enabled = true;

	}

	//Habilita un personaje y deshabilita el anterior seleccionado
	void EnableCharacter (GameObject[] characterEnable, GameObject[] characterDisable) {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");

		characterEnable [0].SetActive (true);
		characterEnable [1].SetActive (true);
		characterEnable [2].SetActive (true);
		characterEnable [6].SetActive (true);

		if (characterDisable [0].activeInHierarchy) {

			foreach (GameObject obj in characterDisable)
				obj.SetActive (false);

		}

	}

	//Descripción de armas
	public void WeaponDescription (int weaponNumber) {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");

		if (characterNumber == 1) 							//Shooter
			EnableWeapon (ShooterProperties, weaponNumber);
		else if (characterNumber == 2) 						//Adventurer
			EnableWeapon (AdventurerProperties, weaponNumber);
		
		this.weaponNumber = weaponNumber;

	}

	//Habilita la descripción del arma y deshabilita la anterior seleccionada
	void EnableWeapon (GameObject[] weaponEnable, int weaponNumber) {

		weaponEnable [3].SetActive (false);
		weaponEnable [4].SetActive (false);
		weaponEnable [5].SetActive (false);

		weaponEnable [weaponNumber + 2].SetActive (true);

	}

	//Controles

	public void Controls (bool show) {

		if (show) {
			FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");
			Time.timeScale = 0f;
		} else {
			FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");
			Time.timeScale = 1f;
		}

		Instructions.SetActive (show);

	}

	//Jugar el nivel
	public void PlayLevel () {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");

		if (characterNumber != 0) {

			GameIsPause = false;
			FindObjectOfType<AudioManager> ().SetIndex (levelNumber);
			FindObjectOfType<AudioManager> ().StageTheme ();
			SceneManager.LoadScene (levelNumber);

		}
	}

	//GENERAL---------------------------------------------------------------------------------------------------------------------

	//Volver al menu
	public void BackTo (string name) {

		FindObjectOfType<AudioManager> ().PlayAudio ("Menu Out");

		if (name == "Character Selection")
			Instructions.SetActive (false);
		else if (name == "Stage Selection")
			EnableAndDisable (WorldSelect, CharacterSelect);
		else if (name == "Main Menu")
			EnableAndDisable (MenuUI, WorldSelect);
		else if (name == "Main Screen")
			EnableAndDisable (MenuStart, MenuUI);

	}

	//Habilita un objeto y deshabilita otro
	void EnableAndDisable (GameObject obj1, GameObject obj2){

		obj1.SetActive (true);
		obj2.SetActive (false);

	}

}
