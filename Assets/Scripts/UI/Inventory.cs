using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/* All items are kept in a dictionary, and interacting with them requires mouse events and buttons. */
public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Dictionary<Item, int> items;
    public Button[] itemButtons;           //when these are clicked, item is used.
    public int currentItem;                //iterator
    int maxItems {get;} = 10;


    bool animateHighlightCoroutineOn;

    ItemManager im;

    // Start is called before the first frame update
    void Start()
    {
        im = ItemManager.instance;
        items = new Dictionary<Item, int>();

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

    public void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        //remove highlight
    }

    //add item to first available slot
    public void AddItem(Item item, int amount)
    {
       
        if (items.ContainsKey(item))
        {
            items[item] += amount;

            //find where the item is located so we can update the text
            int i = 0; 
            foreach(Item itemInInventory in items.Keys)
            {
                if (itemInInventory.Equals(item))
                {
                    itemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " " + items[item];
                    break;
                }
                else
                {
                    i++;
                }
            }
            //itemButtons[currentItem].GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " " + items[item];
        }
        else
        {
            if (items.Count < maxItems)
            {
                items.Add(item, amount);
                itemButtons[currentItem].GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " " + items[item];
                currentItem++;
            }
            else
                return;
        }

        
    }

    public void ShowInventory(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
