using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSendDataScript : MonoBehaviour
{
    [SerializeField] private GameObject[] page;

    [SerializeField] private GameObject lottery;
    [SerializeField] private GameManagerScript GMS;
    [SerializeField] private Slider prepareLoad;
    [SerializeField] private Slider[] sld;
    [SerializeField] private InputField[] iField;
    [SerializeField] private Text[] upgradePrice;
    private Text percent;

    private int gold;
    private int log;
    private int board;
    private int table;
    private int maxCarLoad;
    private int waitingTime;

    private int prepareLog;
    private int prepareBoard;
    private int prepareTable;

    private string itemName;

    private void Start()
    {
        percent = prepareLoad.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        setAll();
    }

    private void setAll()
    {
        gold = GMS.GetGold();
        log = GMS.GetLog();
        board = GMS.GetBoard();
        table = GMS.GetTable();
        maxCarLoad = GMS.GetCarStorageLvl();
        waitingTime = GMS.GetCarSpeedLvl();

        sld[0].maxValue = log;
        sld[1].maxValue = board;
        sld[2].maxValue = table;
        prepareLoad.maxValue = maxCarLoad;
        
        sld[0].value = 0;
        sld[1].value = 0;
        sld[2].value = 0;

        iField[0].text = "0";
        iField[1].text = "0";
        iField[2].text = "0";

        prepareLog = 0;
        prepareBoard = 0;
        prepareTable = 0;

        GMS.setUpgradePrice("storage", upgradePrice[0], upgradePrice[0].transform.parent.parent.GetChild(3).GetComponent<Text>());
        GMS.setUpgradePrice("time", upgradePrice[1], upgradePrice[1].transform.parent.parent.GetChild(3).GetComponent<Text>());
    }

    public void SendItemLog(float flt)
    {
        int numberOfItem = Mathf.FloorToInt(flt);
        string name = "Log";
        prepareForSell(name,numberOfItem);
    }
    public void SendItemBoard(float flt)
    {
        int numberOfItem = Mathf.FloorToInt(flt);
        string name = "Board";
        prepareForSell(name, numberOfItem);
    }
    public void SendItemTable(float flt)
    {
        int numberOfItem = Mathf.FloorToInt(flt);
        string name = "Table";
        prepareForSell(name, numberOfItem);
    }

    public void SendItemLog(string str)
    {
        int numberOfItem = int.Parse(str);
        string name = "Log";
        numberOfItem = numberOfItem > log ? log : numberOfItem;
        prepareForSell(name,numberOfItem);
    }
    public void SendItemBoard(string str)
    {
        int numberOfItem = int.Parse(str);
        string name = "Board";
        numberOfItem = numberOfItem > board ? board : numberOfItem;
        prepareForSell(name, numberOfItem);
    }
    public void SendItemTable(string str)
    {
        int numberOfItem = int.Parse(str);
        string name = "Table";
        numberOfItem = numberOfItem > table ? table : numberOfItem;
        prepareForSell(name, numberOfItem);
    }

    private void prepareForSell(string name, int number)
    {
        int anotherItem = 0;
        switch (name)
        {
            case "Log":
                anotherItem = prepareBoard + prepareTable;
                while (number + anotherItem > maxCarLoad) 
                {
                    number--;
                }
                sld[0].value = number;
                iField[0].text = number.ToString();
                prepareLog = number;
                break;
            case "Board":
                anotherItem = prepareLog + prepareTable;
                while (number + anotherItem > maxCarLoad)
                {
                    number--;
                }
                sld[1].value = number;
                iField[1].text = number.ToString();
                prepareBoard = number;
                break;
            case "Table":
                anotherItem = prepareLog + prepareBoard;
                int loadValue = 120;
                number *= loadValue;
                while (number + anotherItem > maxCarLoad)
                {
                    number-=loadValue;
                }
                sld[2].value = number/loadValue;
                iField[2].text = (number/loadValue).ToString();
                prepareTable = number;
                break;

        }
        SetPrepareLoad();
    }

    private void SetPrepareLoad()
    {
        int allLoad = prepareLog + prepareBoard + prepareTable;
        prepareLoad.value = allLoad;
        float value = allLoad * 100 / maxCarLoad;
        percent.text = value+" / 100%";
        prepareLoad.transform.GetChild(1).GetChild(0).GetComponent<Image>().color =
            value > 80 ? Color.red : Color.green;
    }

    public void ReadyAndSell()
    {
        if (prepareLog + prepareBoard + prepareTable > 0)
        {
            GMS.setLog(-prepareLog);
            GMS.setBoard(-prepareBoard);
            GMS.setTable(-(prepareTable / 120 * 180));
            GMS.ReadyAndSell(prepareLog, prepareBoard, prepareTable, waitingTime, gameObject);
        }
    }

    public void OpenPage(int idx)
    {
        page[0].SetActive(false);
        page[1].SetActive(false);
        page[2].SetActive(false);

        page[idx].SetActive(true);
    }

    public void BuySome(string product)
    {
        switch (product)
        {
            case "IronAxe":
                GMS.BuyAxe(2);
                break;
            case "ObsidianAxe":
                GMS.BuyAxe(3);
                break;
            case "LotteryTicket":
                if (GMS.GetGold() > 100)
                {
                    GMS.setGold(-100);
                    GMS.setLotteryTicket(1);
                    lottery.SetActive(true);
                }
                else
                {
                    GMS.sendErrorMsg("Not Enaugh Gold");
                }
                break;
        }
    }

    public void UpgradeCar(string item)
    {
        GMS.UpgradeCar(item);
        setAll();
    }
}