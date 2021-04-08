using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	
	public GameObject player, YellowKey, BlueKey, RedKey, SaveMenu;
	private Animator anim;
	private PlayerData playerData;
	private PlayerStats playerStats;
	private bool isRespawning = false;
	public static Checkpoint check;

	void Awake () {

		anim = GetComponent <Animator> ();
		player = FindObjectOfType<LookAt> ().GetTarget ();
		playerStats = player.GetComponent <PlayerStats> ();

	}

	void OnTriggerEnter2D (Collider2D collision) {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Player")) && !isRespawning) {

			if (player == null)
				player = collision.gameObject;

			playerStats = collision.GetComponent <PlayerStats> ();

			if (playerStats == null)
				return;
			
			EnableSaveWindow ();
			Time.timeScale = 0f;

		}

	}

	void OnTriggerExit2D () {

		if (!IsInvoking ("StopSave"))
			Invoke("StopSave", 1f);

	}


	void EnableSaveWindow () {

		check = this;

		anim.SetBool ("Save", true);
		//FindObjectOfType<AudioManager> ().PlayAudio ("Healing");

		SaveMenu.SetActive (true);

	}

	void StopSave () {

		anim.SetBool ("Save", false);
		isRespawning = false;
			
	}


	public void RespawnPlayer () {
		
		isRespawning = true;

		playerData = SaveSystem.LoadPlayer ();

		player.transform.position = new Vector3 (playerData.playerPosition[0], playerData.playerPosition[1], playerData.playerPosition[2]);
		player.SetActive (true);

		playerStats.UpdateHealth (playerData.playerHealth);

		playerStats.GetKey ("Yellow Key", playerData.yellowKey);
		playerStats.GetKey ("Blue Key", playerData.blueKey);
		playerStats.GetKey ("Red Key", playerData.redKey);

		if (!playerData.yellowKey && !YellowKey.activeSelf) {		

			YellowKey.SetActive (true);
			YellowKey.GetComponent <Animator> ().SetBool ("Taken", false);

		}

		if (!playerData.blueKey && !BlueKey.activeSelf) {
		
			BlueKey.SetActive (true);
			BlueKey.GetComponent <Animator> ().SetBool ("Taken", false);

		}

		if (!playerData.redKey && !RedKey.activeSelf) {
		
			RedKey.SetActive (true);
			RedKey.GetComponent <Animator> ().SetBool ("Taken", false);

		}

	}

}
