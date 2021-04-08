using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour {

	public GameObject YellowKey, BlueKey, RedKey;


	void Awake () {

		YellowKey.SetActive (false);
		BlueKey.SetActive (false);
		RedKey.SetActive (false);

	}

	public void EnableKey (string keyColor) {

		if (keyColor == "Yellow Key")
			YellowKey.SetActive (true);
		else if (keyColor == "Blue Key")
			BlueKey.SetActive (true);
		else if (keyColor == "Red Key")
			RedKey.SetActive (true);

	}

	public void DisableKey (string keyColor) {

		if (keyColor == "Yellow Key")
			YellowKey.SetActive (false);
		else if (keyColor == "Blue Key")
			BlueKey.SetActive (false);
		else if (keyColor == "Red Key")
			RedKey.SetActive (false);
		
	}

}
