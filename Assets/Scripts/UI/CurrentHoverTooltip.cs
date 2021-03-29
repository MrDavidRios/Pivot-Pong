using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentHoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //GameObjects
    private GameObject lastHoveredButton;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        var selectedObject = pointerEventData.pointerCurrentRaycast.gameObject;

        if (selectedObject == gameObject)
        {
            GetComponent<Tooltip>().ActivateTooltip(true);
            lastHoveredButton = gameObject;
        }
        else
            lastHoveredButton = null;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (lastHoveredButton == gameObject)
            GetComponent<Tooltip>().ActivateTooltip(false);
    }
}