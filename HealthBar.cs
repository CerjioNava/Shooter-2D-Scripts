using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Image fill, fillWhite;

	public void LoseHealthInBar (float Health, float maxHealth) {

		fill.fillAmount = Health/maxHealth;
		InvokeRepeating ("LoseFillWhite", 1.5f, Time.deltaTime);
		CancelInvoke ("GainFill");

	}

	void LoseFillWhite () {

		if (fillWhite.fillAmount >= fill.fillAmount)
			fillWhite.fillAmount -= 0.01f;
		else
			CancelInvoke ();

	}


	public void GainHealthInBar (float Health, float maxHealth) {

		fillWhite.fillAmount = Health/maxHealth;
		InvokeRepeating ("GainFill", 1.5f, Time.deltaTime);
		CancelInvoke ("LoseFillWhite");

	}

	void GainFill () {

		if (fill.fillAmount < fillWhite.fillAmount)
			fill.fillAmount += 0.01f;
		else
			CancelInvoke ();

	}


}
