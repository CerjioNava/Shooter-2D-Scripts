using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLaser : MonoBehaviour {

	public GameObject LaserParent;

	void Awake () {
		
		Destroy (LaserParent, 0.5f);

	}

}
