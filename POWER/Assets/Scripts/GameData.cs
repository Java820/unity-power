using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class GameData : MonoBehaviour
{
    public string playerName;
    public string playerSkin;
    public int playerPower;
    public double playerMoney;
    public int playerGold;
    public double playerMultiplier;
    public bool isUserPremium;
    public double exchange;

    public int adsEnergy;


    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] LeanTweenType easeType;
    public Dictionary<string, bool> gameDictionary = new Dictionary<string, bool>();


    private void Start()
    {

    }

    void Update()
    {
        powerText.text = playerPower.ToString() + "<sprite=0>";
        moneyText.text = "$" + playerMoney.ToString();
        goldText.text = "<sprite=1>" + playerGold.ToString();
    }


    void ReturnPower()
    {
        LeanTween.scaleY(powerText.gameObject, 1f, 0.025f);
        LeanTween.scaleX(powerText.gameObject, 1, 0.025f);
    }

    public IEnumerator AddPower(int scoreToAdd)
    {
        int dummyPower = playerPower;
        while (playerPower < (dummyPower + scoreToAdd))
        {
            LeanTween.scaleY(powerText.gameObject, 1.125f, 0.025f).setOnComplete(ReturnPower);
            LeanTween.scaleX(powerText.gameObject, 1.125f, 0.025f).setOnComplete(ReturnPower);

            playerPower += 1;
            yield return new WaitForSeconds(0.05f);

        }
        //dummyPower = playerPower;
        yield break;
    }

}
