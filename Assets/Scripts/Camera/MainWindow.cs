using UnityEngine;
using System.Collections;

public class MainWindow : GenericWindow {
	public GameObject ingameMenu;

	public void Drive(){
		manager.Open (2);
	}

	public void MyCar(){
		manager.Open (4);
	}

	public void onPause(){
		Time.timeScale = 0;
		ingameMenu.SetActive (true);
	}

	public void onResume(){
		Time.timeScale = 1f;
		ingameMenu.SetActive (false);
	}
}
