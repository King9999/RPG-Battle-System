using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : ItemSlot
{
    private Weapon weaponInSlot;
    
    public override void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index. Display action gauge
        Inventory inv = Inventory.instance;
        if (weaponInSlot != null)
        {
            inv.actGaugeWindow.ShowWindow(true);
            inv.actGaugeWindow.UpdateGaugeData(weaponInSlot.actGauge);
            inv.itemDetailsContainer.gameObject.SetActive(true);
            inv.itemDetailsUI.text = weaponInSlot.details;
        }
    }

    public override void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight and hide action gauge window.
        Inventory inv = Inventory.instance;
        inv.actGaugeWindow.ShowWindow(false);
        inv.itemDetailsContainer.gameObject.SetActive(false);
        inv.itemDetailsUI.text = "";
    }

    public void EquipWeapon()
    {
        if (weaponInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
            Debug.Log("Selecting " + weaponInSlot.itemName);
        }
    }

    public Weapon WeaponInSlot() { return weaponInSlot; }
    public void RemoveWeapon() { weaponInSlot = null; }
    public void AddWeapon(Weapon weapon)
    {
        weaponInSlot = weapon;
    }
}
