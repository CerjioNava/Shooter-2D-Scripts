using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {

	public GameObject[] instruction;

	public void Instruction (int number) {

		foreach (GameObject ins in instruction) {

			if (ins.activeSelf) 
				ins.SetActive (false);

		}

		instruction [0].SetActive (false);
		instruction [number].SetActive (true);
		FindObjectOfType<AudioManager> ().PlayAudio ("Menu In");

	}

}
