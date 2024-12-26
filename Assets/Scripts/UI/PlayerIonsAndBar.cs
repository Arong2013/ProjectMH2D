using UnityEngine;
using UnityEngine.UI;

public class PlayerIonsAndBar : MonoBehaviour, IObserver, IPlayerUesableUI
{
    private PlayerMarcine player;

    [SerializeField] private Slider HpBar;
    [SerializeField] private Slider SpBar;
    [SerializeField] private float barUpdateSpeed = 2f;
    [SerializeField] Button IconBtn;

    private float currentHp;
    private float currentSp;

    private Coroutine updateBarsCoroutine;

    public void Initialize(PlayerMarcine player)
    {
        this.player = player;
        player.RegisterObserver(this);

        IconBtn.onClick.AddListener(() => Utils.GetUI<MenuUI>().SetOpenUI());
        currentHp = player.characterData.GetStat(CharacterStatName.HP);
        currentSp = player.characterData.GetStat(CharacterStatName.SP);

        UpdateBarsInstantly();
    }

    public void UpdateObserver()
    {
        if (updateBarsCoroutine != null)
        {
            StopCoroutine(updateBarsCoroutine);
        }
        updateBarsCoroutine = StartCoroutine(UpdateBarsSmoothly());
    }

    private void OnDestroy()
    {
        if (player != null)
            player.UnregisterObserver(this);

        if (updateBarsCoroutine != null)
        {
            StopCoroutine(updateBarsCoroutine);
        }
    }

    private void UpdateBarsInstantly()
    {
        HpBar.value = currentHp / player.characterData.GetStat(CharacterStatName.MaxHP);
        SpBar.value = currentSp / player.characterData.GetStat(CharacterStatName.MaxSP);
    }

    private System.Collections.IEnumerator UpdateBarsSmoothly()
    {
        float targetHp = player.characterData.GetStat(CharacterStatName.HP);
        float targetSp = player.characterData.GetStat(CharacterStatName.SP);

        while (!Mathf.Approximately(currentHp, targetHp) || !Mathf.Approximately(currentSp, targetSp))
        {
            currentHp = Mathf.Lerp(currentHp, targetHp, Time.deltaTime * barUpdateSpeed);
            currentSp = Mathf.Lerp(currentSp, targetSp, Time.deltaTime * barUpdateSpeed);

            HpBar.value = currentHp / player.characterData.GetStat(CharacterStatName.MaxHP);
            SpBar.value = currentSp / player.characterData.GetStat(CharacterStatName.MaxSP);
            yield return null;
        }
        updateBarsCoroutine = null;
    }
}
