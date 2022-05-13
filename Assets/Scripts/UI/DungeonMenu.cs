using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This is used to access equip and consumable buttons
public class DungeonMenu : MonoBehaviour
{
    public Image border;
    public Button weaponMenuButton;
    public Button armorMenuButton;
    public Button trinketMenuButton;
    public Button consumableMenuButton;
    public Button backButton;           //used to go back to combat menu from different screens.
    public Inventory inv;

    public enum MenuState {Main, ConsumableMenuOpened, WeaponMenuOpened, ArmorMenuOpened, TrinketMenuOpened, SelectingWeaponToEquip, 
                          SelectingArmorToEquip, SelectingTrinketToEquip, SelectingHeroToTakeItem, SelectingHeroToUseSkill}
    public MenuState menuState;

    //singletons
    public static DungeonMenu instance;
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
        backButton.gameObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnWeaponButtonClicked()
    {
        //open inventory
        if (menuState > MenuState.TrinketMenuOpened) return;    //this allows player to select other menus without closing current one.

        inv.ShowInventory(true);
        inv.HideAllSlots();
        inv.ShowWeaponSlots(true);
        backButton.gameObject.SetActive(true);
        menuState = MenuState.WeaponMenuOpened;  
    }

    public void OnConsumableButtonClicked()
    {
        if (menuState > MenuState.TrinketMenuOpened) return;
        inv.ShowInventory(true);
        inv.HideAllSlots();
        inv.ShowItemSlots(true);
        backButton.gameObject.SetActive(true);
        menuState = MenuState.ConsumableMenuOpened;
    }

    public void OnArmorButtonClicked()
    {
        if (menuState > MenuState.TrinketMenuOpened) return;
        inv.ShowInventory(true);
        inv.HideAllSlots();
        inv.ShowArmorSlots(true);
        backButton.gameObject.SetActive(true);
        menuState = MenuState.ArmorMenuOpened;
    }

    public void OnTrinketButtonClicked()
    {
        if (menuState > MenuState.TrinketMenuOpened) return;
        inv.ShowInventory(true);
        inv.HideAllSlots();
        inv.ShowTrinketSlots(true);
        backButton.gameObject.SetActive(true);
        menuState = MenuState.TrinketMenuOpened;
    }

    public void OnBackButtonClicked()
    {
        DungeonUI ui = DungeonUI.instance;
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
                inv.ShowInventory(true);
                menuState = MenuState.WeaponMenuOpened;
                break;

            case MenuState.SelectingArmorToEquip:
                inv.ShowInventory(true);
                menuState = MenuState.ArmorMenuOpened;
                break;

            case MenuState.SelectingTrinketToEquip:
                inv.ShowInventory(true);
                menuState = MenuState.TrinketMenuOpened;
                break;
            
            case MenuState.SelectingHeroToTakeItem:
                inv.ShowInventory(true);
                menuState = MenuState.ConsumableMenuOpened;
                break;
        }
    }
}
