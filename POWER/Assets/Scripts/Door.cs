using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Door : MonoBehaviour
{
    PlayfabManager playfabManager;
    GameData gameData;
    [SerializeField] TextMeshPro priceText;
    //[SerializeField] GameObject unlockButton;
    [SerializeField] string id;
    [SerializeField] double price;

    void Awake()
    {
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
    }

    void Start()
    {
        priceText.text = "$: " + price.ToString();
    }

    void Update()
    {
        if (gameData.gameDictionary.ContainsKey(id))
        {
            //isUnlocked = true;
            transform.gameObject.SetActive(false);
        }
        else
        {
            transform.gameObject.SetActive(true);
        }
    }

    public void UnlockDoor()
    {
        if (gameData.playerMoney >= price)
        {
            gameData.gameDictionary.Add(id, true);
            playfabManager.UseVC("CA", price);
        }
    }
}
