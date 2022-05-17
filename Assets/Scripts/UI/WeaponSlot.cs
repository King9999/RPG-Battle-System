using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : ItemSlot
{
    private Weapon weaponInSlot;
    ActionGaugeWindow actGaugeWindow;

    // Start is called before the first frame update
    void Start()
    {
        inv = Inventory.instance;
        actGaugeWindow.ShowWindow(false);
    }

    public override void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index. Display action gauge
        actGaugeWindow.ShowWindow(true);
    }

    public override void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight and hide action gauge window.
        actGaugeWindow.ShowWindow(false);

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
