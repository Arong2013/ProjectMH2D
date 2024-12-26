using System;
using System.Collections.Generic;
using UnityEngine;
public static class Utils
{

    /// <summary>
    /// 드롭 테이블에서 확률적으로 아이템 ID를 반환합니다.
    /// </summary>
    /// <param name="dropTable">드롭 확률과 아이템 정보를 담고 있는 List<DropItemData></param>
    /// <returns>랜덤하게 선택된 아이템 ID (없으면 -1 반환)</returns>
    public static int GetRandomItemFromDropTable(List<DropItemData> dropTable)
    {
        if (dropTable == null || dropTable.Count == 0)
        {
            Debug.LogWarning("Drop table is null or empty!");
            return -1; // 유효하지 않은 경우
        }

        // 총 드롭 확률 계산
        float totalChance = 0f;
        foreach (var item in dropTable)
        {
            totalChance += item.DropRate;
        }

        // 랜덤 값 생성
        float randomValue = UnityEngine.Random.Range(0f, totalChance);

        // 랜덤 값에 따라 아이템 선택
        float cumulativeChance = 0f;
        foreach (var item in dropTable)
        {
            cumulativeChance += item.DropRate;
            if (randomValue <= cumulativeChance)
            {
                Debug.Log($"Selected Item: {item.ItemName} (ID: {item.ItemID}, DropRate: {item.DropRate})");
                return item.ItemID; // 선택된 아이템 ID 반환
            }
        }

        Debug.LogWarning("Failed to select an item from the drop table.");
        return -1; // 예상치 못한 경우
    }
    public static T GetUI<T>(string _name = null) where T : MonoBehaviour
    {
        T component = null;
        if (component == null)
        {
            component = FindInCanvasChildren<T>();
        }
        else if (component == null)
        {
            Debug.Log(component.name + " found in the current scene.");

        }
        return component;
    }
    private static T FindInCanvasChildren<T>() where T : MonoBehaviour
    {
        T component = null;
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            component = canvas.GetComponentInChildren<T>(true);
        }

        return component;
    }

    public static List<IPlayerUesableUI> SetPlayerMarcineOnUI()
    {
        var list = new List<IPlayerUesableUI>();
        GameObject canvas = GameObject.Find("Canvas");
        foreach (Transform child in canvas.GetComponentsInChildren<Transform>(true))
        {
            IPlayerUesableUI usableUI = child.GetComponent<IPlayerUesableUI>();
            if (usableUI != null)
            {
                Debug.Log(child.name);
                list.Add(usableUI);
            }
        }
        return list;
    }

}
