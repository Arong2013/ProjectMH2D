using Unity.VisualScripting;
using UnityEngine;
public class ItemUnderListParent : NestedScrollUI, IPlayerUesableUI, IObserver
{
    PlayerMarcine PlayerMarcine { get; set; }   
    Inventory Inventory;
    [SerializeField] Transform parent;
    [SerializeField] ItemSlot slot;
    public void Initialize(PlayerMarcine playerMarcine)
    {
        Inventory = playerMarcine.inventory;
        PlayerMarcine = playerMarcine;
        playerMarcine.RegisterObserver(this);
    }

    public void UpdateObserver()
    {
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
            ItemSlot curSlot = Instantiate(slot.gameObject, parent.transform).GetComponent<ItemSlot>();
            curSlot.SetSlot(item);
            curSlot.ActionAdd(() => Useitem(item));
        }
        UpdataScroll(Inventory.items.Count);
    }
    public void Useitem(Item item)
    {
        if(item is IUseableItem useable && !isDrag)
        {
            useable.Use(PlayerMarcine);
        }
    }
}
