 [System.Serializable]
 public class LootTable
 {
    public int tableLevel;
    public TableItem[] tableItems;
 }

[System.Serializable]
 public class TableItem
 {
     public int itemType;           //0 = weapon, 1 = armor, 2 = consumable, 3 = trinket. Values must correspond to ItemType enum in Item class.
     public string itemName;
     public float chance;           //probability of item being generated and placed into a chest.
 }



[System.Serializable]
public class LootTables
{ 
    public LootTable[] tables;
}
