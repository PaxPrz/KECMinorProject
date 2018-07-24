using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookScript : MonoBehaviour {

	public Text friendListText;

	void Awake(){
		if (!FB.IsInitialized) {
			FB.Init (() => {
				if (FB.IsInitialized)
					FB.ActivateApp ();
				else
					Debug.LogError ("Couldn't initialize fb :o ");
			}, 
				isGameShown => {
					if (!isGameShown)
						Time.timeScale = 0;
					else
						Time.timeScale = 1;
				});
		} else {
			FB.ActivateApp ();
		}
	}

	#region Login/Logout

	public void FacebookLogin(){
		var permissions = new List<string> (){ "public_profile", "email", "user_friends" };
		FB.LogInWithReadPermissions (permissions);
	}

	public void FacebookLogout(){
		FB.LogOut ();
	}
	#endregion

	public void FacebookShare(){
		
	}

	#region Inviting

	public void FacebookGameRequest(){
		FB.AppRequest ("Come and try this new game from KEC, Nepal ", title: "Asteroid Smash!");
	}

	public void FacebookInvite(){
		FB.Mobile.AppInvite (new System.Uri (" "));
	}

	#endregion

	public void GetFriendsPlayingThisGame(){
		string query = "/me/friends";
		FB.API (query, HttpMethod.GET, result => {
			var dictionary = (Dictionary<string,object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
			var friendList = (List<object>)dictionary["data"];
			friendListText.text = string.Empty;
			foreach(var dict in friendList){
				friendListText.text += ((Dictionary<string, object>)dict)["name"];
			}
		});
	}

}
