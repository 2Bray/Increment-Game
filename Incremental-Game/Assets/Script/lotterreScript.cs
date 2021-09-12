using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lotterreScript : MonoBehaviour
{
    [SerializeField] private Text TicketInfo;
    [SerializeField] private GameManagerScript GMS;
    [SerializeField] private GameObject feedBack;
    [SerializeField] private Button[] allButton;
    [SerializeField] private Transform spinPointer;


    private void OnEnable()
    {
        spinPointer.gameObject.SetActive(false);
        TicketInfo.text = "Lottery Ticket: " + GMS.GetLotteryTicket();
    }

    public void BuyTicket()
    {
        if (GMS.GetGold() >= 100) 
        { 
            GMS.setLotteryTicket(1);
            GMS.setGold(-100);

            ResultTextScript go = Instantiate(feedBack, transform.parent).GetComponent<ResultTextScript>();
            go.ovrdText("-100", Color.red, 28);
            TicketInfo.text = "Lottery Ticket: "+GMS.GetLotteryTicket();

        }
        else GMS.sendErrorMsg("Not Enaugh Gold");
    }

    public void Spin()
    {
        if (GMS.GetLotteryTicket() > 0)
        {
            allButton[0].interactable = false;
            allButton[1].interactable = false;
            allButton[2].interactable = false;

            ResultTextScript go = Instantiate(feedBack, transform.parent).GetComponent<ResultTextScript>();
            go.ovrdText("-1", Color.red, 32);
            GMS.setLotteryTicket(-1);
            TicketInfo.text = "Lottery Ticket: " + GMS.GetLotteryTicket();
            getReward();
        }
    }

    private void getReward()
    {
        string result="";
        int reward = Random.Range(0,100);
        if (reward < 2)
        {
            GMS.SetSilat();
            result = "SilatBook";
        }
        else if (reward < 12)
        {
            GMS.setLog(20);
            result = "20Log";
        }
        else if (reward < 32)
        {
            GMS.setLog(5);
            result = "5Log";
        }
        else if (reward < 42)
        {
            result = "5Board";
            GMS.setBoard(5);
        }
        else if (reward < 44)
        {
            result = "LogCutter";
            GMS.SetLogCutter();
        }
        else if (reward < 59)
        {
            result = "300Gold";
            GMS.setGold(300);
        }
        else if (reward < 69)
        {
            result = "10Board";
            GMS.setBoard(10);
        }
        else if (reward < 71)
        {
            result = "CraftsmanBook";
            GMS.SetCrafting();
        }
        else if (reward < 86)
        {
            result = "20Gold";
            GMS.setGold(20);
        }
        else if (reward < 96)
        {
            result = "1Table";
            GMS.setTable(1);
        }
        else if (reward < 98)
        {
            result = "10kGold";
            GMS.setGold(10000);
        }
        else if (reward < 100)
        {
            result = "1mLog";
            GMS.setLog(1000000);
        }
        StartCoroutine(movePointer(result));
    }


    private IEnumerator movePointer(string name)
    {
        spinPointer.gameObject.SetActive(true);
        Transform prt = transform.GetChild(1);
        for (int i = 0; i < prt.childCount-1; i++)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            spinPointer.localPosition = prt.GetChild(i).localPosition;
            if (prt.GetChild(i).name == name) break;
        }
        yield return new WaitForSecondsRealtime(0.2f);
        spinPointer.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.4f);
        spinPointer.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        spinPointer.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.2f);
        spinPointer.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        spinPointer.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.2f);
        spinPointer.gameObject.SetActive(true);

        allButton[0].interactable = true;
        allButton[1].interactable = true;
        allButton[2].interactable = true;
    }
}
