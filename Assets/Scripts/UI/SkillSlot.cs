using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//skill slots are similar to item slots, except skills are never removed, and there will never be more than 1 of the same skill.
public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotID;
    private Skill skill;


    CombatMenu menu;
    Inventory inv;
    DungeonMenu dungeonMenu;
    
    public void OnPointerEnter(PointerEventData pointer)
    {
        //highlight item and capture its index
        inv = Inventory.instance;
        if (skill != null)
        {
            inv.itemDetailsContainer.gameObject.SetActive(true);
            inv.itemDetailsUI.text = skill.description;

            //highlight
            Image img = GetComponent<Image>();
            img.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        inv = Inventory.instance;
        inv.itemDetailsContainer.gameObject.SetActive(false);
        inv.itemDetailsUI.text = "";
        
        //highlight
        Image img = GetComponent<Image>();
        img.enabled = false;
    }

    public void UseSkill()
    {
        if (skill != null)
        {
            //use skill
            //case (skill.)
        }
    }
}
