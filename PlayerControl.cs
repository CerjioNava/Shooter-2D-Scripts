using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private float horizontalMov, verticalMov, horizontalAttack, verticalAttack;
	private bool attack;
	private bool normalWeapon, specialWeapon, chargedWeapon;
	public bl_Joystick [] touchController;
	public bl_Joystick moveController, attackController;


	void Start () {
		
		touchController = FindObjectsOfType <bl_Joystick>();

		if (touchController [0].name.StartsWith ("MJoystick")) {

			moveController = touchController [0].GetComponent <bl_Joystick> ();
			attackController = touchController [1].GetComponent <bl_Joystick> ();

		} else if (touchController [0].name.StartsWith ("AJoystick")) {

			moveController = touchController[1].GetComponent <bl_Joystick> ();
			attackController = touchController[0].GetComponent <bl_Joystick> ();

		}

	}

	void Update () {
	
		horizontalMov = moveController.Horizontal;
		verticalMov = moveController.Vertical;

		horizontalAttack = attackController.Horizontal;
		verticalAttack = attackController.Vertical;

	}

	void FixedUpdate() {
		 
		attack = attackController.IsTouching ();

	}

	public bool Action (string action) {

		if (action == "Attack")
			return attack;
						
		else if (action == "Normal Weapon")
			return normalWeapon;
		
		else if (action == "Special Weapon")
			return specialWeapon;
		
		else if (action == "Charged Weapon")
			return chargedWeapon;
		
		else
			return false;

	}

	public Vector2 AttackDirection() {

		return new Vector2 (horizontalAttack, verticalAttack);

	}

	public Vector2 MoveDirection() {

		return new Vector2 (horizontalMov, verticalMov);

	}


	public void SetWeapon (int weapon) {

		normalWeapon = false;
		specialWeapon = false;
		chargedWeapon = false;

		if (weapon == 1) 
			normalWeapon = true;
		else if (weapon == 2) 
			specialWeapon = true;
		else if (weapon == 3) 
			chargedWeapon = true;
		
	}

}
