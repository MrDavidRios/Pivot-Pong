using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Programmed with World Space UI in mind and assuming screen bounds won't be an issue.

    //Booleans
    public bool displayTooltip;

    //GameObjects
    public GameObject tooltip;
    private GameObject _lastHoveredButton;

    private void Awake() => ActivateTooltip(false);

    private void Update()
    {
        if (tooltip.activeInHierarchy && !displayTooltip)
            ActivateTooltip(false);
    }

    public void ActivateTooltip(bool activate)
    {
        if (activate && displayTooltip)
            tooltip.SetActive(true);
        else
            tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        var selectedObject = pointerEventData.pointerCurrentRaycast.gameObject;

        if (selectedObject == gameObject)
        {
            GetComponent<ExtraTooltip>().ActivateTooltip(true);
            _lastHoveredButton = gameObject;
        }
        else
            _lastHoveredButton = null;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (_lastHoveredButton == gameObject)
            GetComponent<ExtraTooltip>().ActivateTooltip(false);
    }
}