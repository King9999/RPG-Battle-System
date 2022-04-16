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
        currentItem = 0;
        AddItem(im.consumables[(int)ItemManager.ConsumableItem.Herb], 1);
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
        }
        else
        {
            if (items.Count < maxItems)
                items.Add(item, amount);
            else
                return;
        }

        itemButtons[currentItem].GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " " + items[item];
        currentItem++;
    }
}
