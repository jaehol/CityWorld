using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBscript : MonoBehaviour {

	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public GameObject DialogUsername;
	public GameObject DialogProfilePic;
	
	void Awake ()
	{
		FBmanager.Instance.InitFB ();
		DealWithFBMenus (FB.IsLoggedIn);
	}
		
	public void FBlogin()
	{
		var perms = new List<string>(){"public_profile", "email", "user_friends"};
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	private void AuthCallback (ILoginResult result) 
	{
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
			FBmanager.Instance.IsLoggedIn = true;
			FBmanager.Instance.GetProfile();
		} else {
			Debug.Log("User cancelled login");
		}
		DealWithFBMenus (FB.IsLoggedIn);
	}

	void DealWithFBMenus(bool isLoggedIn)
	{
		if (isLoggedIn) {
			DialogLoggedIn.SetActive (true);
			DialogLoggedOut.SetActive (false);

			if (FBmanager.Instance.ProfileName != null) {
				Text UserName = DialogUsername.GetComponent<Text> ();
				UserName.text = FBmanager.Instance.ProfileName + ", welcome to city!";
			} else {
				StartCoroutine ("WaitForProfileName");
			}

			if (FBmanager.Instance.ProfilePic != null) {
				Image ProfilePic = DialogProfilePic.GetComponent<Image> ();
				ProfilePic.sprite = FBmanager.Instance.ProfilePic;
			} else {
				StartCoroutine ("WaitForProfilePic");
			}
		} else {
			DialogLoggedIn.SetActive (false);
			DialogLoggedOut.SetActive (true);
		}
	}

	IEnumerator WaitForProfileName()
	{
		while (FBmanager.Instance.ProfileName == null) {
			yield return null;
		}
		DealWithFBMenus (FB.IsLoggedIn);
	}

	IEnumerator WaitForProfilePic()
	{
		while (FBmanager.Instance.ProfilePic == null) {
			yield return null;
		}
		DealWithFBMenus (FB.IsLoggedIn);
	}

	public void Share()
	{
		FBmanager.Instance.Share ();
	}

	public void QueryScore()
	{
		FBmanager.Instance.QueryScore();
	}
}
