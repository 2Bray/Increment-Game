using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxeScript : MonoBehaviour
{
    [SerializeField] private Sprite[] allSpriteAxe;
    [SerializeField] private Image icon;
    [SerializeField] private Slider durabilityStat;
    [SerializeField] private GameObject[] result;

    private int axe;
    private int percent;
    private int sendNewAxe = -1;
    private bool silat = false;
    public int GetAxe() => axe;

    public int CutDown(int durability, bool silat,GameManagerScript GMS, Transform canva)
    {
        this.silat = silat;
        if (durability >= 0)
        {
            durability--;
            int res = percent > Random.Range(0, 100) ? 1 : 0;
            GMS.setLog(res);
            Instantiate(result[res], canva);
        }
        if (durability == 0)
        {
            durability--;
            StartCoroutine(Recorvery(0));
        }
        return durability;
    }

    public int GetNewAxe()
    {
        int send = sendNewAxe;
        if (sendNewAxe > 0) 
        {
            sendNewAxe = -1;
        }
        return send;
    }

    public void ChangeAxe(int i)
    {
        axe = i;
        icon.sprite = allSpriteAxe[i];
        int durability=0;
        switch (i)
        {
            case 0:
                durability = 3;
                percent = 59;
                break;
            case 1:
                durability = 10;
                percent = 69;
                break;
            case 2:
                durability = 500;
                percent = 79;
                break;
            case 3:
                durability = 2000;
                percent = 89;
                break;
        }
        sendNewAxe = durability;
        durabilityStat.maxValue = durability;
        durabilityStat.value = durability;
        durabilityStat.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.green;
    }

    private IEnumerator Recorvery(int T)
    {
        GetComponent<Button>().interactable = false;
        durabilityStat.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        int finish = silat ? 0 : 20;
        durabilityStat.maxValue = finish;
        durabilityStat.value = T;
        yield return new WaitForSecondsRealtime(0.1f);
        T++;
        if (T <= finish) StartCoroutine(Recorvery(T));
        else
        {
            GetComponent<Button>().interactable = true;
            ChangeAxe(0);
        }
    }
}
