using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds every item in the game
public class ItemManager : MonoBehaviour
{
    public Consumable[] consumables;    
    public Weapon[] weapons;
    public Armor[] armor;
    public Trinket[] trinkets;

    //enums
    public enum ConsumableItem {Herb}
    public enum WeaponItem {Dagger, Sword, GoldenAxe}
    public static ItemManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
