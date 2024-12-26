using System;
using System.Data;
using UnityEngine;

public enum ThrowItemType
{
    FlashBoom,
}
public class ThrowItemData : CountableItemData
{
    public readonly ThrowItemType throwType;

    public ThrowItemData(IDataReader reader) : base(reader)
    {
        string typeValue = reader.GetString(1);
        if (Enum.TryParse(typeValue, out ThrowItemType parsedType))
        {
            throwType = parsedType;
        }
        else
        {
            throwType = ThrowItemType.FlashBoom; 
        }
    }
    public override Item CreatItem()
    {
        return new ThrowItem(this);
    }
}
public class ThrowItem : CountableItem,IUseableItem
{
    ThrowItemType throwItemType => (ItemData as ThrowItemData).throwType;
    public ThrowItem(ItemData itemData) : base(itemData) { }
    public void Use(PlayerMarcine playerMarcine)
    {
        var obj =  ItemDataLoader.Instance.GetThrowOBJ(throwItemType);
        playerMarcine.SetInteraction(InteractionType.Throw,obj);
    }
 }  