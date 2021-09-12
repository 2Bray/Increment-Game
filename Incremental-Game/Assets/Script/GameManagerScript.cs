using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private Transform canva;
    [SerializeField] private AxeScript axeScript;
    [SerializeField] private Transform panel;
    [SerializeField] private Text goldStat;
    private childDisableScript goldChild;
    [SerializeField] private GameObject errorMsg;
    [SerializeField] private Button[] coolDown;
    [SerializeField] private GameObject[] resultStat;

    private int gold = 0;
    private int log = 0;
    private int board = 0;
    private int table = 0;
    private int durability;
    private int carStorageLvl;
    private int carSpeedLvl;
    private int lotteryTicket;


    private bool logCutter = false;
    private bool crafting = false;
    private bool silat = false;
    private bool free = false;

    public string[] listAxe = {"null","Wood","Iron","Obsidian"};


    private void Start()
    {
        durability = -1;
        axeScript.ChangeAxe(0);
        goldStat.text = gold.ToString();
        goldChild = goldStat.transform.GetChild(0).GetComponent<childDisableScript>();
    }

    public void setGold(int i)
    {
        gold += i;
        goldStat.text = gold.ToString();
        string nominal = "";
        Color clr = new Color();
        if (i > 0)
        {
            nominal = "+";
            clr = Color.black;
        }
        else 
        { 
            nominal = "-";
            clr = Color.red;
        }
        goldChild.setAwake(nominal+i,clr);
    }

    public int GetGold() => gold;
    public int GetLog() => log;
    public void setLog(int i) => log += i;
    public int GetBoard() => board;
    public void setBoard(int i) => board += i;
    public int GetTable() => table;
    public void setTable(int i) => table += i;
    public int GetLotteryTicket() => lotteryTicket;
    public void setLotteryTicket(int i) => lotteryTicket += i;
    public void SetSilat() => silat = true;
    public void SetLogCutter() => logCutter = true;
    public bool GetLogCutter() => logCutter;
    public void SetCrafting() => crafting = true;
    public bool GetCrafting() => crafting;
    public int GetCarStorageLvl()
    {
        int result = 0;
        switch (carStorageLvl)
        {
            case 0:
                result = 60;
                break;
            case 1:
                result = 90;
                break;
            case 2:
                result = 150;
                break;
            case 3:
                result = 280;
                break;
            case 4:
                result = 400;
                break;
        }
        return result;
    }
    public int GetCarSpeedLvl()
    {
        int result = 0;
        switch (carSpeedLvl)
        {
            case 0:
                result = 120;
                break;
            case 1:
                result = 110;
                break;
            case 2:
                result = 90;
                break;
            case 3:
                result = 60;
                break;
            case 4:
                result = 20;
                break;
        }
        return result;
    }

    public void CutDown()
    {
        if (durability < 0) durability = axeScript.GetNewAxe();
        else durability = axeScript.CutDown(durability, silat, this,canva);
    }

    public void BuyAxe(int idx)
    {
        switch (idx)
        {
            case 1:
                if (GetLog() >= 3) setLog(-3);
                else
                {
                    GameObject go = Instantiate(errorMsg, canva);
                    go.GetComponent<ErrorMsgScript>().Called("Not Enaugh Material");
                    return;
                }
                panel.GetChild(0).gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                break;
            case 2:
                if (GetGold()>=300) setGold(-300);
                else
                {
                    GameObject go = Instantiate(errorMsg, panel);
                    go.GetComponent<ErrorMsgScript>().Called("Not Enaugh Gold");
                    return;
                }
                break;
            case 3:
                if (GetGold() >= 1000) setGold(-1000);
                else
                {
                    GameObject go = Instantiate(errorMsg, panel);
                    go.GetComponent<ErrorMsgScript>().Called("Not Enaugh Gold");
                    return;
                }
                break;
        }
        axeScript.ChangeAxe(idx);
    }

    public void ClickStorage()
    {
        GameObject storage = panel.GetChild(1).gameObject;
        storage.SetActive(true);
        storage.GetComponent<StorageScript>().OpenStorage(log,board,table);
        panel.gameObject.SetActive(true);
    }

    public void ReadyAndSell(int pLog, int pBoar, int pTable, int time, GameObject chld)
    {
        startCoolDownButton(coolDown[0], time);
        StartCoroutine(CarGoingSell(pLog,pBoar,pTable,time, time, coolDown[0].transform.GetChild(1).GetComponent<Slider>()));

        chld.SetActive(false);
        panel.gameObject.SetActive(false);
    }

    private IEnumerator CarGoingSell(int pLog, int pBoar, int pTable, int T, int Tmax,Slider progress)
    {
        yield return new WaitForSecondsRealtime(1);
        T--;
        if (T<0)
        {
            int recieve = 0;
            recieve += pLog;
            recieve += pBoar;
            recieve += pTable / 120 * 180;
            setGold(recieve);
            endCooldownButton(coolDown[0]);
        } else
        {
            progress.value = Tmax - T;
            StartCoroutine(CarGoingSell(pLog, pBoar, pTable, T, Tmax, progress));
        }
    }

    public void setUpgradePrice(string item, Text lvl,Text prc)
    {
        string result = "";
        switch (item)
        {
            case "storage":
                switch (carStorageLvl)
                {
                    case 0:
                        result = "500";
                        break;
                    case 1:
                        result = "1200";
                        break;
                    case 2:
                        result = "3360";
                        break;
                    case 3:
                        result = "8736";
                        break;
                    default:
                        result = "Max";
                        break;
                }
                lvl.text = "Lvl "+(carStorageLvl + 1);
                break;
            case "time":
                switch (carSpeedLvl)
                {
                    case 0:
                        result = "500";
                        break;
                    case 1:
                        result = "1200";
                        break;
                    case 2:
                        result = "3360";
                        break;
                    case 3:
                        result = "8736";
                        break;
                    default:
                        result = "Max";
                        break;
                }
                lvl.text = "Lvl "+(carSpeedLvl + 1);
                break;
        }
        prc.text = result;
    }

    public void UpgradeCar(string item)
    {
        int price = 0;
        switch (item)
        {
            case "UpStorage":
                switch (carStorageLvl)
                {
                    case 0:
                        price = 500;
                        break;
                    case 1:
                        price = 1200;
                        break;
                    case 2:
                        price = 3360;
                        break;
                    case 3:
                        price = 8736;
                        break;
                    default:
                        price = 0;
                        break;
                }
                if (price > 0 && price <= GetGold())
                {
                    carStorageLvl++;
                    setGold(-price);
                }
                break;
            case "UpTime":
                switch (carSpeedLvl)
                {
                    case 0:
                        price = 500;
                        break;
                    case 1:
                        price = 1200;
                        break;
                    case 2:
                        price = 3360;
                        break;
                    case 3:
                        price = 8736;
                        break;
                    default:
                        price = 0;
                        break;
                }
                if (price > 0 && price <= GetGold())
                {
                    carSpeedLvl++;
                    setGold(-price);
                }
                break;
        }
        
    }

    public IEnumerator CraftingBoard(int log)
    {
        for (int i = 0; i < log / 3; i++)
        {
            yield return new WaitUntil(() => free);
            free = false;
            startCoolDownButton(coolDown[1], 20);
            StartCoroutine(progressCraftBoard(20, 20, coolDown[1].transform.GetChild(1).GetComponent<Slider>()));
        }
    }

    private IEnumerator progressCraftBoard(int T, int Tmax, Slider progress)
    {
        yield return new WaitForSecondsRealtime(1);
        T--;
        if (T < 0)
        {
            int r = Random.Range(0, 100);
            if (r < 59)
            {
                setBoard(10);
                ResultTextScript t = Instantiate(resultStat[0], progress.transform.parent).GetComponent<ResultTextScript>();
                t.ovrdText("+10", Color.black, 32);
            }
            else
            {
                ResultTextScript t = Instantiate(resultStat[0], progress.transform.parent).GetComponent<ResultTextScript>();
                t.ovrdText("Failed", Color.red, 24);
            }
            free = true;
            endCooldownButton(coolDown[1]);
        }
        else
        {
            progress.value = Tmax - T;
            StartCoroutine(progressCraftBoard (T, Tmax, progress));
        }
    }

    public void craftTable()
    {
        startCoolDownButton(coolDown[2], 60);
        StartCoroutine(progressTableCrafting(60, 60, coolDown[2].transform.GetChild(1).GetComponent<Slider>()));
    }

    private IEnumerator progressTableCrafting(int T, int Tmax, Slider progress)
    {
        yield return new WaitForSecondsRealtime(1);
        T--;
        if (T < 0)
        {
            if (Random.Range(0, 100) < 69)
            {
                setTable(1);
                ResultTextScript t = Instantiate(resultStat[0], progress.transform.parent).GetComponent<ResultTextScript>();
                t.ovrdText("+1", Color.black, 32);
            }
            else
            {
                ResultTextScript t = Instantiate(resultStat[0], progress.transform.parent).GetComponent<ResultTextScript>();
                t.ovrdText("Broken", Color.red, 24);
            }

            endCooldownButton(coolDown[2]);
        }
        else
        {
            progress.value = Tmax - T;
            StartCoroutine(progressTableCrafting(T, Tmax, progress));
        }
    }

    public void sendErrorMsg(string str)
    {
        GameObject go = Instantiate(errorMsg, canva);
        go.GetComponent<ErrorMsgScript>().Called(str);
    }

    public void startCoolDownButton(Button b, int time)
    {
        b.interactable = false;
        Slider progresSlider = b.transform.GetChild(1).GetComponent<Slider>();
        progresSlider.gameObject.SetActive(true);
        progresSlider.value = 0;
        progresSlider.maxValue = time;
    }

    public void endCooldownButton(Button b)
    {
        b.transform.GetChild(1).gameObject.SetActive(false);
        b.interactable = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}