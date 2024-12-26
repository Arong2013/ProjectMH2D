using System.Collections.Generic;

[System.Serializable]
public class MonsterPartData
{
    public int PartID { get; set; }
    public string PartName { get; set; }
    public float HP { get; set; }
    public float DisDMG { get; set; }
    public List<DropItemData> DropItems { get; set; } = new List<DropItemData>();
}
public class DropItemData
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public float DropRate { get; set; }
}