using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;

public enum MonsterDataType
{
    MonsterData,
    MonsterPartData,
    DropTables
}
public class MonsterDataLoader
{
    private readonly string dbPath = $"{Application.streamingAssetsPath}/MonsterData.db";

    private readonly Dictionary<int, CharacterData> MonsterBaseData = new Dictionary<int, CharacterData>();
    private readonly Dictionary<int, Dictionary<int,MonsterPartData>> MonsterPartData = new Dictionary<int, Dictionary<int, MonsterPartData>>();

    private static MonsterDataLoader instance = null;

    public static MonsterDataLoader GetSingleton()
    {
        if (instance == null)
        {
            instance = new MonsterDataLoader();
        }
        return instance;
    }
    private MonsterDataLoader()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadItemsFromTable(MonsterDataType.MonsterData);
        LoadItemsFromTable(MonsterDataType.MonsterPartData);
        LoadItemsFromTable(MonsterDataType.DropTables);

        Debug.Log("All data loaded successfully!");
    }

    private void LoadItemsFromTable(MonsterDataType tableName)
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
                           switch(tableName)
                            {
                                case MonsterDataType.MonsterData:
                                    LoadMonsterData(reader);
                                    break;
                                case MonsterDataType.MonsterPartData:
                                    LoadMonsterPartData(reader);
                                    break;
                                case MonsterDataType.DropTables:
                                    break;
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
    private void LoadMonsterData(IDataReader reader)
    {
        int monsterID = reader.GetInt32(0); // MonsterID
        string monsterName = reader.GetString(1); // MonsterName
        float baseHP = reader.GetFloat(2); // BaseHP
        float baseMaxHP = reader.GetFloat(3); // BaseHP
        float baseDamage = reader.GetFloat(4); // BaseDamage
        float baseSPD = reader.GetFloat(5); // BaseDamage

        CharacterData characterData = new CharacterData(monsterName, monsterID);
        characterData.SetBaseStat(CharacterStatName.HP, baseHP);
        characterData.SetBaseStat(CharacterStatName.MaxHP, baseMaxHP);
        characterData.SetBaseStat(CharacterStatName.ATK, baseDamage);
        characterData.SetBaseStat(CharacterStatName.SPD, baseSPD);

        MonsterBaseData[monsterID] = characterData;
    }


    private void LoadMonsterPartData(IDataReader reader)
    {
        int monsterID = reader.GetInt32(0); // MonsterID
        int partID = reader.GetInt32(1);   // PartID
        string partName = reader.GetString(2); // PartName
        float hp = reader.GetFloat(3);     // HP
        float disDMG = reader.GetFloat(4); // DisDMG

        MonsterPartData partData = new MonsterPartData
        {
            PartID = partID,
            PartName = partName,
            HP = hp,
            DisDMG = disDMG
        };
        if (!MonsterPartData.ContainsKey(monsterID))
        {
            MonsterPartData[monsterID] = new Dictionary<int, MonsterPartData>();
        }
        Debug.Log(partID);
        MonsterPartData[monsterID][partID] = partData;
    }
    private void LoadDropTableData(IDataReader reader)
    {
        int partID = reader.GetInt32(0);     // PartID
        int itemID = reader.GetInt32(1);     // ItemID
        string itemName = reader.GetString(2); // ItemName
        float dropRate = reader.GetFloat(3);  // DropRate

        DropItemData dropItem = new DropItemData
        {
            ItemID = itemID,
            ItemName = itemName,
            DropRate = dropRate
        };
        foreach (var monsterParts in MonsterPartData.Values)
        {
            if (monsterParts.ContainsKey(partID))
            {
                monsterParts[partID].DropItems.Add(dropItem);
            }
        }
    }
    public CharacterData GetMonsterBaseData(int monsterID)
    {
        if (MonsterBaseData.TryGetValue(monsterID, out CharacterData characterData))
        {
            return characterData;
        }
        Debug.LogWarning($"MonsterBaseData not found for MonsterID: {monsterID}");
        return null;
    }
    public MonsterPartData GetMonsterPartData(int monsterID, int partID)
    {
        if (MonsterPartData.TryGetValue(monsterID, out var parts) && parts.TryGetValue(partID, out MonsterPartData partData))
        {
            return partData;
        }
        Debug.LogWarning($"MonsterPartData not found for MonsterID: {monsterID}, PartID: {partID}");
        return null;
    }
    private string GetConnectionPath()
    {
        return $"URI=file:{dbPath.Replace("\\", "/")}";
    }
}
