 [System.Serializable]
 public class LootTable
 {
    public int tableLevel;
    public TableItem[] tableItems;
 }

[System.Serializable]
 public class TableItem
 {
     public string itemName;
     public float chance;           //probability of item being generated and placed into a chest.
 }



[System.Serializable]
public class LootTables
{ 
    public LootTable[] tables;
}
