using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Item slots are a way to identify which item is clicked in the inventory. Item slots only accept consumable items. */
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotID;
    private Consumable item;
    public int quantity;           //99 is the max


    CombatMenu menu;
    protected Inventory inv;
    protected DungeonMenu dungeonMenu;
    
    public virtual void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
        inv = Inventory.instance;
        if (item != null)
        {
            inv.itemDetailsContainer.gameObject.SetActive(true);
            inv.itemDetailsUI.text = item.details;

            //highlight
            Image img = GetComponent<Image>();
            img.enabled = true;
        }
    }

    public virtual void OnPointerExit(PointerEventData pointer)
    {
        inv = Inventory.instance;
        inv.itemDetailsContainer.gameObject.SetActive(false);
        inv.itemDetailsUI.text = "";
        
        //highlight
        Image img = GetComponent<Image>();
        img.enabled = false;
    }

    public void UseItem()
    {
        Debug.Log("Clicked slot " + slotID);
        if (item != null)
        {
            GameManager gm = GameManager.instance;
            inv = Inventory.instance;
            if (gm.gameState == GameManager.GameState.Combat)
            { 
                //copy the item
                menu = CombatMenu.instance;
                inv.copiedSlot = this;
                inv.ShowInventory(false);
                menu.menuState = CombatMenu.MenuState.SelectingHeroToTakeItem;

                //select the hero who gets the item
                CombatSystem cs = CombatSystem.instance;
                cs.selectingHero = true;
                UI ui = UI.instance;
                ui.selectTargetUI.text = "Click a hero to use " + item.itemName;
                ui.selectTargetUI.gameObject.SetActive(true);
                menu.backButton.gameObject.SetActive(true);
                menu.backButton.transform.position = menu.originalBackButtonPos;
            }
            else //using item outside of combat
            {
                Debug.Log("Using " + item.itemName + " outside of combat");
                //copy the item
                dungeonMenu = DungeonMenu.instance;
                inv.copiedSlot = this;
                dungeonMenu.SetState(DungeonMenu.MenuState.SelectingHeroToTakeItem);
            }

            //remove highlight
            Image img = GetComponent<Image>();
            img.enabled = false;
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
