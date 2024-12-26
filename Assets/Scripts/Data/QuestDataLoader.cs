using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;

public enum QuestDataType
{
    KillQuest
}
public class QuestDataLoader
{
    private readonly string dbPath = $"{Application.streamingAssetsPath}/QuestData.db";

    private readonly Dictionary<int, QuestData> QuestDatas = new Dictionary<int, QuestData>();

    private static QuestDataLoader instance = null;

    public static QuestDataLoader GetSingleton()
    {
        if (instance == null)
        {
            instance = new QuestDataLoader();
        }
        return instance;
    }
    private QuestDataLoader()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadQuestFromTable(QuestDataType.KillQuest);
        Debug.Log("All data loaded successfully!");
    }

    private void LoadQuestFromTable(QuestDataType tableName)
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
                            switch (tableName)
                            {
                                case QuestDataType.KillQuest:
                                    LoadKillQuest(reader);
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

    private void LoadKillQuest(IDataReader reader)
    {
        int ID = reader.GetInt32(1);
        QuestDatas.Add(ID, new KillQuestData(reader));
    }

    public QuestData GetQuestData(int QuestID)
    {
        if (QuestDatas.TryGetValue(QuestID, out QuestData characterData))
        {
            return characterData;
        }
        Debug.LogWarning($"MonsterBaseData not found for MonsterID: {QuestID}");
        return null;
    }
    public List<QuestData> GetQuestsAtLevel(int level)
    {
        List<QuestData> questsAtLevel = new List<QuestData>();

        Debug.Log(QuestDatas.Count);
        foreach (var quest in QuestDatas.Values)
        {
            if (quest.Level == level)
            {
                questsAtLevel.Add(quest);
            }
        }
        return questsAtLevel;
    }

    private string GetConnectionPath()
    {
        return $"URI=file:{dbPath.Replace("\\", "/")}";
    }
}
