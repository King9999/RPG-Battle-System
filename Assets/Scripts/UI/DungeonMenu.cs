using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This is used to access equip and consumable buttons
public class DungeonMenu : MonoBehaviour
{
    public Image border;
    public Button equipMenuButton;
    public Button consumableMenuButton;
    public Button backButton;           //used to go back to combat menu from different screens.
    public Inventory inv;

    public enum MenuState {Main, EquipMenuOpened, ConsumableMenuOpened, SelectingItemToEquip, SelectingHeroToTakeItem, SelectingHeroToUseSkill}
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEquipButtonClicked()
    {
        //open inventory
        if (menuState != MenuState.Main) return;

        inv.ShowInventory(true);

        //display window with equipment
        
        backButton.gameObject.SetActive(true);
        menuState = MenuState.EquipMenuOpened;  
    }

    public void OnConsumableButtonClicked()
    {
        if (menuState != MenuState.Main) return;
        
        menuState = MenuState.ConsumableMenuOpened;
    }
}
