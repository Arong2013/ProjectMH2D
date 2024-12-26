using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;
public class CharacterDataManager
{
    private readonly string dbPath = $"{Application.streamingAssetsPath}/CharacterData.db";
    private readonly Dictionary<int, CharacterData> preloadedCharacters = new Dictionary<int, CharacterData>();

    private static CharacterDataManager instance = null;

    public static CharacterDataManager GetSingleton()
    {
        if (instance == null)
        {
            instance = new CharacterDataManager();
        }
        return instance;
    }
    private CharacterDataManager()
    {
        LoadAllCharacterData();
    }

    private void LoadAllCharacterData()
    {
        string connectionPath = GetConnectionPath();
        Debug.Log($"Connecting to database at path: {connectionPath}");

        using (IDbConnection dbConnection = new SqliteConnection(connectionPath))
        {
            try
            {
                dbConnection.Open();
                Debug.Log("Database connection opened successfully.");

                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    string query = "SELECT * FROM CharacterInfo c LEFT JOIN CharacterBaseStats s ON c.CharacterID = s.CharacterID";


                    dbCommand.CommandText = query;

                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int characterID = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            int level = reader.GetInt32(2);
                            string description = reader.GetString(3);
                            var stats = new Dictionary<CharacterStatName, float>
    {
        { CharacterStatName.HP, reader.IsDBNull(5) ? 0 : reader.GetFloat(5) },
        { CharacterStatName.MaxHP, reader.IsDBNull(6) ? 0 : reader.GetFloat(6) },
        { CharacterStatName.SP, reader.IsDBNull(7) ? 0 : reader.GetFloat(7) },
        { CharacterStatName.MaxSP, reader.IsDBNull(8) ? 0 : reader.GetFloat(8) },
        { CharacterStatName.ATK, reader.IsDBNull(9) ? 0 : reader.GetFloat(9) },
        { CharacterStatName.DEF, reader.IsDBNull(10) ? 0 : reader.GetFloat(10) },
        { CharacterStatName.SPD, reader.IsDBNull(11) ? 0 : reader.GetFloat(11) }
    };

                            if (!preloadedCharacters.ContainsKey(characterID))
                            {
                                preloadedCharacters[characterID] = new CharacterData(name, level);
                            }
                            var character = preloadedCharacters[characterID];
                            foreach (var stat in stats)
                            {
                                character.SetBaseStat(stat.Key, stat.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load character data: {ex.Message}");
            }
            finally
            {
                dbConnection.Close();
                Debug.Log("Database connection closed.");
            }
        }
    }

    private string GetConnectionPath()
    {
        return $"URI=file:{dbPath.Replace("\\", "/")}";
    }

    public CharacterData GetCharacterData(int characterID)
    {
        if (preloadedCharacters.TryGetValue(characterID, out var characterData))
        {
            return characterData;
        }
        Debug.LogWarning($"Character with ID {characterID} not found!");
        return null;
    }
}
