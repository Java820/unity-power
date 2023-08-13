using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;


public class GoogleAdsManager : MonoBehaviour
{
    public InterstitialAd interstitialAd;

    public RewardedAd rewardedGoldAd;
    public RewardedAd rewarded20Ad;

    PlayfabManager playfabManager;

    void Awake()
    {
        playfabManager = this.gameObject.GetComponent<PlayfabManager>();
    }
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

    public void RequestInterstitial()
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-6360445073618726/3650420378";
        #endif
        #if UNITY_IPHONE
                    string adUnitId = "ca-app-pub-6360445073618726/7053939760";
        #endif
        //DE PRUEBA
        //string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";

        this.interstitialAd = new InterstitialAd(adUnitId);

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialAd.LoadAd(request);
    }
    public void ShowInterstitial()
    {
        interstitialAd.Show();
    }

    public void RequestGoldRewarded()
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-6360445073618726/2713561465";
        #endif
        #if UNITY_IPHONE
                string adUnitId = "ca-app-pub-6360445073618726/1079558359";
        #endif
        this.rewardedGoldAd = new RewardedAd(adUnitId);

        this.rewardedGoldAd.OnAdFailedToLoad += HandleGoldRewardedAdFailedToLoad;
        this.rewardedGoldAd.OnUserEarnedReward += HandleEarnedGoldReward;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedGoldAd.LoadAd(request);
    }
    public void HandleGoldRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.RequestGoldRewarded();
    }
    public void HandleEarnedGoldReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        Debug.Log("Reward de Oro Ganado");
        playfabManager.AddVC("GO", 1);
        playfabManager.UseVC("AD", 1);


    }
    public void ShowGoldRewardedAd()
    {
        rewardedGoldAd.Show();
    }


}
