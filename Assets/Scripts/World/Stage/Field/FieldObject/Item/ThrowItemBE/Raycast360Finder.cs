using UnityEngine;
using System;
using System.Collections.Generic;

public class Raycast360Finder
{
    private float maxDistance;
    private int rayCount;
    private LayerMask layerMask;
    private Transform originTransform;
    public Raycast360Finder(Transform origin, float maxDistance, int rayCount, LayerMask layerMask)
    {
        this.originTransform = origin;
        this.maxDistance = maxDistance;
        this.rayCount = rayCount;
        this.layerMask = layerMask;
    }
    public List<T> FindComponentsIn360Degrees<T>() where T : Component
    {
        List<T> foundComponents = new List<T>();

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * (360f / rayCount);
            Vector3 direction = AngleToDirectionXY(angle);

            if (Physics.Raycast(originTransform.position, direction, out RaycastHit hit, maxDistance, layerMask) && hit.transform != originTransform)
            {
                T component = hit.collider.GetComponent<T>();
                if (component != null && !foundComponents.Contains(component))
                {
                    foundComponents.Add(component);
                    Debug.Log($"Found {typeof(T).Name} at {hit.collider.gameObject.name}");
                }
            }
        }
        if (foundComponents.Count == 0)
        {
            Debug.Log("No objects found with the desired component.");
        }
        return foundComponents;
    }
    private Vector3 AngleToDirectionXY(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0); // XY 평면에서 방향 설정
    }
}
