using System.Collections.Generic;
using System.Data;
using UnityEngine;
public abstract class ItemData
{
    public readonly int ID;
    public readonly string Name;
    public readonly string Information;
    public abstract Item CreatItem();

    public ItemData(IDataReader reader)
    {
        ID = reader.GetInt32(0);
        Name = reader.GetString(1);
        Information = reader.GetString(2);
    }
}
public abstract class Item
{
    public readonly int ID; 
    Sprite ItemIcon;
    public Sprite icon => ItemIcon ??= ItemDataLoader.Instance.GetSpriteByName(ItemData.Name);
    public ItemData ItemData => ItemDataLoader.Instance.GetItemDataByID(ID);
    public Item(ItemData itemData)
    {
        this.ID = itemData.ID;   
    }
}
