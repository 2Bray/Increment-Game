using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageScript : MonoBehaviour
{
    [SerializeField] private Text[] allChild;

    public void OpenStorage(int log, int board, int table)
    {
        allChild[0].text = "x"+log;
        allChild[1].text = "x"+board;
        allChild[2].text = "x"+table;
    }
}
