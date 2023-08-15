using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cashier : MonoBehaviour
{
    GameData gameData;
    PlayfabManager playfabManager;
    GoogleAdsManager googleAdsManager;
    [SerializeField] TextMeshPro cashierText;
    [SerializeField] TextMeshProUGUI exchangeText;

    double cashierMultiplier = 1;
    [SerializeField] GameObject cashierPanel;

    [SerializeField] double exchange05UpgradePrice;
    [SerializeField] double exchange10UpgradePrice;
    [SerializeField] double exchange15UpgradePrice;
    [SerializeField] double exchange20UpgradePrice;
    [SerializeField] double exchange25UpgradePrice;
    double exchangeUpgradePrice;

    void Awake()
    {
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
        googleAdsManager = GameObject.Find("Managers").GetComponent<GoogleAdsManager>();
    }
    void Start()
    {
    }

    void Update()
    {
        cashierText.text = gameData.exchange.ToString();
        exchangeText.text = "$"+gameData.exchange.ToString();
    }

    double CalculateSellPrice(int power)
    {
        double sellPrice = ((power * gameData.exchange) * cashierMultiplier);
        return sellPrice;
    }
    double CalculateAdSellPrice(int power)
    {
        double sellPrice = ((power * gameData.exchange) * cashierMultiplier);
        sellPrice += sellPrice * 0.50;
        return sellPrice;
    }

    public void CashierButton()
    {
        if (gameData.playerPower > 0)
        {
            if (cashierPanel.activeSelf == false)
            {
                StartCoroutine(ButtonCoroutine());
            }
        }
    }

    IEnumerator ButtonCoroutine()
    {

        Lean.Localization.LeanLocalization.SetToken("ENERGYAMOUNT", gameData.playerPower.ToString());
        Lean.Localization.LeanLocalization.SetToken("SELLAMOUNT", CalculateSellPrice(gameData.playerPower).ToString());
        Lean.Localization.LeanLocalization.SetToken("ADSELLAMOUNT", CalculateAdSellPrice(gameData.playerPower).ToString());

        yield return null;
        /* ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("title_sell"), Lean.Localization.LeanLocalization.GetTranslationText("body_sell"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("sell"), Callback = Sell },
                                                                                                                                                                            new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("ad_sell"), Callback = CallAd},
                                                                                                                                                                            new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("cancel")}});*/

        cashierPanel.SetActive(true);

    }

    public void Sell()
    {
        Debug.Log("Vendido por: " + CalculateSellPrice(gameData.playerPower));
        playfabManager.AddVC("CA", CalculateSellPrice(gameData.playerPower));
        gameData.playerPower = 0;
        cashierPanel.SetActive(false);

    }
    public void CallAd()
    {
        googleAdsManager.Show50RewardedAd();
    }
    public void AdSell()
    {
        Debug.Log("Vendido por: " + CalculateAdSellPrice(gameData.playerPower));
        playfabManager.AddVC("CA", CalculateAdSellPrice(gameData.playerPower));
        gameData.playerPower = 0;
        cashierPanel.SetActive(false);

    }

    public void UpgradeExchangeButton()
    {
        if (gameData.exchange < 2.5)
        {
            StartCoroutine(UpgradeExchangeCoroutine());
        }
    }

    IEnumerator UpgradeExchangeCoroutine()
    {
        double initialCost = 0;

        if (gameData.exchange < 0.5)
        {
            initialCost = exchange05UpgradePrice;

            for (int x = 1; x <= (gameData.exchange * 10); x++)
            {
                //Debug.Log(x + " atnes " + initialCost);
                initialCost = Math.Round(initialCost * Math.Pow(1.67, x), 2);
                //Debug.Log(x + " despues " + initialCost);

            }
        }
        else if (gameData.exchange < 1.0)
        {
            initialCost = exchange10UpgradePrice;

            for (int x = 1; x <= ((gameData.exchange -0.4) * 10); x++)
            {
                //Debug.Log(x + " atnes " + initialCost);
                initialCost = Math.Round(initialCost * Math.Pow(1.07, x), 2);
                //Debug.Log(x + " despues " + initialCost);

            }
        }
        else if (gameData.exchange < 1.5)
        {
            initialCost = exchange15UpgradePrice;

            for (int x = 1; x <= ((gameData.exchange -0.9) * 10); x++)
            {
                //Debug.Log(x + " atnes " + initialCost);
                initialCost = Math.Round(initialCost * Math.Pow(1.07, x), 2);
                //Debug.Log(x + " despues " + initialCost);

            }
        }
        else if (gameData.exchange < 2.0)
        {
            initialCost = exchange20UpgradePrice;

            for (int x = 1; x <= ((gameData.exchange -1.4) * 10); x++)
            {
                //Debug.Log(x + " atnes " + initialCost);
                initialCost = Math.Round(initialCost * Math.Pow(1.07, x), 2);
                //Debug.Log(x + " despues " + initialCost);

            }
        }
        else
        {
            initialCost = exchange25UpgradePrice;

            for (int x = 1; x <= ((gameData.exchange -1.9) * 10); x++)
            {
                //Debug.Log(x + " atnes " + initialCost);
                initialCost = Math.Round(initialCost * Math.Pow(1.07, x), 2);
                //Debug.Log(x + " despues " + initialCost);

            }
        }

        exchangeUpgradePrice = initialCost;

        Lean.Localization.LeanLocalization.SetToken("EXCHANGEUPGRADEPRICE", exchangeUpgradePrice.ToString());

        yield return null;
        /* ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("title_sell"), Lean.Localization.LeanLocalization.GetTranslationText("body_sell"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("sell"), Callback = Sell },
                                                                                                                                                                            new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("ad_sell"), Callback = CallAd},
                                                                                                                                                                            new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("cancel")}});*/
        ModalManager.Show("", Lean.Localization.LeanLocalization.GetTranslationText("upgrade_exchange"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("yes"), Callback = UpgradeEchange }, new ModalButton() { Text = "No" } });


    }/**/

    void UpgradeEchange()
    {
        if (exchangeUpgradePrice <= gameData.playerMoney)
        {
            //TODO mejorar el exchange
            gameData.exchange += 0.10;
            playfabManager.UseVC("CA", exchangeUpgradePrice);
            cashierPanel.SetActive(false);
        }
        else
        {
            ModalManager.Show("", Lean.Localization.LeanLocalization.GetTranslationText("no_money"), new[] { new ModalButton() { Text = "OK" } });
        }

    }
}
