using System.Collections.Generic;
using UnityEngine.PlayerLoop;


public class PlayerQuestHandler
{
    List<Quest> quests; 
    public PlayerQuestHandler(PlayerData playerData)
    {
        quests =  playerData.GetRunningQuest();
    }
    public void SetQuest(QuestData questdata)
    {
        var quest = questdata.CreatQuest();
        quests.Add(quest);
        quest.LinkEvents();
    }
    public void UpdataQuest(params object[] objects)
    {

    }
}
