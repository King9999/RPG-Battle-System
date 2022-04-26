using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketSlot : ItemSlot
{
    private Trinket trinketInSlot;

    void Start()
    {
        inv = Inventory.instance;
    }

    public void EquipTrinket()
    {
        if (trinketInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
        }
    }

    public Trinket TrinketInSlot() { return trinketInSlot; }
    public void RemoveTrinket() { trinketInSlot = null; }
    public void AddTrinket(Trinket trinket)
    {
        trinketInSlot = trinket;
    }
}
