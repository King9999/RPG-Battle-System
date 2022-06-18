using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//skill slots are similar to item slots, except skills are never removed, and there will never be more than 1 of the same skill.
public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotID;
    [SerializeField]private Skill skill;


    CombatMenu menu;
    Inventory inv;
    DungeonMenu dungeonMenu;
    CombatSystem cs;
    UI ui;
    
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
        cs = CombatSystem.instance;

        Hero hero = cs.heroesInCombat[cs.currentHero];
        if (skill != null && hero.manaPoints >= skill.manaCost * hero.mpMod)
        {
            //use skill
            Debug.Log("Clicked on " + skill.skillName);
            menu = CombatMenu.instance;
            
            ui = UI.instance;
            if (skill.targetType == Skill.Target.OneEnemy)
            {
                cs.selectingTargetToAttackWithSkill = true;
                ui.selectTargetUI.text = "Choose a target";
                inv.copiedSkillSlot = this;
                inv.ShowInventory(false);
                cs.heroUsingSkill = true;
                ui.selectTargetUI.gameObject.SetActive(true);
                menu.backButton.gameObject.SetActive(true);
                menu.backButton.transform.position = menu.originalBackButtonPos;
                menu.menuState = CombatMenu.MenuState.SelectingSkillTarget;
            }

            else if (skill.targetType == Skill.Target.AllEnemies)
            {
                cs.selectingTargetToAttackWithSkill = true;
                ui.selectTargetUI.text = "Click any target to confirm";
                inv.copiedSkillSlot = this;
                inv.ShowInventory(false);
                cs.heroUsingSkill = true;
                ui.selectTargetUI.gameObject.SetActive(true);
                menu.backButton.gameObject.SetActive(true);
                menu.backButton.transform.position = menu.originalBackButtonPos;
                menu.menuState = CombatMenu.MenuState.SelectingSkillTarget;
            }

            else if (skill.targetType == Skill.Target.Self)
            {
                cs.selectingHeroToUseSkillOn = true;
                ui.selectTargetUI.text = "Click skill user to confirm";
                inv.copiedSkillSlot = this;
                inv.ShowInventory(false);
                cs.heroUsingSkill = true;
                ui.selectTargetUI.gameObject.SetActive(true);
                menu.backButton.gameObject.SetActive(true);
                menu.backButton.transform.position = menu.originalBackButtonPos;
                menu.menuState = CombatMenu.MenuState.SelectingSkillTarget;
            }

            else if (skill.targetType == Skill.Target.OneHero)
            {
                cs.selectingHeroToUseSkillOn = true;
                ui.selectTargetUI.text = "Choose a hero";
                inv.copiedSkillSlot = this;
                inv.ShowInventory(false);
                cs.heroUsingSkill = true;
                ui.selectTargetUI.gameObject.SetActive(true);
                menu.backButton.gameObject.SetActive(true);
                menu.backButton.transform.position = menu.originalBackButtonPos;
                menu.menuState = CombatMenu.MenuState.SelectingSkillTarget;
            }

            //disable highlight and description
            Image img = GetComponent<Image>();
            img.enabled = false;
            inv.itemDetailsContainer.gameObject.SetActive(false);
            inv.itemDetailsUI.text = "";
        }
    }

    public Skill SkillInSlot()
    {
        return skill;
    }

    public void AddSkill(Skill skill)
    {
        this.skill = skill;
    }

    public void RemoveSkill()
    {
        skill = null;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
    }
}
