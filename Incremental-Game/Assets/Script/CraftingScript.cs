using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingScript : MonoBehaviour
{
    [SerializeField] private GameManagerScript GMS;
    [SerializeField] private GameObject page;
    private int log;
    private int board;

    public void ClickButton()
    {
        if (GMS.GetLogCutter())
        {
            page.transform.parent.gameObject.SetActive(true);
            page.SetActive(true);

            log = GMS.GetLog();
            board = GMS.GetBoard();
        }
        else
        {
            GMS.sendErrorMsg("Locked");
        }
    }

    public void CraftTable()
    {
        if (log >= 20 && board >=60)
        {
            GMS.setLog(-20);
            GMS.setBoard(-60);
            GMS.craftTable();
        }
        else
        {
            GMS.sendErrorMsg("Not Enaugh Material");
        }


        page.transform.parent.gameObject.SetActive(false);
        page.SetActive(false);
    }
}
