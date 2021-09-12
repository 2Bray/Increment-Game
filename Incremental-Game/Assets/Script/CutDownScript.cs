using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutDownScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameManagerScript GMS;

    public void OnPointerClick(PointerEventData eventData)
    {
        GMS.CutDown();
    }
}
