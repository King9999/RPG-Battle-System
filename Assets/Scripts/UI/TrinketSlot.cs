using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrinketSlot : ItemSlot
{
    private Trinket trinketInSlot;

    public override void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
        inv = Inventory.instance;
        if (trinketInSlot != null)
        {
            inv.itemDetailsContainer.gameObject.SetActive(true);
            inv.itemDetailsUI.text = trinketInSlot.details;

            //highlight
            Image img = GetComponent<Image>();
            img.enabled = true;
        }
    }

    public override void OnPointerExit(PointerEventData pointer)
    {
        inv = Inventory.instance;
        inv.itemDetailsContainer.gameObject.SetActive(false);
        inv.itemDetailsUI.text = "";
        
        //highlight
        Image img = GetComponent<Image>();
        img.enabled = false;
    }

    public void SelectTrinket()
    {
        if (trinketInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
            inv = Inventory.instance;
            dungeonMenu = DungeonMenu.instance;
            inv.copiedSlot = this;
            dungeonMenu.SetState(DungeonMenu.MenuState.SelectingTrinketToEquip);
        }
    }

    public Trinket TrinketInSlot() { return trinketInSlot; }
    public void RemoveTrinket() { trinketInSlot = null; }
    public void AddTrinket(Trinket trinket)
    {
        trinketInSlot = trinket;
    }
}
