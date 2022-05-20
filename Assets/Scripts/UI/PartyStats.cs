using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/* This script manages the party UI in the dungeon outside of combat. All of the data can be acquired from Hero Manager.*/
public class PartyStats : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //HeroManager hm;
    public Hero hero;
    //public Image[] heroSprites;
    public Image heroSprite;
    public TextMeshProUGUI heroStatsUI;                 //displays HP, MP, EXP and status
    public HeroStatsDisplay statsDisplay;
    public Image background;
    Color normalColor;
    Color highlightColor;
    public int heroID {get; set;}                      //used to target heroes for item use. Corresponds to HeroManager index.


    //singletons
    DungeonMenu menu;
    DungeonUI ui;
    Inventory inv;

    void Start()
    {
        //disable sprite display except the first one since that one will always be populated.
        //heroSprite.gameObject.SetActive(false);
        normalColor = new Color(0.08f, 0.13f, 0.5f, 0.4f);
        highlightColor = new Color(0.8f, 0.2f, 0.2f, 0.4f);
        statsDisplay.ShowDisplay(false);
        heroID = 0;
    }

    public void UpdateUI()
    {
        
        heroStatsUI.text = hero.className + " Lv " + hero.level + 
            "\n<color=#0fbe1f>Status</color> " +  hero.status +
            "\n<color=#f65974>HP</color> " + hero.hitPoints + "/" + hero.maxHitPoints + 
            "\n<color=#4be4fc>MP</color> " + hero.manaPoints + "/" + hero.maxManaPoints + 
            "\n<color=#ebca20>Next Lv</color> " + hero.xpToNextLevel;
    
    }

    public void SetSprite(Sprite sprite)
    {
        if (!heroSprite.gameObject.activeSelf)
            heroSprite.gameObject.SetActive(true);

        heroSprite.sprite = sprite;
    }

    public void ShowSprite(bool toggle)
    {
        heroSprite.gameObject.SetActive(toggle);
    }

    public void OnPointerClick(PointerEventData pointer)
    {
        //equip or use an item
        inv = Inventory.instance;
        menu = DungeonMenu.instance;
        ui = DungeonUI.instance;

        if (hero != null && ui.currentHero >= 0 && menu.menuState == DungeonMenu.MenuState.SelectingHeroToTakeItem)
        {
            if (inv.copiedSlot.TryGetComponent(out ItemSlot iSlot))
            {
                //apply item effect
                HeroManager hm = HeroManager.instance;
                iSlot.ItemInSlot().itemEffect.Activate(hm.heroes[heroID]);
                inv.RemoveItem(iSlot.ItemInSlot(), 1);
                menu.SetState(DungeonMenu.MenuState.ConsumableMenuOpened);
            }
        }

        if (hero != null && (menu.menuState == DungeonMenu.MenuState.SelectingWeaponToEquip || menu.menuState == DungeonMenu.MenuState.SelectingArmorToEquip
            || menu.menuState == DungeonMenu.MenuState.SelectingTrinketToEquip))
        {
            
            if (inv.copiedSlot.TryGetComponent(out WeaponSlot wSlot))
            {
                Debug.Log(wSlot.WeaponInSlot().itemName + " equipped!");
                Weapon oldWeapon = hero.weapon;

                if (wSlot.WeaponInSlot().CanBeEquipped(hero))
                {
                    wSlot.WeaponInSlot().Equip(hero);
                    statsDisplay.UpdateStats(hero);
                    inv.statsDisplay.UpdateStats(hero, hero.weapon);    //showing updated stats
                    inv.RemoveItem(wSlot.WeaponInSlot(), 1);

                    if (oldWeapon != null)
                        inv.AddItem(oldWeapon, 1);
                    ui.ConfirmEquip("Equipped!", DungeonMenu.MenuState.WeaponMenuOpened);
                }
                /*else
                {
                    Debug.Log(wSlot.WeaponInSlot().itemName + " can't be equipped by " + hero.className);
                    //ui.DisplayEquipStatus(true, "<color=red>Can't equip!</color>");
                }*/

                //TODO: send player back to inventory after briefly showing an "Equipped" message
                //menu.SetState(DungeonMenu.MenuState.WeaponMenuOpened);
            }

            if (inv.copiedSlot.TryGetComponent(out ArmorSlot aSlot))
            {
                Debug.Log(aSlot.ArmorInSlot().itemName + " equipped!");
                Armor oldArmor = hero.armor == null ? null : hero.armor;
                aSlot.ArmorInSlot().Equip(hero);
                inv.RemoveItem(aSlot.ArmorInSlot(), 1);

                if (oldArmor != null)
                    inv.AddItem(oldArmor, 1);

                inv.statsDisplay.UpdateStats(hero, hero.armor);    //showing updated stats
                statsDisplay.UpdateStats(hero);
                
                //TODO: send player back to inventory after briefly showing an "Equipped" message
                menu.SetState(DungeonMenu.MenuState.ArmorMenuOpened);
            }

            if (inv.copiedSlot.TryGetComponent(out TrinketSlot tSlot))
            {
                Debug.Log(tSlot.TrinketInSlot().itemName + " equipped!");
                Trinket oldTrinket = hero.trinket;
                tSlot.TrinketInSlot().Equip(hero);
                inv.RemoveItem(tSlot.TrinketInSlot(), 1);

                if (oldTrinket != null)
                    inv.AddItem(oldTrinket, 1);

                inv.statsDisplay.UpdateStats(hero, hero.trinket);    //showing updated stats
                statsDisplay.UpdateStats(hero);
                
                //TODO: send player back to inventory after briefly showing an "Equipped" message
                menu.SetState(DungeonMenu.MenuState.TrinketMenuOpened);
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        menu = DungeonMenu.instance;
        ui = DungeonUI.instance;
        if (hero != null && menu.menuState == DungeonMenu.MenuState.Main)
        {
            //display detailed stats next to the hero
            background.color = highlightColor;
            statsDisplay.ShowDisplay(true);
            Vector3 newPos = transform.position;
            statsDisplay.transform.position = new Vector3(statsDisplay.transform.position.x, newPos.y, statsDisplay.transform.position.z);
            statsDisplay.UpdateStats(hero);
        }

        //using item
        if (hero != null && menu.menuState == DungeonMenu.MenuState.SelectingHeroToTakeItem)
        {
            ui.currentHero = heroID;
            Debug.Log("Hero ID is " + heroID);
        }

        //equipment
        if (hero != null && (menu.menuState == DungeonMenu.MenuState.SelectingWeaponToEquip || menu.menuState == DungeonMenu.MenuState.SelectingArmorToEquip
            || menu.menuState == DungeonMenu.MenuState.SelectingTrinketToEquip))
        {
            //update equip stats display. Need to get the correct slot type to access the correct item.
            inv = Inventory.instance;
            if (inv.copiedSlot.TryGetComponent(out WeaponSlot wSlot))
            {
                inv.statsDisplay.UpdateStats(hero, wSlot.WeaponInSlot());
                if (wSlot.WeaponInSlot().CanBeEquipped(hero))
                {
                    ui.DisplayEquipStatus(true, "Can equip");
                }
                else
                {
                    ui.DisplayEquipStatus(true, "<color=red>Cannot equip</color>");
                }
            }
            if (inv.copiedSlot.TryGetComponent(out ArmorSlot aSlot))
            {
                inv.statsDisplay.UpdateStats(hero, aSlot.ArmorInSlot());
            }
            if (inv.copiedSlot.TryGetComponent(out TrinketSlot tSlot))
            {
                inv.statsDisplay.UpdateStats(hero, tSlot.TrinketInSlot());
            }
        }

    }

    public void OnPointerExit(PointerEventData pointer)
    {
        //hide hero stats
        background.color = normalColor;
        statsDisplay.ShowDisplay(false);
        DungeonUI ui = DungeonUI.instance;
        ui.currentHero = -1;

        //hide equip stats
        menu = DungeonMenu.instance;
        if (menu.menuState == DungeonMenu.MenuState.SelectingWeaponToEquip || menu.menuState == DungeonMenu.MenuState.SelectingArmorToEquip
            || menu.menuState == DungeonMenu.MenuState.SelectingTrinketToEquip)
        {
            inv = Inventory.instance;
            inv.statsDisplay.ClearDisplay();
            ui.DisplayEquipStatus(false);
        }
    }
}
