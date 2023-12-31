﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;


public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject[] hidableObjects;
    [SerializeField] TextMeshProUGUI versionText;

    string id;
    public bool isLogged;

    public bool isUserNew = false;
    GameData gameData;
    GameManager gameManager;
    PlayerController playerController;

    void Awake()
    {
        gameData = this.gameObject.GetComponent<GameData>();
        gameManager = this.gameObject.GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        //versionText = GameObject.Find("Version Text").GetComponent<TextMeshProUGUI>();


    }

    private void Start()
    {
        LeanTween.rotateAround(loadingPanel.transform.GetChild(0).gameObject, new Vector3(0, 0, 1), -360f, 2f).setLoopClamp();
        loadingPanel.SetActive(false);
        versionText.text = "v: " + Application.version;

    }
    public void newStart()
    {
        id = SystemInfo.deviceUniqueIdentifier;
        versionText.text = "v: " + Application.version;
        //LeanTween.moveY(logo.GetComponent<RectTransform>(), 10f, 1.75f).setLoopPingPong().setEase(easeType);
        menuPanel.SetActive(false);
        playerController.SetupSkin(gameData.playerSkin);

        EnergyGenerator[] allGenerators = FindObjectsOfType<EnergyGenerator>();

        foreach (var item in allGenerators)
        {
            item.newStart();
        }
        gameManager.Setup();
    }

    private void OnDisable()
    {
        if (isLogged)
        {
            SaveGameDictionary();
            SavePlayerData();
        }
    }

    public void PlayButton()
    {
        StartCoroutine(CheckConnection());
        OpenLoadingPanel();
    }

    public void CloseLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
    public void OpenLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }


    IEnumerator CheckConnection()
    {
        // Realiza una solicitud a un URL para comprobar si hay conexi�n a internet.
        UnityWebRequest request = UnityWebRequest.Get("https://google.com");

        // Env�a la solicitud y espera la respuesta.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("connection_error"), Lean.Localization.LeanLocalization.GetTranslationText("check_wifi"), new[] { new ModalButton() { Text = "OK", Callback = CloseLoadingPanel } });
        }
        else
        {
            Login();
        }
    }


    void Update()
    {

    }
    public void Login()
    {

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,

            CreateAccount = true,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        isLogged = true;
        Debug.Log("Sesion iniciada: " + SystemInfo.deviceUniqueIdentifier);
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            gameData.playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
            isUserNew = false;
        }
        else
        {
            isUserNew = true;
        }
        GetVersion();


        /*if (playerSetup.maxScore > 99999)
        {
            AddBan(result.PlayFabId, banHours);
        }*/

        //SubmitName();


        //if (name == null)
        //nameWindow.SetActive(true);


        //UpdateLeaderBoardData();

    }

    private void OnLoginError(PlayFabError error)
    {
        isLogged = false;

        Debug.Log("Error while logging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }
    public void AddVC(string vc, double amount)
    {
        int newAmount = Convert.ToInt32(amount);
        if (vc == "CA")
        {
            newAmount = Convert.ToInt32(amount * 100);
        }
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
        {
            Amount = newAmount,
            VirtualCurrency = vc
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnCurrencyChange, OnError);
    }
    public void UseVC(string vc, double amount)
    {
        int newAmount = Convert.ToInt32(amount);
        if (vc == "CA")
        {
            newAmount = Convert.ToInt32(amount * 100);
        }
        SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest
        {
            Amount = newAmount,
            VirtualCurrency = vc
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnCurrencyChange, OnError);
    }

    private void OnCurrencyChange(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Added Currencies");
        GetInventory();
    }


    private void OnError(PlayFabError error)
    {
        Debug.Log("Error " + error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }

    void GetVersion()
    {
        var request = new PlayFab.ClientModels.GetTitleDataRequest
        {
            //Keys = new List<string> { "titleVersion" },
        };
        PlayFabClientAPI.GetTitleData(request, OnTitleDataSuccess, OnLoginError);
    }

    void OnTitleDataSuccess(GetTitleDataResult result)
    {
        GetInventory();
        if (int.Parse(result.Data["titleVersion"].Substring(result.Data["titleVersion"].LastIndexOf('.') + 1)) <= int.Parse(Application.version.Substring(Application.version.LastIndexOf('.') + 1)))
        {
            GetCatalog();

            if (isUserNew == false)
            {
                GetPlayerData();
                GetInventory();
            }
            else
            {
                SavePlayerData();
                SaveGameDictionary();
            }

        }
        else
        {
            ModalManager.Show(Lean.Localization.LeanLocalization.GetTranslationText("new_version"), Lean.Localization.LeanLocalization.GetTranslationText("update"), new[] { new ModalButton() { Text = "OK", Callback = CloseLoadingPanel } });
        }
    }
    public void GetCatalog()
    {

        var request = new PlayFab.ClientModels.GetCatalogItemsRequest
        {
            CatalogVersion = "Items Catalog"
        };
        PlayFabClientAPI.GetCatalogItems(request, OnCatalogGet, OnError);
    }

    private void OnCatalogGet(PlayFab.ClientModels.GetCatalogItemsResult result)
    {
        foreach (var item in result.Catalog)
        {
        }

    }

    public void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    private void OnDataRecieved(GetUserDataResult result)
    {

        if (result.Data != null && result.Data.ContainsKey("Skin") && result.Data.ContainsKey("GameDictionary") && result.Data.ContainsKey("Exchange") && result.Data.ContainsKey("PlayerMultiplier") && result.Data.ContainsKey("Premium"))
        {
            gameData.playerSkin = result.Data["Skin"].Value;
            gameData.exchange = Convert.ToDouble(result.Data["Exchange"].Value);
            gameData.playerMultiplier = Convert.ToDouble(result.Data["PlayerMultiplier"].Value);
            gameData.isUserPremium = Convert.ToBoolean(result.Data["Premium"].Value);
            //Debug.Log(result.Data["Levels"].Value);

            gameData.gameDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(result.Data["GameDictionary"].Value);

            //StartCoroutine(UpdatePlayer());
            isLogged = true;
        }
        else
        {
            Debug.Log("Data not complete.");
            SavePlayerData();
        }

        CloseLoadingPanel();
        newStart();
    }
    public void SavePlayerData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    {"Skin", gameData.playerSkin},
                    {"Exchange", gameData.exchange.ToString()},
                    {"PlayerMultiplier", gameData.playerMultiplier.ToString()},
                    {"Premium", gameData.isUserPremium.ToString()}
                    /*,
                    {"Chestplate", player_chestplate},
                    {"Arms", player_arms},
                    {"Pants", player_pants},
                    {"isUserPremium", isPremium.ToString()},
                    {"dReward", dailyReward.ToString()},
                    {"dateReward", thisDateReward.ToString()}*/
                }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Player data sent!");

    }

    public void SaveGameDictionary()
    {
        // Convertir los diccionarios en cadenas JSON
        string gameDicJson = JsonConvert.SerializeObject(gameData.gameDictionary);

        // Crear una solicitud para guardar los datos del jugador en PlayFab
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    { "GameDictionary", gameDicJson },
                }
        };

        // Enviar la solicitud a PlayFab
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void GetInventory()
    {

        var request = new PlayFab.ClientModels.GetUserInventoryRequest
        {
            //CustomId = id
        };
        PlayFabClientAPI.GetUserInventory(request, OnUserCurrencyGet, OnError);
    }

    private void OnUserCurrencyGet(PlayFab.ClientModels.GetUserInventoryResult result)
    {

        gameData.playerMoney = Convert.ToDouble(result.VirtualCurrency["CA"]) / 100;
        gameData.playerGold = Convert.ToInt32(result.VirtualCurrency["GO"]);
        gameData.adsEnergy = Convert.ToInt32(result.VirtualCurrency["AD"]);

        /*playerData.energy = result.VirtualCurrency["EN"];
        playerData.adsEnergy = result.VirtualCurrency["AD"];
        moneyText.text = "EV: " + playerData.evMoney.ToString();*/
        //secondsLeftToRefreshEnergy = result.VirtualCurrencyRechargeTimes["EN"].SecondsToRecharge;

        SellingItem[] storeItemButtons = FindObjectsOfType<SellingItem>();
        foreach (var item in result.Inventory)
        {
            foreach (var i in storeItemButtons)
            { 
                if(i.nameId == item.ItemId)
                {
                    i.isBuyed = true;
                }
                
            }
        }
        foreach (var i in storeItemButtons)
        {
            i.Setup();
        }


    }

    public void MakePurchase(string id, double price)
    {
        
        var request = new PurchaseItemRequest
        {
            CatalogVersion = "Items Catalog",
            ItemId = id,
            Price = Convert.ToInt32(price * 100),
            VirtualCurrency = "CA"
        };
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, OnError);
    }

    void OnPurchaseSuccess(PurchaseItemResult result)
    {
        Debug.Log("Purchase complete");
        GetInventory();
    }

    public void SetPremium()
    {
        gameData.isUserPremium = true;
        gameData.playerMultiplier = 2;
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    {"Premium", "true"},
                    {"PlayerMultiplier", "2"},
                }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }


    public void AddGold(int num)
    {
        AddVC("GO", num);
    }



}
