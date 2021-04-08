using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour {

	public GameObject ShotgunParent;

	void Awake () {

		Destroy (ShotgunParent, 0.62f);

	}
		
	void OnTriggerEnter2D () {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Enemy")) )
			FindObjectOfType<SpecialFill> ().FillSpecial1 (0.01f);
		else if (GetComponent <CircleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Enemy")) )
			FindObjectOfType<SpecialFill> ().FillSpecial1 (0.01f);

	}



}
