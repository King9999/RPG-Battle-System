using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*base class for all items in the game. Items include:
Weapons
Armor
Consumables 
*/

[CreateAssetMenu(menuName = "Item", fileName = "Medal_")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public int cost;
    public Sprite sprite;
    
    public enum ItemType
    {
        Weapon, Armor, Consumable
    }

    public enum EffectID
    {
        None, HealMinor, Ice
    }    

    public EffectID itemEffect;
    public ItemType itemType;

   //parameters for this will vary, so method must be overloaded. This will mainly be used by consumables
   public virtual void UseItem()
   {
        
   }

}
