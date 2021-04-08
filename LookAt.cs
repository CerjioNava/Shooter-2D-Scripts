/* CERJIO N°02
 * SERGIO NAVA EL PAPIRRUKI DEL 2D
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	public Transform target;					//Objetivo a mirar

	private float horizontal, vertical;			//Movimiento
			
	void Awake () {

		if (target == null)
			return;

		transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, -1);	//Posicion inicial

	}


	void Update () {
	
		if (target == null)
			return;

		horizontal = target.transform.position.x - transform.position.x;			//Distancia respecto al target
		vertical = target.transform.position.y - transform.position.y;				//Distancia respecto al target

	}


	void FixedUpdate() {

		transform.Translate (3*horizontal * Time.deltaTime, 4*vertical * Time.deltaTime, 0);	//Movimiento

	}

	public void SetTarget(GameObject objTarget) {

		target = objTarget.transform;

	}

	public GameObject GetTarget() {

		return target.gameObject;

	}


}
