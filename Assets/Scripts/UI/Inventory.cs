using UnityEngine;
using TMPro;

/* All items are kept in a dictionary, and interacting with them requires mouse events and buttons. */
public class Inventory : MonoBehaviour
{
    //Dictionary<Consumable, int> items;
    //Dictionary<Weapon, int> weapons;
    int weaponCount;
    int itemCount;          //consumables
    int armorCount;
    int trinketCount;
    //Dictionary<Armor, int> armor;
    //Dictionary<Trinket, int> trinkets;
    //public Button[] itemButtons;           //when these are clicked, item is used.
    public ItemSlot[] itemSlots;            //contains consumables only
    public WeaponSlot[] weaponSlots;
    public ArmorSlot[] armorSlots;
    public TrinketSlot[] trinketSlots;
    public SkillSlot[] skillSlots;
    public ActionGaugeWindow actGaugeWindow;
    public StatsDisplay statsDisplay;
    public TextMeshProUGUI itemDetailsUI;

    //containers
    public GameObject itemSlotContainer;    
    public GameObject weaponSlotContainer;
    public GameObject armorSlotContainer;
    public GameObject trinketSlotContainer;
    public GameObject itemDetailsContainer;
    public GameObject skillSlotContainer;

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
        /*items = new Dictionary<Consumable, int>();
        weapons = new Dictionary<Weapon, int>();
        armor = new Dictionary<Armor, int>();
        trinkets = new Dictionary<Trinket, int>();*/

        weaponCount = 0;
        itemCount = 0;
        trinketCount = 0;
        armorCount = 0;

        currentItem = 0;
        gameObject.SetActive(false);
        itemSlotContainer.gameObject.SetActive(false);
        weaponSlotContainer.gameObject.SetActive(false);
        trinketSlotContainer.gameObject.SetActive(false);
        armorSlotContainer.gameObject.SetActive(false);
        skillSlotContainer.gameObject.SetActive(false);
        actGaugeWindow.ShowWindow(false);
        itemDetailsContainer.gameObject.SetActive(false);
        //itemDetailsUI.text = "";

