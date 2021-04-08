using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour {

	public GameObject player;
	private Selection s;

	void Awake() {

		s = Selection.instance;
		player = s.characterSelect;

		if (player == null)
			return;

		GameObject newPlayer = Instantiate (player, transform.position, transform.rotation);
		FindObjectOfType<LookAt> ().SetTarget (newPlayer);

	}

	void Start() {

		if (s.Continue) {
			FindObjectOfType<Checkpoint> ().RespawnPlayer ();
		}

	}

}
