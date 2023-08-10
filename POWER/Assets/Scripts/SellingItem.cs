using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.UI;

public class SellingItem : MonoBehaviour, System.IComparable<SellingItem>
{
    GameData gameData;
    PlayfabManager playfabManager;
    PlayerController playerController;

    [SerializeField] Image itemImage;
    [SerializeField] Sprite itemSprite;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI nameText;
    public double price;
    public string nameId;
    public bool isBuyed;
    public bool isEquipped;

    void Awake()
    {
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        itemImage.sprite = itemSprite;

        if (isBuyed)
        {
            if (isEquipped)
            {

            }
        }
        else
        {
            priceText.text = "$" + price.ToString();
        }
        nameText.text = Lean.Localization.LeanLocalization.GetTranslationText(nameId);
    }

    public int CompareTo(SellingItem other)
    {
        // Comparar por precio
        return price.CompareTo(other.price);
    }

    public void ItemButton()
    {
        if (isBuyed)
        {
            Equip();
        }
        else
        {
            Buy();
        }
        playfabManager.SavePlayerData();
        playfabManager.SaveGameDictionary();
    }

    void Buy() 
    {
        if (gameData.playerMoney >= price)
        { 
            //Comprar
        }
        else 
        {
            ModalManager.Show("", Lean.Localization.LeanLocalization.GetTranslationText("no_money"), new[] { new ModalButton() { Text = "OK" } });
        }
    }

    void Equip()
    {
        if (nameId.StartsWith("skin"))
        {
            gameData.playerSkin = nameId;
            playerController.SetupSkin(nameId);
        }
    }
}
