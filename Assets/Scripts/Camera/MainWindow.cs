using UnityEngine;
using System.Collections;

public class MainWindow : GenericWindow {
	public GameObject ingameMenu;

	public void Build(){
		manager.Open (2);
	}

	public void MyCity(){
		manager.Open (4);
	}
}
