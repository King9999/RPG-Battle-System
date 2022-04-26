using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : ItemSlot
{
    private Weapon weaponInSlot;

    // Start is called before the first frame update
    void Start()
    {
        inv = Inventory.instance;
    }

    public void EquipWeapon()
    {
        if (weaponInSlot != null)
        {
            //choose who to give item to, and then equip. Keep in mind equip restrictions.
        }
    }

    public Weapon WeaponInSlot() { return weaponInSlot; }
    public void RemoveWeapon() { weaponInSlot = null; }
    public void AddWeapon(Weapon weapon)
    {
        weaponInSlot = weapon;
    }
}
