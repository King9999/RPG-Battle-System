using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : ItemSlot
{
    private Weapon weaponInSlot;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index. Display action gauge
        Inventory inv = Inventory.instance;
        if (weaponInSlot != null)
        {
            inv.actGaugeWindow.ShowWindow(true);
        }
    }

    public override void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight and hide action gauge window.
        Inventory inv = Inventory.instance;
        inv.actGaugeWindow.ShowWindow(false);

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
