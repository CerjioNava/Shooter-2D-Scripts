using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour {

	public float attackDamage, knockback, knockbackResist, freezeTime;
	public bool freeze;

	//Devuelve el daño de la bala
	public float GetDamage () {

		return attackDamage;

	}

	//Devuelve el knockback
	public float GetKnockback () {

		return knockback;

	}

	//Devuelve la resistencia al knockback
	public float GetKnockbackResist() {

		return knockbackResist;

	}

	//Devuelve estado de parálisis
	public bool GetFreeze () {

		return freeze;

	}

	//Devuelve tiempo de parálisis
	public float GetFreezeTime() {
	
		return freezeTime;
	
	}


}
