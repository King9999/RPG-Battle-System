using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//opens up a menu with options to choose from. The menu options are: Attack, Skill, Item, Escape
public class CombatMenu : MonoBehaviour
{
    CombatSystem cs;

    public Image border;
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button escapeButton;
    public Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        cs = CombatSystem.instance;
        gameObject.SetActive(false);
    }

   
    public void OnAttackButtonClicked()
    {
        //player selects a target
        //if (cs.currentTarget < 0) return;

        cs.selectingTargetToAttack = true;
        //cs.playerState = CombatSystem.PlayerState.SelectingTargetToAttack;
        UI ui = UI.instance;
        ui.selectTargetUI.text = "Click a target to attack";
        ui.selectTargetUI.gameObject.SetActive(true);
        //Debug.Log("Targeting " + cs.enemiesInCombat[cs.currentTarget].className + " at location " + cs.currentTarget);
        //Debug.Log(Mouse.current.position.ReadValue());
    
    }

    public void OnItemButtonClicked()
    {
        //open inventory
        inv.ShowInventory(true);  
    }

    public void ShowCombatMenu(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
