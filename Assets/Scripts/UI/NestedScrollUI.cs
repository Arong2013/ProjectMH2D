using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class NestedScrollUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] int ViewSlotCount;

     private float[] pos;
     private float distance, curPos, targetPos;
     private int targetIndex;
    private int SIZE;

    protected bool isDrag;

    protected void  UpdataScroll(int size)
    {
        SIZE = size;
        pos = new float[SIZE];

        distance = 1f / (SIZE - ViewSlotCount);

        for (int i = 0; i < SIZE; i++)
        {
            if (SIZE - 1 - i < ViewSlotCount && ViewSlotCount >= 2)
            {
                pos[i] = 1f;
                continue;
            }
            pos[i] = distance * i;
        }
        curPos = scrollbar.value;
        targetPos = curPos;
        targetIndex = 0;
    }

    float FindClosestPos()
    {
        float closestDistance = Mathf.Infinity;
        float closestPos = 0;
        for (int i = 0; i < SIZE; i++)
        {
            float diff = Mathf.Abs(scrollbar.value - pos[i]);
            if (diff < closestDistance)
            {
                closestDistance = diff;
                targetIndex = i;
                closestPos = pos[i];
            }
        }
        return closestPos;
    }

    public  void OnBeginDrag(PointerEventData eventData)
    {
        curPos = FindClosestPos();
    }
    public  void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }
    private void UpdateScrollbarValue(int firstint)
    {
        float newScrollbarValue = pos[firstint] + (distance * 0.5f); 
        scrollbar.value = Mathf.Clamp01(newScrollbarValue);
    }
    public  void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = FindClosestPos();
        if (curPos == targetPos)
        {
            if (eventData.delta.x > 30 && targetIndex > 0) // Fast left drag
            {
                targetIndex--;
                targetPos = pos[targetIndex];
            }
            else if (eventData.delta.x < -30 && targetIndex < SIZE - 1) // Fast right drag
            {
                targetIndex++;
                targetPos = pos[targetIndex];
            }
        }

    }
    public void Update()
    {
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, Time.deltaTime * 10f);
            if (Mathf.Abs(scrollbar.value - targetPos) < 0.001f)
            {
                scrollbar.value = targetPos;
            }
        }
    }
}
