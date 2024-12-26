using UnityEngine;

public class QuestInfoScrollUI : NestedScrollUI
{
    [SerializeField] Transform slotparent;
    [SerializeField] QuestInfoSlot QuestInfoSlotPrefab;

    public void SetQuestInfoDatas(int Level, PlayerMarcine playerMarcine)
    {
        var questList = QuestDataLoader.GetSingleton().GetQuestsAtLevel(Level);

        foreach (var quest in questList)
        {
            QuestInfoSlot slotInstance = Instantiate(QuestInfoSlotPrefab, slotparent);
            slotInstance.Init(quest);

            slotInstance.OnQuestAccepted += playerMarcine.SetQuest;
            slotInstance.OnQuestRejected += questData => Debug.Log($"Quest Rejected: {questData.Name}");
        }

        UpdataScroll(questList.Count);
    }

    private void OnDisable()
    {
        foreach (Transform child in slotparent)
        {
            Destroy(child.gameObject);
        }
    }
}
