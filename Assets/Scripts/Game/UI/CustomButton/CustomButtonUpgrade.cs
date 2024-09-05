using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonUpgrade : Button
{
    public event Action OnSelectUpgrade;
    public event Action OnDeselectUpgrade;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnSelectUpgrade?.Invoke();
        Debug.Log("OnSelect ");
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        OnDeselectUpgrade?.Invoke();
        Debug.Log("OnDeselect");

    }
}
