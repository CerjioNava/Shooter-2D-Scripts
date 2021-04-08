using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour {

	public float effectTime;		//Tiempo para borrar el efecto de impacto

	//Método llamado al iniciar el efecto
	void Awake () {
	
		Destroy (transform.gameObject, effectTime); 	//Destruye el objeto de impacto

	}

}
