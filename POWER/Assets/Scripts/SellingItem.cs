using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.UI;

public class SellingItem : MonoBehaviour, System.IComparable<SellingItem>
{
    GameData gameData;

    [SerializeField] Image itemImage;
    [SerializeField] Sprite itemSprite;
    [SerializeField] TextMeshProUGUI priceText;
    public double price;
    public string nameId;
    public bool isBuyed;
    public bool isEquipped;
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
    }

    public int CompareTo(SellingItem other)
    {
        // Comparar por precio
        return price.CompareTo(other.price);
    }

    void Button()
    {
        if (isBuyed)
        {
            Equip();
        }
        else
        {
            Buy();
        }
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
    
    }
}
