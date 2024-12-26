using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public readonly int ID;
    public readonly string Name;
    public int Level;
    public Vector3 position;

    [SerializeField]
    private Dictionary<CharacterStatName, float> baseStats = new Dictionary<CharacterStatName, float>();
    [NonSerialized]
    private Dictionary<CharacterStatName, Dictionary<object, float>> updatedStats = new Dictionary<CharacterStatName, Dictionary<object, float>>();

    public CharacterData(string name, int level)
    {
        Name = name;
        Level = level;
        foreach (CharacterStatName stat in Enum.GetValues(typeof(CharacterStatName)))
        {
            baseStats[stat] = 0;
        }
    }

    public static CharacterData CreatPlayerData()
    {
        var data = new CharacterData("Player", 1);
        data.SetBaseStat(CharacterStatName.HP, 100);
        data.SetBaseStat(CharacterStatName.MaxHP, 100);
        data.SetBaseStat(CharacterStatName.SP, 100);
        data.SetBaseStat(CharacterStatName.MaxSP, 100);
        data.SetBaseStat(CharacterStatName.ATK, 20);
        data.SetBaseStat(CharacterStatName.DEF, 20);
        data.SetBaseStat(CharacterStatName.SPD, 3);
        data.SetBaseStat(CharacterStatName.RollSP, 20);
        return data;
    }
    public void SetBaseStat(CharacterStatName statName, float value)
    {
        if (baseStats.ContainsKey(statName))
        {
            baseStats[statName] = value;
            Debug.Log($"{statName}={value}");
        }
    }

    public void UpdateBaseStat(CharacterStatName statName, float value)
    {
        if (baseStats.ContainsKey(statName))
        {
            baseStats[statName] += value;
            if (statName == CharacterStatName.HP)
            {
                baseStats[statName] = Mathf.Min(baseStats[statName], GetStat(CharacterStatName.MaxHP));
            }
            else if (statName == CharacterStatName.SP)
            {
                baseStats[statName] = Mathf.Min(baseStats[statName], GetStat(CharacterStatName.MaxSP));
            }
        }
    }
    public void UpdateStat(CharacterStatName statName, object source, float value)
    {
        if (!updatedStats.ContainsKey(statName))
        {
            updatedStats[statName] = new Dictionary<object, float>();
        }
        if (!updatedStats[statName].ContainsKey(source))
        {
            updatedStats[statName][source] = 0;
        }

        updatedStats[statName][source] += value;

        if (statName == CharacterStatName.HP)
        {
            baseStats[statName] = Mathf.Min(baseStats[statName], GetStat(CharacterStatName.MaxHP));
        }
        else if (statName == CharacterStatName.SP)
        {
            baseStats[statName] = Mathf.Min(baseStats[statName], GetStat(CharacterStatName.MaxSP));
        }
    }



    public void ChangeStat(CharacterStatName statName, object source, float value)
    {
        if (!updatedStats.ContainsKey(statName))
        {
            updatedStats[statName] = new Dictionary<object, float>();
        }
        updatedStats[statName][source] = value;
    }

    public float GetStat(CharacterStatName statName)
    {
        float baseValue = baseStats.ContainsKey(statName) ? baseStats[statName] : 0;

        if (updatedStats.ContainsKey(statName))
        {
            foreach (var bonus in updatedStats[statName].Values)
            {
                baseValue += bonus;
            }
        }

        return baseValue;
    }
}