        //add test items
        AddItem(im.consumables[(int)ItemManager.ConsumableItem.Herb], 5);
        //AddItem(im.armor[(int)ItemManager.ArmorItem.Undershirt], 1);
        //AddItem(im.weapons[(int)ItemManager.WeaponItem.Dagger], 1);
        //AddItem(im.weapons[(int)ItemManager.WeaponItem.Axe], 1);
        //AddItem(im.trinkets[(int)ItemManager.TrinketItem.SpeedBoots], 1);
    }

    public void HideAllSlots()
    {
        itemSlotContainer.gameObject.SetActive(false);
        weaponSlotContainer.gameObject.SetActive(false);
        trinketSlotContainer.gameObject.SetActive(false);
        armorSlotContainer.gameObject.SetActive(false);
        skillSlotContainer.gameObject.SetActive(false);
    }

    public void ShowItemSlots(bool toggle){ itemSlotContainer.gameObject.SetActive(toggle); }
    public void ShowWeaponSlots(bool toggle){ weaponSlotContainer.gameObject.SetActive(toggle); }
    public void ShowArmorSlots(bool toggle){ armorSlotContainer.gameObject.SetActive(toggle); }
    public void ShowTrinketSlots(bool toggle){ trinketSlotContainer.gameObject.SetActive(toggle); }
    public void ShowSkillSlots(bool toggle){ skillSlotContainer.gameObject.SetActive(toggle); }

    //add item to first available slot
    public void AddItem(Consumable item, int amount)
    {
        /*if (items.Count >= maxItems)
        {
            //prevent item from being destroyed so player has a chance to make room
            return;
        }*/
        if (itemCount >= maxItems) return;

        bool itemFound = false;
        foreach(ItemSlot slot in itemSlots)
        {
            //if item already in inventory, just add 1 to quantity
            if (slot.ItemInSlot() == null) continue;

            if (slot.ItemInSlot().itemName == item.itemName)
            {
                slot.quantity += amount;
                itemFound = true;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.ItemInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }

        if (!itemFound)
        {
            //add item to a new slot
            int i = 0;
            while(itemSlots[i].ItemInSlot() != null && i < itemSlots.Length)
            {
                i++;
            }

            //itemSlots[i].item = item;
            itemCount++;
            itemSlots[i].AddItem(item);
            itemSlots[i].quantity += amount;
            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemSlots[i].ItemInSlot().itemName + " -- " + itemSlots[i].quantity;
        }

    }

    public void AddItem(Weapon item, int amount)
    {
        /*if (weapons.Count >= maxItems)
        {
            //prevent item from being destroyed so player has a chance to make room
            return;
        }*/
        if (weaponCount >= maxItems) return;
        
        bool itemFound = false;
        foreach(WeaponSlot slot in weaponSlots)
        {
            //if item already in inventory, just add 1 to quantity
            if (slot.WeaponInSlot() == null) continue;

            if (slot.WeaponInSlot().itemName == item.itemName)
            {
                //extra check for staves
                if (slot.WeaponInSlot().weaponType == Weapon.WeaponType.Staff)
                {
                    if (slot.WeaponInSlot().weaponSkill == item.weaponSkill)
                    {
                        slot.quantity += amount;
                        //itemFound = true;
                        slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.WeaponInSlot().itemName + " -- " + slot.quantity;
                    }
                    else
                    {
                        //put staff in its own slot
                        int i = 0;
                        while(weaponSlots[i].WeaponInSlot() != null && i < weaponSlots.Length)
                        {
                            i++;
                        }

                        weaponCount++;
                        weaponSlots[i].AddWeapon(item);
                        weaponSlots[i].quantity += amount;
                        weaponSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = weaponSlots[i].WeaponInSlot().itemName + " -- " + weaponSlots[i].quantity;
                    }
                    itemFound = true;
                }
                else
                {
                    slot.quantity += amount;
                    itemFound = true;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.WeaponInSlot().itemName + " -- " + slot.quantity;
                }
            
                break;
            }
        }

        if (!itemFound)
        {
            //add item to a new slot
            int i = 0;
            while(weaponSlots[i].WeaponInSlot() != null && i < weaponSlots.Length)
            {
                i++;
            }

            weaponCount++;
            weaponSlots[i].AddWeapon(item);
            weaponSlots[i].quantity += amount;
            weaponSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = weaponSlots[i].WeaponInSlot().itemName + " -- " + weaponSlots[i].quantity;
        }
    }

    public void AddItem(Armor item, int amount)
    {
        /*if (armor.Count >= maxItems)
        {
            //prevent item from being destroyed so player has a chance to make room
            return;
        }*/
        if (armorCount >= maxItems) return;

        bool itemFound = false;
        foreach(ArmorSlot slot in armorSlots)
        {
            //if item already in inventory, just add 1 to quantity
            if (slot.ArmorInSlot() == null) continue;

            if (slot.ArmorInSlot().itemName == item.itemName)
            {
                slot.quantity += amount;
                itemFound = true;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.ArmorInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }

        if (!itemFound)
        {
            //add item to a new slot
            int i = 0;
            while(armorSlots[i].ArmorInSlot() != null && i < armorSlots.Length)
            {
                i++;
            }
            
            armorCount++;
            armorSlots[i].AddArmor(item);
            armorSlots[i].quantity += amount;
            armorSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = armorSlots[i].ArmorInSlot().itemName + " -- " + armorSlots[i].quantity;
        }
    }
    public void AddItem(Trinket item, int amount)
    {
        /*if (trinkets.Count >= maxItems)
        {
            //prevent item from being destroyed so player has a chance to make room
            return;
        }*/
        if (trinketCount >= maxItems) return;

        bool itemFound = false;
        foreach(TrinketSlot slot in trinketSlots)
        {
            //if item already in inventory, just add 1 to quantity
            if (slot.TrinketInSlot() == null) continue;

            if (slot.TrinketInSlot().itemName == item.itemName)
            {
                slot.quantity += amount;
                itemFound = true;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.TrinketInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }

        if (!itemFound)
        {
            //add item to a new slot
            int i = 0;
            while(trinketSlots[i].TrinketInSlot() != null && i < trinketSlots.Length)
            {
                i++;
            }
            
            trinketCount++;
            trinketSlots[i].AddTrinket(item);
            trinketSlots[i].quantity += amount;
            trinketSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = trinketSlots[i].TrinketInSlot().itemName + " -- " + trinketSlots[i].quantity;
        }
    }

    public void AddSkill(Skill skill)
    {
       
        //add skill to a new slot
        int i = 0;
        while(skillSlots[i].SkillInSlot() != null && i < skillSlots.Length)
        {
            i++;
        }
        
        skillSlots[i].AddSkill(skill);
        skillSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = skillSlots[i].SkillInSlot().skillName + " -- " + skillSlots[i].SkillInSlot().manaCost + "MP";     
    }


    public void RemoveItem(Weapon item, int amount)
    {
        //if (weapons.Count <= 0) return;
        if (weaponCount <= 0) return;
        
        foreach(WeaponSlot slot in weaponSlots)
        {
            if (slot.WeaponInSlot() == null) continue;

            if (slot.WeaponInSlot().itemName == item.itemName)
            {
                slot.quantity -= amount;
                if (slot.quantity <= 0)
                {
                    //remove from slot
                    slot.RemoveWeapon();
                    weaponCount--;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                else
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.WeaponInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }
    }
    public void RemoveItem(Armor item, int amount)
    {
        if (armorCount <= 0) return;
        
        foreach(ArmorSlot slot in armorSlots)
        {
            if (slot.ArmorInSlot() == null) continue;

            if (slot.ArmorInSlot().itemName == item.itemName)
            {
                slot.quantity -= amount;
                if (slot.quantity <= 0)
                {
                    //remove from slot
                    slot.RemoveArmor();
                    armorCount--;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                else
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.ArmorInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }
    }

    public void RemoveItem(Trinket item, int amount)
    {
        if (trinketCount <= 0) return;
        
        foreach(TrinketSlot slot in trinketSlots)
        {
            if (slot.TrinketInSlot() == null) continue;

            if (slot.TrinketInSlot().itemName == item.itemName)
            {
                slot.quantity -= amount;
                if (slot.quantity <= 0)
                {
                    //remove from slot
                    slot.RemoveTrinket();
                    trinketCount--;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                else
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.TrinketInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }
    }
    public void RemoveItem(Consumable item, int amount)
    {
        if (itemCount <= 0) return;
        
        foreach(ItemSlot slot in itemSlots)
        {
            if (slot.ItemInSlot() == null) continue;

            if (slot.ItemInSlot().itemName == item.itemName)
            {
                slot.quantity -= amount;
                if (slot.quantity <= 0)
                {
                    //remove from slot
                    slot.RemoveItem();
                    itemCount--;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                else
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = slot.ItemInSlot().itemName + " -- " + slot.quantity;
                break;
            }
        }
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
