using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData{

	public float playerHealth;
	public bool yellowKey, blueKey, redKey;
	public float [] playerPosition;
	public int playerNumber;


	public PlayerData (PlayerStats player) {

		playerHealth = player.playerHealth;
		playerNumber = Selection.instance.characterN;

		yellowKey = player.yellowKey;
		blueKey = player.blueKey;
		redKey = player.redKey;

		playerPosition = new float[3];
		playerPosition[0] = player.transform.position.x;
		playerPosition[1] = player.transform.position.y;
		playerPosition[2] = player.transform.position.z;


	}


}
