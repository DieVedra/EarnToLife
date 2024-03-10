using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CustomButton : Button
{
    public event Action OnButtonDown;
    public event Action OnButtonUp;
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnButtonDown?.Invoke();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        OnButtonUp?.Invoke();
    }
}
