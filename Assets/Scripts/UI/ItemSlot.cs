using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* Item slots are a way to identify which item is clicked in the inventory. Item slots only accept consumable items. */
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotID;
    private Consumable item;
    public int quantity;           //99 is the max

    protected Inventory inv;
    CombatMenu menu;
    // Start is called before the first frame update
    void Start()
    {
        inv = Inventory.instance;
        menu = CombatMenu.instance;
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
            GameManager gm = GameManager.instance;
            if (gm.gameState == GameManager.GameState.Combat)
            { 
                //copy the item
                inv.copiedSlot = this;
                inv.ShowInventory(false);
                menu.menuState = CombatMenu.MenuState.SelectingHeroToTakeItem;

                //select the hero who gets the item
                CombatSystem cs = CombatSystem.instance;
                cs.selectingHero = true;
                UI ui = UI.instance;
                ui.selectTargetUI.text = "Click a hero to use " + item.itemName;
                ui.selectTargetUI.gameObject.SetActive(true);
            }
            else //using item outside of combat
            {
                Debug.Log("Using " + item.itemName + " outside of combat");
            }
        }
    }

    public Consumable ItemInSlot()
    {
        return item;
    }

    public void RemoveItem()
    {
        item = null;
    }

    public void AddItem(Consumable item)
    {
        this.item = item;
    }
}
