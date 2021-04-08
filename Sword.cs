using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	void OnTriggerEnter2D () {

		if (GetComponent <CapsuleCollider2D> ().IsTouchingLayers (LayerMask.GetMask ("Enemy")) )
			FindObjectOfType<SpecialFill> ().FillSpecial1 (0.05f);

	}

}
