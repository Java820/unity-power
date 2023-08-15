using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Cantidad de generadores activos
    [SerializeField] EnergyGenerator[] toyBatteryGenerators;
    [SerializeField] EnergyGenerator[] carBatteryGenerators;
    [SerializeField] EnergyGenerator[] gasolineGenerators;
    [SerializeField] EnergyGenerator[] solarPanelGenerators;
    [SerializeField] EnergyGenerator[] eolicTurbineGenerators;
    [SerializeField] EnergyGenerator[] teslaGenerators;
    [SerializeField] EnergyGenerator[] nuclearFisionGenerators;
    [SerializeField] EnergyGenerator[] nuclearFusionGenerators;


    PlayfabManager playfabManager;
    GameData gameData;
    GoogleAdsManager googleAdsManager;

    float actualBoostTimer = 0;

    [SerializeField] Button rewardedGoldButton;

    [NonSerialized] float interstitialAdTimer = 90;
    [NonSerialized] float actualAdTimer;
    [NonSerialized] public bool adShowable = false;

    TextMeshProUGUI boostTimerText;



    void Awake()
    {
        //UpdateQuantity();
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
        googleAdsManager = GameObject.Find("Managers").GetComponent<GoogleAdsManager>();
        boostTimerText = GameObject.Find("x2 Power Timer").GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        actualAdTimer = interstitialAdTimer;
    }

    void Update()
    {
        Lean.Localization.LeanLocalization.SetToken("ADSLIMIT", gameData.adsEnergy.ToString());

        if (actualBoostTimer > 0)
        {
            actualBoostTimer -= 1 * Time.deltaTime;
            gameData.playerMultiplier = 2f;
            boostTimerText.enabled = true;

            TimeSpan time = TimeSpan.FromSeconds(actualBoostTimer);
            boostTimerText.text = "<sprite=0>x2 - "+ time.ToString("mm':'ss");
        }
        else
        {
            gameData.playerMultiplier = 1f;
            boostTimerText.enabled = false;
        }

        if (gameData.adsEnergy >= 1)
        {
            rewardedGoldButton.interactable = true;
        }
        else
        {
            rewardedGoldButton.interactable = false;
        }

        if (actualAdTimer > 0)
        {
            if (adShowable == false)
            {
                actualAdTimer -= Time.deltaTime;
            }
        }
        else
        {
            adShowable = true;
            actualAdTimer = interstitialAdTimer;
        }

    }


    public void UpdateGeneratorsQuantity()
    {

        //Revisa cada uno de los generadores para checar a los que estan activos

        foreach (EnergyGenerator i in toyBatteryGenerators)
        {
            int generatorsOfThisType = 0;

            //Revisa cada uno de los generadores y los que estan activos, los suma a una variable
            foreach (EnergyGenerator j in toyBatteryGenerators)
            {

                if (j.isUnlocked == true)
                {
                    generatorsOfThisType++;
                }


            }
            //Startcos es startcos del inspector, luego va este ciclo
            double initialCost = i.startCost;

            for (int x = 1; x <= generatorsOfThisType; x++)
            {
                //Debug.Log(x + " atnes " + initialCost);
                initialCost = Math.Round(initialCost * Math.Pow(i.rateGrowth, x), 2);
                //Debug.Log(x + " despues " + initialCost);

            }
            i.actualCost = initialCost;


            /*double seiso = 3.74;
            for (int i = 1; i <= 8; i++)
            {
                seiso = Math.Round(seiso * Math.Pow(1.07, i), 2);
                Debug.Log(i + ": " + seiso);
            }*/

        }



        EnergyGenerator[] allGenerators = FindObjectsOfType<EnergyGenerator>();

        foreach (var item in allGenerators)
        {
            item.UpdateGenerator();
        }



    }

    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void GoldMultiplierBoost(int goldAmount)
    {
        actualBoostTimer += goldAmount * 60;
        playfabManager.UseVC("GO", goldAmount);
    }

    public void FreeGoldButton()
    {

        ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("free_gold"), Lean.Localization.LeanLocalization.GetTranslationText("free_gold_text"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("yes"), Callback = CallFreeGoldAd }, new ModalButton() { Text = "No" } });
    }

    void CallFreeGoldAd()
    {
        googleAdsManager.ShowGoldRewardedAd();
    }


}
