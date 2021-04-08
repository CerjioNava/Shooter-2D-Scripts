using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
	
	public float playerHealth, maxHealth, shootTime, moveSpeed;
	private float takeDamage;

	public bool yellowKey, blueKey, redKey;
	private bool keyReturn;

	public static PlayerStats player;

	void Awake () {

		if (player == null)
			player = this;

		if (SceneManager.GetActiveScene ().buildIndex == 3) {

			FindObjectOfType<LookAt> ().SetTarget (gameObject);
			FindObjectOfType<Weapons> ().Awake ();
			FindObjectOfType<SpecialFill> ().Awake ();
	
		}

	}
	
	void Update () {
		
	}


	//---------------------------------------------------------------------------------------------------------------------

	//Obtiene una llave
	public void GetKey (string keyColor, bool state) {

		if (state) {

			if (keyColor == "Yellow Key")
				yellowKey = true;
			else if (keyColor == "Blue Key")
				blueKey = true;
			else if (keyColor == "Red Key")
				redKey = true;

			FindObjectOfType<Keys> ().EnableKey (keyColor);

		} else {

			if (keyColor == "Yellow Key")
				yellowKey = false;
			else if (keyColor == "Blue Key")
				blueKey = false;
			else if (keyColor == "Red Key")
				redKey = false;

			FindObjectOfType<Keys> ().DisableKey (keyColor);

		}

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Devuelve estado de la llave seleccionada (PARA PUERTAS)
	public bool ReturnKey (string keyColor) {

		if (keyColor == "Yellow")
			keyReturn = yellowKey;
		else if (keyColor == "Blue")
			keyReturn = blueKey;
		else if (keyColor == "Red")
			keyReturn = redKey;

		return keyReturn;

	}

	//---------------------------------------------------------------------------------------------------------------------

	//Pierde vida
	public void LoseHealth (float health) {
		playerHealth = health;
		FindObjectOfType <HealthBar> ().LoseHealthInBar (playerHealth, maxHealth);
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Otorga vida al player
	public void UpdateHealth ( float gainHealth ) {

		playerHealth += gainHealth;

		if (playerHealth > maxHealth)
			playerHealth = maxHealth;

		if (gameObject.name.StartsWith ("PlayerShooter"))
			GetComponentInParent <PlayerBehaviour> ().SetHealth (playerHealth);
		else if (gameObject.name.StartsWith ("PlayerAdventurer"))
			GetComponentInParent <Player2Behaviour> ().SetHealth (playerHealth);

		FindObjectOfType <HealthBar> ().GainHealthInBar (playerHealth, maxHealth);
		
	}


}
