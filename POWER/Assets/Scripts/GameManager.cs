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

    float actualBoostTimer = 0;

    [SerializeField] Button rewardedGoldButton;

    void Awake()
    {
        //UpdateQuantity();
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
        gameData = GameObject.Find("Managers").GetComponent<GameData>();

    }
    public void UpdateQuantity()
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

            i.UpdateGenerator();

        }
    }

    void Update()
    {
        Lean.Localization.LeanLocalization.SetToken("ADSLIMIT", gameData.adsEnergy.ToString());

        if (actualBoostTimer > 0)
        {
            actualBoostTimer -= 1 * Time.deltaTime;
            gameData.playerMultiplier = 2f;
        }
        else
        {
            gameData.playerMultiplier = 1f;
        }

        if (gameData.adsEnergy >= 1)
        {
            rewardedGoldButton.interactable = true;
        }
        else
        {
            rewardedGoldButton.interactable = false;
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
        playfabManager.UseVC("GO", Convert.ToInt32(goldAmount));
    }

    public void FreeGoldButton()
    {

        ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("free_gold"), Lean.Localization.LeanLocalization.GetTranslationText("free_gold_text"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("yes"), Callback = CallFreeGoldAd }, new ModalButton() { Text = "No" } });
    }

    void CallFreeGoldAd()
    {
        //TODO Llamar a call ad en el ads manager
    }


}
