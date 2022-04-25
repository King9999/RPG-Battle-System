using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* Item slots are a way to identify which item is clicked in the inventory. Item slots only accept consumable items. */
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotID;
    public Consumable item;
    public int quantity;           //99 is the max

    Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        inv = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight
    }

    public void UseItem()
    {
        Debug.Log("Clicked slot " + slotID);
        if (item != null)
        {
            //copy the item
            inv.copiedSlot = this;
            inv.ShowInventory(false);

            //select the hero who gets the item
            CombatSystem cs = CombatSystem.instance;
            cs.selectingHero = true;
            UI ui = UI.instance;
            ui.selectTargetUI.text = "Click a hero to use " + item.itemName;
            ui.selectTargetUI.gameObject.SetActive(true);
        }
    }
}
