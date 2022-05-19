using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : ItemSlot
{
    private Weapon weaponInSlot;
    
    public override void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index. Display action gauge
        inv = Inventory.instance;
        if (weaponInSlot != null)
        {
            inv.actGaugeWindow.ShowWindow(true);
            inv.actGaugeWindow.UpdateGaugeData(weaponInSlot.actGauge);
            inv.itemDetailsContainer.gameObject.SetActive(true);
            inv.itemDetailsUI.text = weaponInSlot.details;

            //highlight item
            Image img = GetComponent<Image>();
            img.enabled = true;
        }
    }

    public override void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight and hide action gauge window.
        inv = Inventory.instance;
        inv.actGaugeWindow.ShowWindow(false);
        inv.itemDetailsContainer.gameObject.SetActive(false);
        inv.itemDetailsUI.text = "";

        //remove highlight
        Image img = GetComponent<Image>();
        img.enabled = false;
    }

    public void EquipWeapon()
    {
        if (weaponInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
            inv = Inventory.instance;
            dungeonMenu = DungeonMenu.instance;
            inv.copiedSlot = this;
            dungeonMenu.SetState(DungeonMenu.MenuState.SelectingWeaponToEquip);
        }
    }

    public Weapon WeaponInSlot() { return weaponInSlot; }
    public void RemoveWeapon() { weaponInSlot = null; }
    public void AddWeapon(Weapon weapon)
    {
        weaponInSlot = weapon;
    }
}
