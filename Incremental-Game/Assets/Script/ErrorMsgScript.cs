using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMsgScript : MonoBehaviour
{
    public void Called(string msg)
    {
        transform.GetChild(0).GetComponent<Text>().text = msg;
        StartCoroutine(countDount());
    }

    private IEnumerator countDount()
    {
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }
}
