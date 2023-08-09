using System;
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
    TextMeshProUGUI versionText;

    string id;
    public bool isLogged;

    public bool isUserNew = false;
    GameData gameData;
    GameManager gameManager;

    void Awake()
    {
        gameData = this.gameObject.GetComponent<GameData>();
        versionText = GameObject.Find("Version Text").GetComponent<TextMeshProUGUI>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        LeanTween.rotateAround(loadingPanel.transform.GetChild(0).gameObject, new Vector3(0, 0, 1), 360f, 2f).setLoopClamp();
        loadingPanel.SetActive(false);
    }
    public void newStart()
    {
        id = SystemInfo.deviceUniqueIdentifier;
        versionText.text = "v: " + Application.version;
        //LeanTween.moveY(logo.GetComponent<RectTransform>(), 10f, 1.75f).setLoopPingPong().setEase(easeType);
        CloseLoadingPanel();
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
        int newAmount = Convert.ToInt32(amount * 100);
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
        {
            Amount = newAmount,
            VirtualCurrency = vc
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnCurrencyChange, OnError);
    }
    public void UseVC(string vc, double amount)
    {
        int newAmount = Convert.ToInt32(amount * 100);
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
        GetCurrencies();
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
        GetCurrencies();
        if (int.Parse(result.Data["titleVersion"].Substring(result.Data["titleVersion"].LastIndexOf('.') + 1)) <= int.Parse(Application.version.Substring(Application.version.LastIndexOf('.') + 1)))
        {
            GetCatalog();

            if (isUserNew == false)
            {
                GetPlayerData();
            }
            else
            {
                SavePlayerData();
                SaveLevelsDictionary();
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

        if (result.Data != null && result.Data.ContainsKey("Skin") && result.Data.ContainsKey("GameDictionary"))
        {
            gameData.playerSkin = result.Data["Skin"].Value;
            //Debug.Log(result.Data["Levels"].Value);

            gameData.gameDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(result.Data["Levels"].Value);

            //StartCoroutine(UpdatePlayer());
            isLogged = true;
        }
        else
        {
            Debug.Log("Data not complete.");
            SavePlayerData();
        }

        CloseLoadingPanel();
        menuPanel.SetActive(false);
    }
    public void SavePlayerData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    {"Skin", gameData.playerSkin}
                    /*{"Helmet", player_helmet},
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
        //Debug.Log("Player data sent!");

    }

    public void SaveLevelsDictionary()
    {
        // Convertir los diccionarios en cadenas JSON
        string levelDicJson = JsonConvert.SerializeObject(gameData.gameDictionary);

        // Crear una solicitud para guardar los datos del jugador en PlayFab
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                {
                    { "GameDictionary", levelDicJson },
                }
        };

        // Enviar la solicitud a PlayFab
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void GetCurrencies()
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
        /*playerData.energy = result.VirtualCurrency["EN"];
        playerData.adsEnergy = result.VirtualCurrency["AD"];
        moneyText.text = "EV: " + playerData.evMoney.ToString();*/
        //secondsLeftToRefreshEnergy = result.VirtualCurrencyRechargeTimes["EN"].SecondsToRecharge;


    }




}
