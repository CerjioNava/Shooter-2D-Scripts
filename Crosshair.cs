using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

	public GameObject crosshair, player;
	private Vector2 target;
	public Transform firepoint;

	void Start() {

		player = FindObjectOfType<LookAt> ().GetTarget ();

		if (player.name.StartsWith ("PlayerShooter"))
			crosshair.SetActive (true);
		
		FollowMouse ();
		Cursor.visible = false;

	}

	void FixedUpdate () {

		FollowMouse ();

	}


	void FollowMouse () {

		if (FindObjectOfType<PlayerControl> ().AttackDirection () == new Vector2 (0f, 0f))
			crosshair.SetActive (false);
		else {

			crosshair.SetActive (true);
			firepoint = player.GetComponent <PlayerBehaviour> ().GetFirepoint ();
			target = new Vector2 (firepoint.position.x, firepoint.position.y) + FindObjectOfType<PlayerControl> ().AttackDirection ();
			crosshair.transform.position = new Vector2 (target.x, target.y);

		}

	}

}
