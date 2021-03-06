using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//This is used to access equip and consumable buttons
public class DungeonMenu : MonoBehaviour
{
    public Image border;
    public Button weaponMenuButton;
    public Button armorMenuButton;
    public Button trinketMenuButton;
    public Button consumableMenuButton;
    public Button backButton;           //used to go back to combat menu from different screens.
    public Image weaponIcon;
    public Image armorIcon;             //when an item is selected, one of the icons follows the mouse cursor.
    public Image trinketIcon;
    public Image consumableIcon;
    Image itemIcon;                 //the image that will follow the mouse.
    Inventory inv;

    public enum MenuState {Main, ConsumableMenuOpened, WeaponMenuOpened, ArmorMenuOpened, TrinketMenuOpened, SelectingWeaponToEquip, 
                          SelectingArmorToEquip, SelectingTrinketToEquip, SelectingHeroToTakeItem, SelectingHeroToUseSkill}
    public MenuState menuState;

    //singletons
    public static DungeonMenu instance;
    DungeonUI ui;

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
        backButton.gameObject.SetActive(false);
        HideAllIcons();   
    }

    // Update is called once per frame
    void Update()
    {
        //icon follows mouse cursor
        ui = DungeonUI.instance;
        if (!consumableIcon.gameObject.activeSelf && menuState == MenuState.SelectingHeroToTakeItem)
        {
            HideAllIcons();
            itemIcon = ShowIcon(consumableIcon);
        }

        if (!weaponIcon.gameObject.activeSelf && menuState == MenuState.SelectingWeaponToEquip)
        {
            HideAllIcons();
            itemIcon = ShowIcon(weaponIcon);
        }

        if (!trinketIcon.gameObject.activeSelf && menuState == MenuState.SelectingTrinketToEquip)
        {
            HideAllIcons();
            itemIcon = ShowIcon(trinketIcon);
        }

        if (!armorIcon.gameObject.activeSelf && menuState == MenuState.SelectingArmorToEquip)
        {
            HideAllIcons();
            itemIcon = ShowIcon(armorIcon);
        }

        if (itemIcon != null)
            itemIcon.transform.position = Mouse.current.position.ReadValue();
    }

    Image ShowIcon(Image icon)
    {
        icon.gameObject.SetActive(true);
        return icon;
    }

    void HideAllIcons()
    {
        weaponIcon.gameObject.SetActive(false);
        trinketIcon.gameObject.SetActive(false);
        armorIcon.gameObject.SetActive(false);
        consumableIcon.gameObject.SetActive(false);
    }

    public void SetState(MenuState state)
    {
        inv = Inventory.instance;
        ui = DungeonUI.instance;
        menuState = state;
        switch(menuState)
        {
            case MenuState.Main:
                HideAllIcons();
                itemIcon = null;
                inv.HideAllSlots();
                inv.ShowInventory(false);
                ui.selectTargetUI.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                break;
            
            case MenuState.ConsumableMenuOpened:
                if (menuState > MenuState.TrinketMenuOpened) return; //this allows player to select other menus without closing current one.
                inv.ShowInventory(true);
                inv.HideAllSlots();
                HideAllIcons();
                itemIcon = null;
                inv.ShowItemSlots(true);
                ui.selectTargetUI.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
                break;
            
            case MenuState.WeaponMenuOpened:
                if (menuState > MenuState.TrinketMenuOpened) return;    
                inv.ShowInventory(true);
                inv.HideAllSlots();
                HideAllIcons();
                itemIcon = null;
                inv.ShowWeaponSlots(true);
                inv.statsDisplay.ShowDisplay(false);
                ui.selectTargetUI.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
                break;
            
            case MenuState.ArmorMenuOpened:
                if (menuState > MenuState.TrinketMenuOpened) return;
                inv.ShowInventory(true);
                inv.HideAllSlots();
                HideAllIcons();
                itemIcon = null;
                inv.ShowArmorSlots(true);
                inv.statsDisplay.ShowDisplay(false);
                ui.selectTargetUI.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
                break;

            case MenuState.TrinketMenuOpened:
                if (menuState > MenuState.TrinketMenuOpened) return;
                inv.ShowInventory(true);
                inv.HideAllSlots();
                HideAllIcons();
                itemIcon = null;
                inv.ShowTrinketSlots(true);
                inv.statsDisplay.ShowDisplay(false);
                ui.selectTargetUI.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
                break;

            case MenuState.SelectingWeaponToEquip:
            case MenuState.SelectingArmorToEquip:
            case MenuState.SelectingTrinketToEquip:
                inv.ShowInventory(false);
                inv.statsDisplay.ShowDisplay(true); 
                ui.selectTargetUI.gameObject.SetActive(true);
                break;

            
            case MenuState.SelectingHeroToTakeItem:
            case MenuState.SelectingHeroToUseSkill:
                ui.selectTargetUI.gameObject.SetActive(true);
                break;
        
        }   
    }

    public void OnWeaponButtonClicked()
    {
        SetState(MenuState.WeaponMenuOpened);  
    }

    public void OnConsumableButtonClicked()
    {
        SetState(MenuState.ConsumableMenuOpened);
    }

    public void OnArmorButtonClicked()
    {
        SetState(MenuState.ArmorMenuOpened);
    }

    public void OnTrinketButtonClicked()
    {
        SetState(MenuState.TrinketMenuOpened);
    }

    public void OnBackButtonClicked()
    {
        ui = DungeonUI.instance;
        inv = Inventory.instance;
        //check which state we're on
        switch(menuState)
        {
            case MenuState.WeaponMenuOpened:
                inv.ShowInventory(false);
                inv.ShowWeaponSlots(false);
                backButton.gameObject.SetActive(false);
                menuState = MenuState.Main;
                break;

            case MenuState.ConsumableMenuOpened:
                //close
                inv.ShowInventory(false);
                inv.ShowItemSlots(false);
                backButton.gameObject.SetActive(false);
                menuState = MenuState.Main;
                break;

            case MenuState.ArmorMenuOpened:
                //close
                inv.ShowInventory(false);
                inv.ShowArmorSlots(false);
                backButton.gameObject.SetActive(false);
                menuState = MenuState.Main;
                break;

            case MenuState.TrinketMenuOpened:
                //close
                inv.ShowInventory(false);
                inv.ShowTrinketSlots(false);
                backButton.gameObject.SetActive(false);
                menuState = MenuState.Main;
                break;

            case MenuState.SelectingWeaponToEquip:
                ui.selectTargetUI.gameObject.SetActive(false);
                HideAllIcons();
                itemIcon = null;
                SetState(MenuState.WeaponMenuOpened);
                break;

            case MenuState.SelectingArmorToEquip:
                ui.selectTargetUI.gameObject.SetActive(false);
                HideAllIcons();
                itemIcon = null;
                SetState(MenuState.ArmorMenuOpened);
                break;

            case MenuState.SelectingTrinketToEquip:
                ui.selectTargetUI.gameObject.SetActive(false);
                HideAllIcons();
                itemIcon = null;
                SetState(MenuState.TrinketMenuOpened);
                break;
            
            case MenuState.SelectingHeroToTakeItem:
                ui.selectTargetUI.gameObject.SetActive(false);
                HideAllIcons();
                itemIcon = null;
                menuState = MenuState.ConsumableMenuOpened;
                break;
        }
    }
}
