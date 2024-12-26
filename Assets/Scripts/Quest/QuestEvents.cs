using System;

public static class QuestEvents
{
    public static event Action<int> OnMonsterKilled;
    public static event Action<int, int> OnItemCollected;
    public static void MonsterKilled(int MonsterID)
    {
        OnMonsterKilled?.Invoke(MonsterID);
    }
    public static void ItemCollected(int ItemID,int ItemCount)
    {
        OnItemCollected?.Invoke(ItemID, ItemCount);
    }
}
