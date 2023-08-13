using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItem : MonoBehaviour
{
    Button button;
    GameData gameData;
    GameManager gameManager;

    [SerializeField] string vc;
    [SerializeField] int price;

    void Awake()
    {
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
        gameManager = GameObject.Find("Managers").GetComponent<GameManager>();

        button = gameObject.transform.GetComponent<Button>();
    }

    void Update()
    {
        switch (vc)
        {
            case "CA":
                if (gameData.playerMoney < price)
                {
                    button.enabled = false;
                }
                else
                {
                    button.enabled = true;
                }
                break;
            case "GO":
                if (gameData.playerGold < price)
                {
                    button.enabled = false;
                }
                else
                {
                    button.enabled = true;
                }
                break;
        }
    }
    public void ThisButton()
    {
        ModalManager.Show("", Lean.Localization.LeanLocalization.GetTranslationText("confirm_purchase"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("yes"), Callback = BuyAction }, new ModalButton() { Text = "No" } });
    }

    void BuyAction()
    {
        gameManager.GoldMultiplierBoost(1);
    }
}
