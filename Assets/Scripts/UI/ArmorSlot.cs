using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmorSlot : ItemSlot
{
    private Armor armorInSlot;

    public override void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
        inv = Inventory.instance;
        if (armorInSlot != null)
        {
            inv.itemDetailsContainer.gameObject.SetActive(true);
            inv.itemDetailsUI.text = armorInSlot.details;

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

    public void EquipArmor()
    {
        if (armorInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
            inv = Inventory.instance;
            dungeonMenu = DungeonMenu.instance;
            inv.copiedSlot = this;
            dungeonMenu.SetState(DungeonMenu.MenuState.SelectingArmorToEquip);
            //inv.ShowInventory(false);
            //inv.statsDisplay.ShowDisplay(true);
            //inv.statsDisplay.itemToEquipUI.text = armorInSlot.itemName;
            //dungeonMenu.menuState = DungeonMenu.MenuState.SelectingArmorToEquip;
            //DungeonUI ui = DungeonUI.instance;
            //ui.selectTargetUI.gameObject.SetActive(true);
        }
    }

    public Armor ArmorInSlot() { return armorInSlot; }
    public void RemoveArmor() { armorInSlot = null; }
    public void AddArmor(Armor armor)
    {
        armorInSlot = armor;
    }
}
