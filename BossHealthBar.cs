using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {

	public Image fill, staminaFill;

	public void SetBossHealth (float Health, float maxHealth) {

		fill.fillAmount = Health/maxHealth;

	}

	public void SetBossStamina (float stamina, float maxStamina) {

		staminaFill.fillAmount = stamina/maxStamina;

	}

}
