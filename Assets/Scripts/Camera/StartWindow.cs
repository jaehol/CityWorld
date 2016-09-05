using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartWindow : GenericWindow {
	public AudioSource sound;

	/*public Button continueButton;

	public override void Open ()
	{
		var canContitnue = true;

		continueButton.gameObject.SetActive (canContitnue);

		if (continueButton.gameObject.activeSelf) {
			firstSelected = continueButton.gameObject;
		}

		base.Open ();
	}*/

	public void NewGame(){
		manager.Open (1);
	}
}
