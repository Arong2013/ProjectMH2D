using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class ItemDataLoader
{
    private readonly string dbPath = $"{Application.streamingAssetsPath}/ItemData.db";
    private readonly Dictionary<int, ItemData> loadedItems = new Dictionary<int, ItemData>();
    private readonly Dictionary<string, Sprite> itemSprites = new Dictionary<string, Sprite>(); 
    private readonly Dictionary<ThrowItemType,GameObject> throwItems = new Dictionary<ThrowItemType,GameObject>();

    private static ItemDataLoader instance = null;

    /// <summary>
    /// 싱글톤 인스턴스 반환
    /// </summary>
    public static ItemDataLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemDataLoader();
            }
            return instance;
        }
    }

    private ItemDataLoader()
    {
        LoadAllSprites();
        LoadAllItems();
        LoadAllThrowObj();
    }

    /// <summary>
    /// 모든 테이블의 아이템 데이터를 로드
    /// </summary>
    private void LoadAllItems()
    {
        foreach (ItemTableName tableName in Enum.GetValues(typeof(ItemTableName)))
        {
            LoadItemsFromTable(tableName);
        }
    }
    private void LoadAllSprites()
    {
        // Resources 경로에서 모든 Texture2D를 로드
        Texture2D[] textures = Resources.LoadAll<Texture2D>("ItemSprites"); // 리소스 경로: Resources/ItemSprites/
        foreach (Texture2D texture in textures)
        {
            string itemName = texture.name;
            if (!itemSprites.ContainsKey(itemName))
            {
                // Texture2D로부터 Sprite 생성
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f) // Pivot 설정: (0.5, 0.5)는 중심
                );

                itemSprites[itemName] = sprite;
            }
        }
    }

    /// <summary>
    /// 특정 테이블의 데이터를 로드
    /// </summary>
    private void LoadItemsFromTable(ItemTableName tableName)
    {
        string queryTableName = $"{tableName}";
        string connectionPath = GetConnectionPath();

        using (IDbConnection dbConnection = new SqliteConnection(connectionPath))
        {
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    string query = $"SELECT * FROM {queryTableName}";
                    dbCommand.CommandText = query;

                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ItemData item = CreateItem(tableName, reader);
                            if (item != null && !loadedItems.ContainsKey(item.ID))
                            {
                                loadedItems[item.ID] = item;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data from {queryTableName}: {ex.Message}");
            }
        }
    }
    private void LoadAllThrowObj()
    {
        GameObject[] throwObjs = Resources.LoadAll<GameObject>("ThrowItems");

        foreach (GameObject obj in throwObjs)
        {
            if (Enum.TryParse(obj.name, out ThrowItemType throwItemType))
            {
                if (!throwItems.ContainsKey(throwItemType))
                {
                    throwItems.Add(throwItemType, obj);
                }
            }
            else
            {
                Debug.LogWarning($"Enum 변환 실패: {obj.name}는 ThrowItemType에 없습니다.");
            }
        }
    }
    /// <summary>
    /// 특정 테이블의 데이터로 아이템 객체 생성
    /// </summary>
    private ItemData CreateItem(ItemTableName tableName, IDataReader reader)
    {
        switch (tableName)
        {
            case ItemTableName.MeleeWeaponData:
                return new MeleeWeaponData(reader);
            case ItemTableName.ThrowItemData:
                return new ThrowItemData(reader);
            case ItemTableName.RecoveryItemData:
                return new RecoveryItemData(reader);
            default:
                Debug.LogWarning($"Unknown table name: {tableName}");
                return null;
        }
    }

    /// <summary>
    /// DB 연결 경로 반환
    /// </summary>
    private string GetConnectionPath()
    {
        return $"URI=file:{dbPath.Replace("\\", "/")}";
    }

    /// <summary>
    /// ID로 아이템 데이터를 가져오기
    /// </summary>
    public ItemData GetItemDataByID(int itemID)
    {
        if (loadedItems.TryGetValue(itemID, out var itemData))
        {
            return itemData;
        }
        Debug.LogWarning($"Item with ID {itemID} not found!");
        return null;
    }

    public Sprite GetSpriteByName(string itemName)
    {
        if (itemSprites.TryGetValue(itemName, out var sprite))
        {
            return sprite;
        }
        Debug.LogWarning($"Sprite for item '{itemName}' not found!");
        return null;
    }

    public GameObject GetThrowOBJ(ThrowItemType throwItemType)
    {
        if (throwItems.TryGetValue(throwItemType, out GameObject throwObj))
        {
            return throwObj; // 요청된 타입의 GameObject 반환
        }
        else
        {
            Debug.LogWarning($"ThrowItemType {throwItemType}에 해당하는 GameObject가 없습니다.");
            return null; // 없는 경우 null 반환
        }
    }


}
/// <summary>
/// 테이블 이름 열거형
/// </summary>
public enum ItemTableName
{
    MeleeWeaponData,
    ThrowItemData,
    RecoveryItemData
}

/// <summary>
/// 기본 아이템 데이터 클래스
/// </summary>
