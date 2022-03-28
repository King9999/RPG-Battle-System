using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*base class for all items in the game. Items include:
Weapons
Armor
Consumables 
*/

public abstract class Item : ScriptableObject
{
    [Header("Item Details")]
    public string itemName;
    public string details;
    public int cost;
    public Sprite sprite;
    [System.NonSerialized]protected bool isEquipped = false;     //applies to weapons and armor. NonSerialized means Unity will reset the variable state
    
    public enum ItemType
    {
        Weapon, Armor, Consumable
    }

   
    public ItemType itemType;

   //parameters for this will vary, so method must be overloaded. This will mainly be used by consumables
   public virtual void UseItem() {}
   public bool IsEquipped() { return isEquipped; }

}
