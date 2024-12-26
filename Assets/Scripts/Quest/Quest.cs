using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class QuestData
{
    public readonly int Level,ID, Reward, Deposit, TimeLimit;
    public readonly string Name;

    public QuestData(IDataReader reader)
    {
        this.Level = reader.GetInt32(0);
        this.ID = reader.GetInt32(1);
        this.Name = reader.GetString(2);
        this.Reward = reader.GetInt32(3);
        this.Deposit = reader.GetInt32(4);
        this.TimeLimit = reader.GetInt32(5);
    }
    public abstract Quest CreatQuest();
}
public class KillQuestData : QuestData
{
    public readonly SerializableDictionary<int, int> TargetData;   
    public KillQuestData(IDataReader reader) : base(reader)
    {

        string TargetIDsRaw = reader.GetString(6);
        string TargetCountsRaw = reader.GetString(7);

        string[] targetIDs = TargetIDsRaw.Split(',');
        string[] targetCounts = TargetCountsRaw.Split(',');

        SerializableDictionary<int, int> targetData = new SerializableDictionary<int, int>();

        targetIDs = Array.FindAll(targetIDs, id => !string.IsNullOrWhiteSpace(id));
        targetCounts = Array.FindAll(targetCounts, count => !string.IsNullOrWhiteSpace(count));

        if (targetIDs.Length != targetCounts.Length)
        {
            Debug.LogError($"Data mismatch: TargetIDs ({targetIDs.Length}) and TargetCounts ({targetCounts.Length})");
            return;
        }

        for (int i = 0; i < targetIDs.Length; i++)
        {
            try
            {
                int targetID = int.Parse(targetIDs[i].Trim()); // 빈 값이 없다고 가정
                int targetCount = int.Parse(targetCounts[i].Trim());

                targetData[targetID] = targetCount;
            }
            catch (FormatException ex)
            {
                Debug.LogError($"Invalid format in TargetIDs or TargetCounts at index {i}: {ex.Message}");
            }
        }
        this.TargetData = targetData;
    }
    public override Quest CreatQuest()
    {
        return new KillQuest(ID);
    }
}
public abstract class Quest
{
    QuestData QuestData => QuestDataLoader.GetSingleton().GetQuestData(ID);
    protected int ID;
    public Quest(int ID)
    {
        this.ID = ID;
    }
    public abstract bool IsComplete();

    public abstract void LinkEvents();
    public T GetQuestData<T>()
        where T : QuestData
    {
        return QuestData as T;
    }
}

public class KillQuest : Quest
{
    public readonly SerializableDictionary<int, int> TargetData;
    private SerializableDictionary<int, int> CurrentKillCounts;

    public KillQuest(int ID) : base(ID) 
    {
        CurrentKillCounts = new SerializableDictionary<int, int>(); 
        TargetData = GetQuestData<KillQuestData>().TargetData;
        foreach (var target in TargetData)
        {
            CurrentKillCounts[target.Key] = 0;
        }
        
    }
    public override bool IsComplete()
    {
        foreach (var target in TargetData)
        {
            int targetId = target.Key;
            int requiredKillCount = target.Value;

            if (!CurrentKillCounts.ContainsKey(targetId) || CurrentKillCounts[targetId] < requiredKillCount)
            {
                return false;
            }
        }

        return true; 
    }

    public override void LinkEvents()
    {
        QuestEvents.OnMonsterKilled += Update;
    }
    public void Update(int monsterID)
    {
        if (CurrentKillCounts.ContainsKey(monsterID))
        {
            CurrentKillCounts[monsterID]++;
            if (IsComplete())
            {
            }
        }
    }
}
public class QuestIterator : IIterator<Quest>
{
    private List<Quest> _quests;
    private int _position;

    public QuestIterator(List<Quest> quests)
    {
        _quests = quests;
        _position = 0;
    }

    public bool HasNext()
    {
        while (_position < _quests.Count)
        {
            if (!_quests[_position].IsComplete())
            {
                return true;
            }
            _position++;
        }
        return false;
    }

    public Quest Next()
    {
        return _quests[_position++];
    }
}
public class QuestCollection : IAggregate<Quest>
{
    private List<Quest> _quests;

    public QuestCollection(List<Quest> quests)
    {
        _quests = quests;
    }

    public IIterator<Quest> CreateIterator()
    {
        return new QuestIterator(_quests);
    }
}
