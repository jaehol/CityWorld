using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook.Unity;

public class FBmanager : MonoBehaviour {
	private static FBmanager _instance;

	public static FBmanager Instance
	{
		get {
			if (_instance == null) {
				GameObject fbm = new GameObject ("FBManager");
				fbm.AddComponent<FBmanager> ();
			}
			return _instance;
		}
	}

	public bool IsLoggedIn{ get; set; }
	public string ProfileName { get; set; }
	public Sprite ProfilePic { get; set; }

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		_instance = this;
		IsLoggedIn = true;
	}

	public void InitFB()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
			IsLoggedIn = FB.IsLoggedIn;
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
		IsLoggedIn = FB.IsLoggedIn;

		if (FB.IsLoggedIn) {
			Debug.Log ("Facebook is logged in.");
			GetProfile ();
		} else {
			Debug.Log ("Facebook is not logged in.");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void GetProfile()
	{
		FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
		FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
	}

	void DisplayUsername(IResult result)
	{
		if (result.Error == null) {
			ProfileName = "" + result.ResultDictionary ["first_name"];
		} else {
			Debug.Log (result.Error);
		}
	}

	void DisplayProfilePic(IGraphResult result)
	{
		if (result.Texture != null) {
			ProfilePic = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
		}
	}

	public void Share()
	{
		FB.ShareLink(
			new Uri("https://developers.facebook.com/"),
			callback: ShareCallback
		);
	}

	private void ShareCallback (IShareResult result) {
		if (result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
			Debug.Log("ShareLink Error: "+result.Error);
		} else if (!String.IsNullOrEmpty(result.PostId)) {
			// Print post identifier of the shared content
			Debug.Log(result.PostId);
		} else {
			// Share succeeded without postID
			Debug.Log("ShareLink success!");
		}
	}

	public void QueryScore()
	{
		FB.API ("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, ScoresCallback);
	}

	private void ScoresCallback(IResult result)
	{
		Debug.Log("Scores callback: " + result);
	}
}
