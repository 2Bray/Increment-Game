using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class childDisableScript : MonoBehaviour
{
    private int replaced = 0;

    public void setAwake(string str, Color clr)
    {
        replaced++;
        GetComponent<Text>().text = str;
        GetComponent<Text>().color = clr;
        StartCoroutine(countDount());
    }

    private IEnumerator countDount()
    {
        yield return new WaitForSecondsRealtime(replaced);
        gameObject.GetComponent<Text>().text = "";
        replaced = 0;
    }
}
