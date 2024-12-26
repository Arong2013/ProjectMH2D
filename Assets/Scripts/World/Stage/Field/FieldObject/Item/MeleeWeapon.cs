using Newtonsoft.Json;
using System.Data;

public class MeleeWeaponData: ItemData
{
    public readonly int ATK;
    public MeleeWeaponData(IDataReader reader) : base(reader)
    {
        ATK = reader.GetInt32(3);
    }
    public override Item CreatItem()
    {
        return new MeleeWeapon(this);
    }
}
public class MeleeWeapon : Item
{
    [JsonProperty] public int ATK { get; }
    public MeleeWeapon(ItemData itemData) : base(itemData)
    {
        ATK = (itemData as MeleeWeaponData)?.ATK ?? 0;
    }
}