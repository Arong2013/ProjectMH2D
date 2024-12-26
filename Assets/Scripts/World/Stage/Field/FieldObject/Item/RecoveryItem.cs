using System;
using System.Collections.Generic;
using System.Data;

public class RecoveryItemData : CountableItemData
{
    public readonly Dictionary<CharacterStatName, float> recoveryStats = new Dictionary<CharacterStatName, float>();

    public RecoveryItemData(IDataReader reader):base(reader)
    {
        foreach (CharacterStatName stat in Enum.GetValues(typeof(CharacterStatName)))
        {
            float statValue = reader.GetFloat((int)stat + 5); // 스탯 컬럼은 5번째 이후부터 시작
            if (statValue > 0)
            {
                SetRecoveryStat(stat, statValue);
            }
        }
    }
    public void SetRecoveryStat(CharacterStatName statName, float amount)
    {
        if (recoveryStats.ContainsKey(statName))
        {
            recoveryStats[statName] = amount;
        }
        else
        {
            recoveryStats.Add(statName, amount);
        }
    }
    public float GetRecoveryStat(CharacterStatName statName)
    {
        return recoveryStats.ContainsKey(statName) ? recoveryStats[statName] : 0;
    }
    public Dictionary<CharacterStatName, float> GetAllRecoveryStats()
    {
        return new Dictionary<CharacterStatName, float>(recoveryStats);
    }
    public override Item CreatItem()
    {
        return new RecoveryItem(this);
    }
}


public class RecoveryItem: CountableItem
{
    public Dictionary<CharacterStatName, float> recoveryStats => GetStat();
    public RecoveryItem(ItemData itemData) : base(itemData) { }
    public Dictionary<CharacterStatName, float> GetStat()
    {
        var data = ItemData as RecoveryItemData;
        return data.GetAllRecoveryStats();
    }

    public void Use(PlayerMarcine playerMarcine)
    {
       
    }
}