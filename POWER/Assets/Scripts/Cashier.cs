using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cashier : MonoBehaviour
{
    GameData gameData;
    PlayfabManager playfabManager;
    [SerializeField] TextMeshPro exchangeText;

    double cashierMultiplier = 1;

    void Awake()
    {
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
    }
    void Start()
    {

    }

    void Update()
    {
        exchangeText.text = gameData.exchange.ToString();
    }

    double CalculateSellPrice(int power)
    {
        double sellPrice = ((power * gameData.exchange) * cashierMultiplier);
        return sellPrice;
    }
    double CalculateAdSellPrice(int power)
    {
        double sellPrice = ((power * gameData.exchange) * cashierMultiplier);
        sellPrice += sellPrice * 0.20;
        return sellPrice;
    }

    public void CashierButton()
    {
        if (gameData.playerPower > 0)
        {
            StartCoroutine(ButtonCoroutine());
        }
    }

    IEnumerator ButtonCoroutine()
    {
        Lean.Localization.LeanLocalization.SetToken("ENERGYAMOUNT", gameData.playerPower.ToString());
        Lean.Localization.LeanLocalization.SetToken("SELLAMOUNT", CalculateSellPrice(gameData.playerPower).ToString());
        Lean.Localization.LeanLocalization.SetToken("ADSELLAMOUNT", CalculateAdSellPrice(gameData.playerPower).ToString());

        yield return null;
        ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("title_sell"), Lean.Localization.LeanLocalization.GetTranslationText("body_sell"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("sell"), Callback = Sell },
                                                                                                                                                                            new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("ad_sell"), Callback = AdSell},
                                                                                                                                                                            new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("cancel")}});

    }

    void Sell()
    {
        Debug.Log("Vendido por: " + CalculateSellPrice(gameData.playerPower));
        playfabManager.AddVC("CA", CalculateSellPrice(gameData.playerPower));
        gameData.playerPower = 0;
    }
    void AdSell()
    {
        Debug.Log("Vendido por: " + CalculateAdSellPrice(gameData.playerPower));
        playfabManager.AddVC("CA", CalculateAdSellPrice(gameData.playerPower));
        gameData.playerPower = 0;
    }
}
