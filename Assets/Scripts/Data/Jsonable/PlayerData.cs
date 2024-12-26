using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    List<Item> items;
    public  int InventoryMaxCount = 30;
    List<Quest> runningQuest;
    public List<Item> Items => items ??= new List<Item>();   
    public List<Quest> GetRunningQuest() => runningQuest ??= new List<Quest>(); 
}