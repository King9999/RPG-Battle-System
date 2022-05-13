using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds every item in the game
public class ItemManager : MonoBehaviour
{
    LootTables lootTable;
    public TextAsset tableFile;
    public Consumable[] consumables;    
    public Weapon[] weapons;
    public Armor[] armor;
    public Trinket[] trinkets;

    //enums
    public enum ConsumableItem {Herb}
    public enum WeaponItem {Dagger, Sword, GoldenAxe}
    public enum ArmorItem {Undershirt, Vest, Robe}
    public static ItemManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    //get item from table.
    public Item GetItem(int tableLevel)
    {
        lootTable = JsonUtility.FromJson<LootTables>(tableFile.text);   //treat this like opening file stream except you don't have to close
        Item item = null;

        //search given table for the requested item
        if (tableLevel < 0 || tableLevel >= lootTable.tables.Length) return null;

        int itemCount = lootTable.tables[tableLevel].tableItems.Length;

        float roll = Random.value;
        for (int i = 0; i < itemCount; i++)
        {
            if (roll <= lootTable.tables[tableLevel].tableItems[i].chance)
            {
                Debug.Log("Generated " + lootTable.tables[tableLevel].tableItems[i].itemName);

                //find this item in the array and generate it
                switch((Item.ItemType)lootTable.tables[tableLevel].tableItems[i].itemType)
                {
                    case Item.ItemType.Weapon:
                        foreach(Weapon weapon in weapons)
                        {
                            if (lootTable.tables[tableLevel].tableItems[i].itemName == weapon.itemName)
                            {
                                item = weapon;
                            }
                        }
                        break;

                    case Item.ItemType.Armor:
                        foreach(Armor armor in armor)
                        {
                            if (lootTable.tables[tableLevel].tableItems[i].itemName == armor.itemName)
                            {
                                item = armor;
                            }
                        }
                        break;

                    case Item.ItemType.Consumable:
                        foreach(Consumable consumable in consumables)
                        {
                            if (lootTable.tables[tableLevel].tableItems[i].itemName == consumable.itemName)
                            {
                                item = consumable;
                            }
                        }
                        break;

                    case Item.ItemType.Trinket:
                        foreach(Trinket trinket in trinkets)
                        {
                            if (lootTable.tables[tableLevel].tableItems[i].itemName == trinket.itemName)
                            {
                                item = trinket;
                            }
                        }
                        break;
                }
                break;  //break for loop
            }
            
        }

        return item;
    }
}
