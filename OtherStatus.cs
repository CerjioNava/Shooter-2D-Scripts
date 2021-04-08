using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherStatus : MonoBehaviour {

	public GameObject[] Status;
	private int number;

	public void EnableStatus (int statusNumber ) {

		foreach (GameObject s in Status) {

			if (s.activeSelf) {
				s.SetActive (false);
				CancelInvoke ();
			}

		}

		Status [statusNumber].SetActive (true);
		number = statusNumber;
		Invoke ("DisableStatus", 2f);

	}

	public void DisableStatus () {

		Status [number].SetActive (false);

	}

}
