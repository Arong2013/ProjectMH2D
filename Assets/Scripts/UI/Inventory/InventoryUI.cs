using UnityEngine;

public class InventoryUI : MonoBehaviour, IPlayerUesableUI,IObserver,IDisableUI
{
    Inventory Inventory;

    [SerializeField] ItemSlot itemSlot;
    [SerializeField] ItemInformationUI itemInformationUI;
    [SerializeField] Transform parent;
    public void Initialize(PlayerMarcine playerMarcine)
    {
        Inventory = playerMarcine.inventory;
        playerMarcine.RegisterObserver(this);
    }
    public void OpenInventoryUI()
    {
        Utils.GetUI<DisableBackGround>().SetOBJ(this);
        gameObject.SetActive(true);
        UpDateSlots();
    }
    public void UpDateSlots()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in Inventory.items)
        {
            ItemSlot curSlot = Instantiate(itemSlot.gameObject, parent.transform).GetComponent<ItemSlot>();
            curSlot.SetSlot(item);
            curSlot.ActionAdd(() =>itemInformationUI.SetItem(item));
        }
    }
    public void UpdateObserver()
    {
        UpDateSlots();
    }
    public void SetActiveFalse()
    {
       gameObject.SetActive(false); 
    }
}