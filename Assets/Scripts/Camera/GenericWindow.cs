using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour {

	public static WindowManager manager;

	public GameObject firstSelected;

	protected EventSystem eventSystem{
		get {
			return GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		}
	}

	public virtual void OnFocus(){
		eventSystem.SetSelectedGameObject (firstSelected);
	}

	protected virtual void Display(bool value){
		gameObject.SetActive (value);
	}

	public virtual void Open2(){
		Display (true);
		OnFocus ();
	}

	public virtual void Close(){
		Display (false);
	}
		
	protected virtual void Awake () {
		Close ();
	}
}
