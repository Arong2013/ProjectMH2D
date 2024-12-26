using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class QuestCollectionUI : MonoBehaviour, IPointerDownHandler, IPlayerUesableUI
{
    [SerializeField] Transform LevelParent;
    [SerializeField] QuestInfoScrollUI InfoUI;
    private PlayerMarcine playerMarcine;
    private enum UIState { LevelSelection, InfoScroll, Hidden }
    private UIState currentState;

    private void Start()
    {
        for (int i = 0; i < LevelParent.childCount; i++)
        {
            int level = i + 1;
            Button button = LevelParent.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => OpenInfoScroll(level));
        }
        SetUIState(UIState.LevelSelection);
    }

    public void Initialize(PlayerMarcine playerMarcine)
    {
        this.playerMarcine = playerMarcine;
    }

    public void OpenCollectionUI()
    {
        SetUIState(UIState.LevelSelection);
    }

    public void OpenInfoScroll(int Level)
    {
        var questList = QuestDataLoader.GetSingleton().GetQuestsAtLevel(Level);

        if (questList == null || questList.Count == 0)
        {
            Debug.Log($"No quests available for Level {Level}");
            return;
        }

        InfoUI.SetQuestInfoDatas(Level, playerMarcine);
        SetUIState(UIState.InfoScroll);
    }

    private void SetUIState(UIState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case UIState.LevelSelection:
                LevelParent.gameObject.SetActive(true);
                InfoUI.gameObject.SetActive(false);
                gameObject.SetActive(true);
                break;
            case UIState.InfoScroll:
                LevelParent.gameObject.SetActive(false);
                InfoUI.gameObject.SetActive(true);
                break;
            case UIState.Hidden:
                LevelParent.gameObject.SetActive(false);
                InfoUI.gameObject.SetActive(false);
                gameObject.SetActive(false);
                break;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (currentState == UIState.InfoScroll)
        {
            SetUIState(UIState.LevelSelection);
        }
        else
        {
            SetUIState(UIState.Hidden);
        }
    }
}
