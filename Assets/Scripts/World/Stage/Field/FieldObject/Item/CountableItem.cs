using System.Data;

public abstract class CountableItemData : ItemData
{
    public readonly int Amount;
    public readonly int MaxAmount;
    protected CountableItemData(IDataReader reader) : base(reader)
    {
        Amount = reader.GetInt32(3);
        MaxAmount =  reader.GetInt32(4);
    }
}


public abstract class CountableItem : Item
{
    public int Amount;
    public int MaxAmount => (ItemData as CountableItemData).MaxAmount;
    protected CountableItem(ItemData itemData) : base(itemData)
    {
        Amount = (ItemData as CountableItemData)?.Amount ?? 0;
    }  
}