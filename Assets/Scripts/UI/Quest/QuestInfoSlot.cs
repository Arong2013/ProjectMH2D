using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfoSlot : MonoBehaviour
{
    public Action<QuestData> OnQuestAccepted;
    public Action<QuestData> OnQuestRejected;

    QuestData questData;
    [SerializeField] TextMeshProUGUI TypeName, Title, Content;
    [SerializeField] Button YesBtn, NoBtn;

    public void Init(QuestData questData)
    {
        this.questData = questData;
        TypeName.text = SetTypeName(questData);
        Title.text = questData.Name;
        Content.text = $"보수금: {questData.Reward}W\n" +
                       $"계약금: {questData.Deposit}W\n" +
                       $"제한시간: {questData.TimeLimit}분\n" +
                       $"중요 몬스터:\n";

        if (questData is KillQuestData killQuestData)
        {
            SetKillQuest(killQuestData);
        }

        YesBtn.onClick.AddListener(() => OnQuestAccepted?.Invoke(questData));
        NoBtn.onClick.AddListener(() => OnQuestRejected?.Invoke(questData));
    }

    public void SetKillQuest(KillQuestData questData)
    {
        foreach (var target in questData.TargetData)
        {
            int monsterId = target.Key;    // 몬스터 아이디
            int count = target.Value;     // 필요한 수량
            string monsterName = MonsterDataLoader.GetSingleton().GetMonsterBaseData(monsterId).Name;

            Content.text += $"{monsterName}  :{count}마리\n";
        }
    }

    public string SetTypeName(QuestData questData)
    {
        var type = questData.GetType();
        switch (type)
        {
            case Type when type == typeof(KillQuestData):
                return "수렵 퀘스트";
            default:
                return "무슨 퀘스트?";
        }
    }
}
