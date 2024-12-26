using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    [SerializeField] private float scroll_pos = 0;
    [SerializeField] private float[] pos;

    void Start()
    {
        // Initialize position array based on the number of child elements
        int childCount = transform.childCount;
        pos = new float[childCount];
        float distance = 1f / (childCount-1);

        for (int i = 0; i < childCount; i++)
        {
            pos[i] = distance * i;
            pos[i] += 0.05f;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Update the current scroll position while the user is dragging
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            // Find the closest position to snap to
            float closestPos = pos[0];
            float minDistance = 1;

            for (int i = 1; i < pos.Length; i++)
            {
                float distance = Mathf.Abs(scroll_pos - pos[i]);
                if (distance < minDistance)
                {
                    closestPos = pos[i];
                    minDistance = distance;
                }
            }
            scrollbar.GetComponent<Scrollbar>().value = closestPos;
        }
    }
}
