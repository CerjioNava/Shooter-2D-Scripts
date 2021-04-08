using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRespawn : MonoBehaviour {

	public GameObject CacodemonPrefab, BlueFlamePrefab, BossPrefab, Player;
	public int maxRespawn = 11;
	public float respawnTime1, respawnTime2;
	public bool enableRespawn;
	public Transform respawnPoint1, respawnPoint2, bossPoint;
	private Vector3 position;

	void Awake () {

		Player = FindObjectOfType<LookAt> ().GetTarget ();

		if (Player.name.StartsWith ("PlayerAdventurer"))
			enableRespawn = false;

		if (enableRespawn) {

			InvokeRepeating ("RespawnBlueFlame", 10f, respawnTime1);
			InvokeRepeating ("RespawnCacodemon", 15f, respawnTime2);

		} else {
			maxRespawn = 0;
		}

	}
	
	void RespawnBlueFlame () {

		if (maxRespawn > 0) {
			maxRespawn -= 1;
			position = new Vector3 (respawnPoint1.position.x, 
									respawnPoint1.position.y * Random.Range (-1f, 1f), 
									respawnPoint1.position.z);
			Instantiate (BlueFlamePrefab, position, respawnPoint1.rotation);
			FindObjectOfType<AudioManager> ().PlayAudio ("Respawn");
		} else 
			CancelInvoke ();
	}

	void RespawnCacodemon () {

		if (maxRespawn > 0) {
			maxRespawn -= 1;
			position = new Vector3 (respawnPoint2.position.x, 
									respawnPoint2.position.y * Random.Range (-1f, 1f), 
									respawnPoint2.position.z);
			Instantiate (CacodemonPrefab, position, respawnPoint2.rotation);
			FindObjectOfType<AudioManager> ().PlayAudio ("Respawn");
		} else
			CancelInvoke ();

	}


	public void BossRespawn () {

		Instantiate (BossPrefab, bossPoint.position, bossPoint.rotation);
		FindObjectOfType<BossHealthBar> ().enabled = true;
		FindObjectOfType<EnemyLeft> ().enabled = false;

	}

	public int GetMaxRespawn () {

		if (!enableRespawn)
			maxRespawn = 0;

		return maxRespawn;

	}


}
