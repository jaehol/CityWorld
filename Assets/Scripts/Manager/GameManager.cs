using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Current = null;

	public List<PlayerSetupDefinition> Players = new List<PlayerSetupDefinition>();

	// Use this for initialization
	void Start () {
		Current = this;
	}

	// Update is called once per frame
	void Update () {

	}
}
