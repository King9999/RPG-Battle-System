using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//opens up a menu with options to choose from. The menu options are: Attack, Skill, Item, Escape
public class CombatMenu : MonoBehaviour
{
    
    public Image border;
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button escapeButton;
    public Button backButton;           //used to go back to combat menu from different screens.\
    public Transform backButtonInventoryPos;
    [HideInInspector]public Vector3 originalBackButtonPos;
    public Inventory inv;

    public enum MenuState {Main, SelectingTargetToAttack, InventoryOpened, SelectingHeroToTakeItem, SelectingSkillTarget, EscapeConfirmWindowOpen}
    public MenuState menuState;

    //singletons
    public static CombatMenu instance;
    CombatSystem cs;
    UI ui;

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
        cs = CombatSystem.instance;
        ui = UI.instance;
        gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        menuState = MenuState.Main;
        UpdateMenuUI(menuState);
        originalBackButtonPos = backButton.transform.position;
    }

   
    public void OnAttackButtonClicked()
    {
        //player selects a target
        if (menuState != MenuState.Main) 
            return;
        
        cs.selectingTargetToAttack = true;
        ui.selectTargetUI.text = "Click a target to attack";
        ui.selectTargetUI.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        backButton.transform.position = originalBackButtonPos;
        menuState = MenuState.SelectingTargetToAttack;
    }

    public void OnItemButtonClicked()
    {
        //open inventory
        if (menuState != MenuState.Main) 
            return;

        inv.ShowInventory(true);
        inv.HideAllSlots();
        //itemIcon = null;
        inv.ShowItemSlots(true);
        backButton.gameObject.SetActive(true);
        backButton.transform.position = backButtonInventoryPos.position;
        menuState = MenuState.InventoryOpened;  
    }

    public void OnSkillButtonClicked()
    {
        //open inventory
        if (menuState != MenuState.Main) 
            return;

        inv.ShowInventory(true);
        inv.HideAllSlots();
        //itemIcon = null;
        inv.ShowSkillSlots(true);
        backButton.gameObject.SetActive(true);
        backButton.transform.position = backButtonInventoryPos.position;
        menuState = MenuState.InventoryOpened;  
    }

    public void OnEscapeButtonClicked()
    {
        //open inventory
        if (menuState != MenuState.Main) 
            return;
            
        ShowCombatMenu(false);
        cs.playerRanAway = true;
    }

    public void ShowCombatMenu(bool toggle)
    {
        gameObject.SetActive(toggle);
        backButton.gameObject.SetActive(false);
        backButton.transform.position = originalBackButtonPos;
        menuState = MenuState.Main;
    }

    public void UpdateMenuUI(MenuState state)
    {
        switch(state)
        {
            case MenuState.Main:

                break;
        }
    }

    public void OnBackButtonClicked()
    {
        //check which state we're on
        switch(menuState)
        {
            case MenuState.SelectingTargetToAttack:
                cs.selectingTargetToAttack = false;
                ui.selectTargetUI.text = "";
                ui.selectTargetUI.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                backButton.transform.position = originalBackButtonPos;
                menuState = MenuState.Main;
                break;

            case MenuState.InventoryOpened:
                //close
                inv.ShowInventory(false);
                backButton.gameObject.SetActive(false);
                backButton.transform.position = originalBackButtonPos;
                menuState = MenuState.Main;
                break;
            
            case MenuState.SelectingHeroToTakeItem:
                cs.selectingHero = false;
                inv.ShowInventory(true);
                inv.copiedSlot = null;
                backButton.transform.position = backButtonInventoryPos.transform.position;
                ui.selectTargetUI.text = "";
                ui.selectTargetUI.gameObject.SetActive(false);
                menuState = MenuState.InventoryOpened;
                break;

            case MenuState.SelectingSkillTarget:
                cs.selectingTargetToAttackWithSkill = false;
                cs.selectingHeroToUseSkillOn = false;
                ui.selectTargetUI.text = "";
                inv.ShowInventory(true);
                inv.copiedSkillSlot = null;
                ui.selectTargetUI.gameObject.SetActive(false);
                //backButton.gameObject.SetActive(false);
                backButton.transform.position = backButtonInventoryPos.transform.position;
                menuState = MenuState.InventoryOpened;
                break;
        }
    }
}
