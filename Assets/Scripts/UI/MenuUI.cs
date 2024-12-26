using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour,IDisableUI
{
    [SerializeField] Button[] btns;

    public void Awake()
    {
        btns[0].onClick.AddListener(() => Utils.GetUI<InventoryUI>().OpenInventoryUI());
    }
    public void SetOpenUI()
    {
        Utils.GetUI<DisableBackGround>().SetOBJ(this);
        gameObject.SetActive(true);
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);    
    }
}