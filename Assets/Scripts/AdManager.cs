using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour {

	public static AdManager Instance{ get; set;}

	public string interstitialAdID = "ca-app-pub-4914476274989016/2656498835";
	private string userID = "ca-app-pub-4914476274989016~1702100692";
	private string rewardAdID = "ca-app-pub-4914476274989016/5062336171";
	public bool adShown = false;

	public Slider loadMeter;

	InterstitialAd myInterstitialAd;
	RewardBasedVideoAd myRewardBasedVideoAd;

	// Use this for initialization
	void Start () {
		
		Instance = this;
		DontDestroyOnLoad (gameObject);
		StartCoroutine (loadAsynchronously ());
		//#if UNITY_EDITOR
		Debug.Log("Cant load ad in editor");
		//#elif UNITY_ANDROID
		MobileAds.Initialize(userID);
		myInterstitialAd = new InterstitialAd(interstitialAdID);
		myRewardBasedVideoAd = RewardBasedVideoAd.Instance;
		myRewardBasedVideoAd.SetUserId (userID);
		//#endif
	}

	private IEnumerator loadAsynchronously(){
		AsyncOperation operation = SceneManager.LoadSceneAsync ("UI");
		while (!operation.isDone) {
			float progress = Mathf.Clamp01 (operation.progress / 0.9f);
			loadMeter.value = progress;
			Debug.Log (operation.progress);
			yield return null;
		}
	}

	void loadInterstitialAd(){
		AdRequest request = new AdRequest.Builder ().Build ();
		myInterstitialAd.LoadAd (request);
	}

	void loadRewardBasedVideoAd(){
		if (!myRewardBasedVideoAd.IsLoaded ()) {
			AdRequest request = new AdRequest.Builder ().Build ();
			myRewardBasedVideoAd.LoadAd (request, rewardAdID);
		}
	}

	public void showInterstitialAd(){
		//#if UNITY_EDITOR
		Debug.Log("Interstitial Ad show");
		//#elif UNITY_ANDROID
		if (myInterstitialAd.IsLoaded ()) {
			myInterstitialAd.Show ();
		} else {
			loadInterstitialAd ();
		}
		//#endif
	}

	public void ShowVideoAd(){
		//#if UNITY_EDITOR
		Debug.Log("Cannot play add in editor");
		//#elif UNITY_ANDROID
		if (myRewardBasedVideoAd.IsLoaded ()) {
			myRewardBasedVideoAd.Show ();
			adShown = true;
		} else {
			loadRewardBasedVideoAd ();
		}
		//#endif
	}

	public void ShowAllAd(){
		ShowVideoAd ();
		showInterstitialAd ();
	}
}
