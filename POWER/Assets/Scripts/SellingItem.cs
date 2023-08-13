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
        Setup();
        playfabManager.GetInventory();

        itemImage.sprite = itemSprite;
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
            /* if (isEquipped)
            {
                UnEquip();
            }
            else
            {
                Equip();
            }*/
            Equip();
        }
        else
        {
            Buy();
        }
        playfabManager.GetInventory();
    }

    void Buy() 
    {
        if (gameData.playerMoney >= price)
        {
            //Comprar
            
            ModalManager.Show("", Lean.Localization.LeanLocalization.GetTranslationText("confirm_purchase"), new[] { new ModalButton() { Text = Lean.Localization.LeanLocalization.GetTranslationText("yes"),Callback = BuyAction}, new ModalButton() { Text = "No"} });

        }
        else 
        {
            ModalManager.Show("", Lean.Localization.LeanLocalization.GetTranslationText("no_money"), new[] { new ModalButton() { Text = "OK" } });
        }
    }

    void BuyAction()
    {
        playfabManager.MakePurchase(nameId, price);
    }

    public void Setup()
    {
        if (nameId.StartsWith("skin"))
        {
            if (gameData.playerSkin == nameId)
            {
                Equip();
            }
            else
            {
                UnEquip();
            }
        }
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

    }

    void Equip()
    {
        if (nameId.StartsWith("skin"))
        {
            gameData.playerSkin = nameId;
            playerController.SetupSkin(nameId);
        }
        isEquipped = true;
        priceText.text = "x";
    }
    void UnEquip()
    {
        if (nameId.StartsWith("skin"))
        {
        }
        isEquipped = false;
        priceText.text = "-";
    }
}
