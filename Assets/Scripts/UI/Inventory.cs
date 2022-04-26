using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/* All items are kept in a dictionary, and interacting with them requires mouse events and buttons. */
public class Inventory : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    Dictionary<Consumable, int> items;
    Dictionary<Weapon, int> weapons;
    Dictionary<Armor, int> armor;
    Dictionary<Trinket, int> trinkets;
    //public Button[] itemButtons;           //when these are clicked, item is used.
    public ItemSlot[] itemSlots;            //contains consumables only
    public WeaponSlot[] weaponSlots;
    public ArmorSlot[] armorSlots;
    public TrinketSlot[] trinketSlots;
    public ItemSlot copiedSlot;            //copy of an item that is about to be used.
    int money;
    int maxMoney {get;} = 10000000;
    public int currentItem;                //iterator
    int maxItems {get;} = 10;


    bool animateHighlightCoroutineOn;

    ItemManager im;
    public static Inventory instance;

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
        im = ItemManager.instance;
        items = new Dictionary<Consumable, int>();

        currentItem = 0;
        gameObject.SetActive(false);
        AddItem(im.consumables[(int)ItemManager.ConsumableItem.Herb], 1);
        AddItem(im.consumables[(int)ItemManager.ConsumableItem.Herb], 2);
    }

    // Update is called once per frame
    void Update()
    {
        //run coroutine to highlight item
    }

    /*public void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight
    }*/

    //add item to first available slot
    public void AddItem(Consumable item, int amount)
    {
        bool itemFound = false;
        foreach(ItemSlot slot in itemSlots)
        {
            //if item already in inventory, just add 1 to quantity
            if (slot.ItemInSlot() == null) continue;

            if (slot.ItemInSlot().itemName == item.itemName)
            {
                slot.quantity += amount;
                itemFound = true;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.ItemInSlot().itemName + " " + slot.quantity;
                break;
            }
        }

        if (!itemFound)
        {
            //add item to a new slot
            int i = 0;
            while(itemSlots[i].ItemInSlot() != null && itemSlots.Length < maxItems)
            {
                i++;
            }

            //itemSlots[i].item = item;
            itemSlots[i].AddItem(item);
            itemSlots[i].quantity += amount;
            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemSlots[i].ItemInSlot().itemName + " " + itemSlots[i].quantity;
        }

    }

    public void AddItem(Weapon item, int amount)
    {
        //TODO: Modify the code below for non-consumable items. The items must go into an inventory specifically for non-consumables.
        /*bool itemFound = false;
        foreach(ItemSlot slot in itemSlots)
        {
            //if item already in inventory, just add 1 to quantity
            if (slot.item == null) continue;

            if (slot.item.itemName == item.itemName)
            {
                slot.quantity += amount;
                itemFound = true;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.itemName + " " + slot.quantity;
                break;
            }
        }

        if (!itemFound)
        {
            //add item to a new slot
            int i = 0;
            while(itemSlots[i].item != null && itemSlots.Length < maxItems)
            {
                i++;
            }

            itemSlots[i].item = item;
            itemSlots[i].quantity += amount;
            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemSlots[i].item.itemName + " " + itemSlots[i].quantity;
        }*/
    }

    public void AddItem(Armor item, int amount)
    {

    }
    public void AddItem(Trinket item, int amount)
    {

    }

    public void AddMoney(int amount)
    {
        money += amount;
        if (money > maxMoney)
        {
            money = maxMoney;
        }  
    }

    public void ShowInventory(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    /*public void OnItemClicked()
    {
        //check which item was clicked.
        Debug.Log("Clicked on " + )
    }*/
}
