using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogCutterScript : MonoBehaviour
{
    [SerializeField] private GameManagerScript GMS;
    [SerializeField] private GameObject page;
    [SerializeField] private Slider slid;
    [SerializeField] private Text number;
    private int preLog;


    public void ClickButton()
    {
        if (GMS.GetLogCutter())
        {
            page.transform.parent.gameObject.SetActive(true);
            page.SetActive(true);
            slid.maxValue = GMS.GetLog();
            number.text = "x0";
        } 
        else
        {
            GMS.sendErrorMsg("Locked");
        }
    }

    public void setLogToBoard(float flt)
    {
        preLog = Mathf.FloorToInt(flt);
        number.text = "x"+preLog;
    }

    public void craft()
    {
        while (preLog % 3 != 0 && preLog>0) 
        {
            preLog--;
            number.text = "x" + preLog;
        }
        if (preLog > 0)
        {
            GMS.setLog(-preLog);
            GMS.StartCoroutine(GMS.CraftingBoard(preLog));

            page.transform.parent.gameObject.SetActive(false);
            page.SetActive(false);
        }
    }
}
