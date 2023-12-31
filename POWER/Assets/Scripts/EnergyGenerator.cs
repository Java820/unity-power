using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//public enum GeneratorType { ToyBattery, CarBattery, GasolineGenerator, SolarPanel, EolicTurbine, TeslaGenerator, NuclearFision, NuclearFusion }
public class EnergyGenerator : MonoBehaviour
{
    [SerializeField] TextMeshPro productionAccumulatedText;
    [SerializeField] TextMeshPro unlockPriceText;
    //public GeneratorType generatorType;

    //Ganancias iniciales
    [SerializeField] int productionBaseIncome;
    //Costo de desbloqueo
    public double startCost;
    //Costo actual
    [NonSerialized] public double actualCost;
    //Ratio de crecimiento
    public double rateGrowth = 1.07;

    //Tiempo en generar energia
    [SerializeField] float delay;
    float timer;
    //Producción acumulado
    [SerializeField] int accumulatedProduction;
    [SerializeField] double topProduction;

    [SerializeField] bool isFirstGenerator = false;

    /*-----DATOS A GUARDAR-----*/
    //Esta desbloqueado
    public bool isUnlocked = false;
    //id
    public string id;


    GameManager gameManager;
    GameData gameData;
    PlayfabManager playfabManager;
    GoogleAdsManager googleAdsManager;
    Animator anim;
    [SerializeField] ParticleSystem dustExplosion;
    [SerializeField] Transform productionBar;
    float initialScale;


    void Awake()
    {
        gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
        gameData = GameObject.Find("Managers").GetComponent<GameData>();
        playfabManager = GameObject.Find("Managers").GetComponent<PlayfabManager>();
        googleAdsManager = GameObject.Find("Managers").GetComponent<GoogleAdsManager>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        timer = 0;
        actualCost = startCost;
        initialScale = productionBar.localScale.x;
        timer = delay;
    }

    public void newStart()
    {
        if (gameData.gameDictionary.ContainsKey(id))
        {
            isUnlocked = true;
        }
        gameManager.UpdateGeneratorsQuantity();
    }

    public void UpdateGenerator()
    {
        anim.SetBool("isUnlocked", isUnlocked);

        if (isUnlocked)
        {
            unlockPriceText.enabled = false;
        }
        else
        {
            if (isFirstGenerator)
            {
                unlockPriceText.text = "$: " + actualCost.ToString();
            }
            else
            {
                if (actualCost == 0)
                {
                    unlockPriceText.text = "";
                }
                else
                {
                    unlockPriceText.text = "$: " + actualCost.ToString();
                }

            }
        }
    }
    void Update()
    {
        if (isUnlocked)
        {
            if (accumulatedProduction < topProduction * Convert.ToInt32(gameData.playerMultiplier))
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    GenerateEnergy();
                    timer = delay;
                }
            }
            productionBar.localScale = new Vector3((initialScale * timer) / delay, productionBar.localScale.y, productionBar.localScale.z);
        }

    }
    void GenerateEnergy()
    {
        accumulatedProduction += productionBaseIncome * Convert.ToInt32(gameManager.actualPlayerMultiplier);
        productionAccumulatedText.text = accumulatedProduction.ToString();
    }

    void GetEnergy()
    {
        StartCoroutine(gameData.AddPower(accumulatedProduction));
        accumulatedProduction = 0;
        productionAccumulatedText.text = accumulatedProduction.ToString();
    }

    void UnlockGenerator()
    {
        //TODO: Agregar que cueste dinero hacer esto
        if (gameData.playerMoney >= actualCost)
        {
            isUnlocked = true;

            gameData.gameDictionary.Add(id, isUnlocked);
            playfabManager.UseVC("CA", actualCost);
            anim.SetBool("isUnlocked", isUnlocked);
            dustExplosion.Play();
            gameManager.UpdateGeneratorsQuantity();
        }

    }

    public void GeneratorButton()
    {
        if (gameManager.adShowable == true)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                googleAdsManager.ShowInterstitial();
            }
        
        }
        if (isUnlocked)
        {
            GetEnergy();
        }
        else
        {
            UnlockGenerator();
        }
    }



}
