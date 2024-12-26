using TMPro;
using UnityEngine;

public class ItemInformationUI : MonoBehaviour, IDisableUI
{
    [SerializeField] TextMeshProUGUI nameTMP, InfoTMP;

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void SetItem(Item item)
    {
        Utils.GetUI<DisableBackGround>().SetOBJ(this);

        nameTMP.text = item.ItemData.Name;
        InfoTMP.text = item.ItemData.Information;

        gameObject.SetActive(true);
    }
}