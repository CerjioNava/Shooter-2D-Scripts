using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Selection : MonoBehaviour {

	public GameObject characterSelect, ShooterPrefab, AdventurerPrefab;
	public int characterN;
	public static Selection instance;
	public bool SaveFile = false, Continue = false;

	void Awake () {
		
		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		if (File.Exists (Application.persistentDataPath + "/playerStats.Data")) {
			SaveFile = true;
		}

	}

	public void SetCharacter (int characterNumber) {

		characterN = characterNumber;

		if (characterNumber == 1) 
			characterSelect = ShooterPrefab;
		else if (characterNumber == 2) 
			characterSelect = AdventurerPrefab;
		
	}

	public GameObject GetCharacter () { 
	
		return characterSelect;
	
	}


}
