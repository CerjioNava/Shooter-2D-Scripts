using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	public GameObject LaserParent;

	void Awake () {

		Destroy (LaserParent, 0.5f);

	}

	void OnTriggerEnter2D () {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Enemy")) )
			FindObjectOfType<SpecialFill> ().FillSpecial1 (0.01f);

	}

}
