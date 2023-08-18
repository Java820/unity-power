using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;


public class GoogleAdsManager : MonoBehaviour
{
    public InterstitialAd interstitialAd;

    public RewardedAd rewardedGoldAd;
    public RewardedAd rewarded50Ad;

    PlayfabManager playfabManager;
    GameManager gameManager;
    Cashier cashier;
    GameData gameData;


    void Awake()
    {
        playfabManager = this.gameObject.GetComponent<PlayfabManager>();
        gameManager = this.gameObject.GetComponent<GameManager>();
        gameData = this.gameObject.GetComponent<GameData>();
        cashier = GameObject.Find("Cashier").GetComponent<Cashier>();
    }
    void Start()
    {
        MobileAds.Initialize(initStatus => {

            RequestInterstitial();
            RequestGoldRewarded();
            Request50Rewarded();

        });


    }

    public void RequestInterstitial()
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-6360445073618726/9484714250";
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
        if (gameData.isUserPremium == false)
        {
            interstitialAd.Show();
            gameManager.adShowable = false;
            RequestInterstitial();
        }
    }

    public void RequestGoldRewarded()
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-6360445073618726/8547887169";
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
        RequestGoldRewarded();
    }

    public void Request50Rewarded()
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-6360445073618726/9073914045";
        #endif
        #if UNITY_IPHONE
                string adUnitId = "ca-app-pub-6360445073618726/1079558359";
        #endif
        this.rewarded50Ad = new RewardedAd(adUnitId);

        this.rewarded50Ad.OnAdFailedToLoad += Handle50RewardedAdFailedToLoad;
        this.rewarded50Ad.OnUserEarnedReward += HandleEarned50Reward;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewarded50Ad.LoadAd(request);
    }
    public void Handle50RewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.Request50Rewarded();
    }
    public void HandleEarned50Reward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        Debug.Log("Reward de 50% Ganado");
        cashier.AdSell();


    }
    public void Show50RewardedAd()
    {
        rewarded50Ad.Show();
        Request50Rewarded();
    }


}
