using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt2 : MonoBehaviour {

	public Transform target;					//Objetivo a mirar

	void Awake () {

		transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, -1);	//Posicion inicial

	}

	public void SetTarget(GameObject objTarget) {

		target = objTarget.transform;

	}

	public GameObject GetTarget() {

		return target.gameObject;

	}

}
