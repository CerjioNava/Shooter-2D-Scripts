using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour {

	public GameObject EnergyBallParent;
	private Rigidbody2D rb;
	private Animator anim;

	void Awake () {

		anim = GetComponent <Animator> ();		
		rb = GetComponent <Rigidbody2D> ();		
		Invoke ("Impact", 3f);

		rb.velocity = transform.right*3f;

	}

	void Impact () {

		anim.SetBool ("Impact", true);
		rb.velocity = new Vector2 (0f, 0f);
		Destroy (EnergyBallParent, 0.59f);

	}

}
