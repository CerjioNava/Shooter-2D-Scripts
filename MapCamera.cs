using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {

	public Camera playerCamera, mapCamera;

	void Awake () {

		playerCamera.enabled = true;
		mapCamera.enabled = false;
		playerCamera.gameObject.GetComponent<Crosshair> ().enabled = true;

	}


	void Update () {
	
		CamChange ();

	}

	public void CamChange () {

		if (Input.GetKey (KeyCode.Tab)) {
			
			mapCamera.enabled = true;
			playerCamera.enabled = false;
			playerCamera.gameObject.GetComponent<Crosshair> ().enabled = false;
			Time.timeScale = 0f;

		} else if (Input.GetKeyUp (KeyCode.Tab)) {

			mapCamera.enabled = false;
			playerCamera.enabled = true;
			playerCamera.gameObject.GetComponent<Crosshair> ().enabled = true;
			Time.timeScale = 1f;

		}

	}

}
