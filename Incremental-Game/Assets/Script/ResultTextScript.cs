using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(countDount(5));
    }

    public void ovrdText(string s, Color clr, int size)
    {
        Text me = transform.GetComponent<Text>();

        me.text = s;
        me.color = clr;
        me.fontSize = size;
    }

    private IEnumerator countDount(int i)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (i < 0) Destroy(gameObject);
        transform.Translate(Vector2.up*10);
        i--;
        StartCoroutine(countDount(i));
    }
}
