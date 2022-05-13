using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSlot : ItemSlot
{
    private Armor armorInSlot;

    void Start()
    {
        inv = Inventory.instance;
    }

    public void EquipArmor()
    {
        if (armorInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
            Debug.Log("Selecting " + armorInSlot.itemName);
        }
    }

    public Armor ArmorInSlot() { return armorInSlot; }
    public void RemoveArmor() { armorInSlot = null; }
    public void AddArmor(Armor armor)
    {
        armorInSlot = armor;
    }
}
