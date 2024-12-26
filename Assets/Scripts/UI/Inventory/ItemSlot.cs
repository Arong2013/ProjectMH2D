using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerDownHandler
{
    Action buttonClickAction;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amountText;
    public void SetSlot(Item item)
    {
        icon.sprite = item.icon;
        var itemData = item as CountableItem;
        amountText.text = itemData.Amount.ToString();
     }
    public void UpdataSlot(Item item)
    {
        SetSlot(item);  
    }
    public void ActionAdd(Action action)
    {
        buttonClickAction += action;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonClickAction?.Invoke();
    }
}
