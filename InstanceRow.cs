using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceRow : MonoBehaviour {

	public GameObject objectInstance;
	public int row, number;
	public float distance, instanceSpeed;
	public bool repeat;

	private Vector3 instancePosition;

	void Awake () {

		number = 0;
		instancePosition = transform.position;
		InvokeRepeating ("Row", 1f, instanceSpeed);

	}
	
	void Row () {

		if (number < row) {
			
			Instantiate (objectInstance, instancePosition, transform.rotation);
			instancePosition.x += distance;
			number += 1;

		} else if (number >= row && repeat) {

			number = 0;
			instancePosition = transform.position;

		}

	}
}
