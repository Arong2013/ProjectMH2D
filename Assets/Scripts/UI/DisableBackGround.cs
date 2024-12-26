using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableBackGround : MonoBehaviour,IPointerDownHandler
{
    Stack<IDisableUI> disableGameObjs = new Stack<IDisableUI>();
    public void OnPointerDown(PointerEventData eventData)
    {
        if (disableGameObjs.Count > 0)
        {
            disableGameObjs.Peek().SetActiveFalse();
            disableGameObjs.Pop();
            return;
        }
    }
    public void SetOBJ(IDisableUI disableUI)
    {
        disableGameObjs.Push(disableUI);
    }
}
public interface IDisableUI
{
    public void SetActiveFalse();
}