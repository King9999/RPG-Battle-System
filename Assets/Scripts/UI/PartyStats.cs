using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* This script manages the party UI in the dungeon outside of combat. All of the data can be acquired from Hero Manager.*/
public class PartyStats : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //HeroManager hm;
    public Hero hero;
    //public Image[] heroSprites;
    public Image heroSprite;
    public TextMeshProUGUI heroStats;                 //displays HP, MP, EXP and status
    public HeroStatsDisplay statsDisplay;
    public Image background;
    Color normalColor;
    Color highlightColor;

    //singletons
    DungeonMenu menu;
    Inventory inv;

    void Start()
    {
        //disable sprite display except the first one since that one will always be populated.
        //heroSprite.gameObject.SetActive(false);
        normalColor = new Color(0.08f, 0.13f, 0.5f, 0.4f);
        highlightColor = new Color(0.8f, 0.2f, 0.2f, 0.4f);
        statsDisplay.ShowDisplay(false);
    }

    public void UpdateUI()
    {
        
        heroStats.text = hero.className + " Lv " + hero.level + 
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
        if (inv.copiedSlot.TryGetComponent(out WeaponSlot wSlot))
        {
            Debug.Log(wSlot.WeaponInSlot().itemName + " equipped!");
            Weapon oldWeapon = hero.weapon;
            wSlot.WeaponInSlot().Equip(hero);
            inv.RemoveItem(wSlot.WeaponInSlot(), 1);
            
            if (oldWeapon != null)
                inv.AddItem(oldWeapon, 1);

            inv.statsDisplay.UpdateStats(hero, hero.weapon);    //showing new weapon
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

    public void OnPointerEnter(PointerEventData pointer)
    {
        menu = DungeonMenu.instance;
        if (hero != null && menu.menuState == DungeonMenu.MenuState.Main)
        {
            //display detailed stats next to the hero
            background.color = highlightColor;
            statsDisplay.ShowDisplay(true);
            Vector3 newPos = transform.position;
            statsDisplay.transform.position = new Vector3(statsDisplay.transform.position.x, newPos.y, statsDisplay.transform.position.z);
            statsDisplay.UpdateStats(hero);
        }

        if (hero != null && (menu.menuState == DungeonMenu.MenuState.SelectingWeaponToEquip || menu.menuState == DungeonMenu.MenuState.SelectingArmorToEquip
            || menu.menuState == DungeonMenu.MenuState.SelectingTrinketToEquip))
        {
            //update equip stats display. Need to get the correct slot type to access the correct item.
            inv = Inventory.instance;
            if (inv.copiedSlot.TryGetComponent(out WeaponSlot wSlot))
            {
                inv.statsDisplay.UpdateStats(hero, wSlot.WeaponInSlot());
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

        //hide equip stats
        menu = DungeonMenu.instance;
        if (menu.menuState == DungeonMenu.MenuState.SelectingWeaponToEquip || menu.menuState == DungeonMenu.MenuState.SelectingArmorToEquip
            || menu.menuState == DungeonMenu.MenuState.SelectingTrinketToEquip)
        {
            inv = Inventory.instance;
            inv.statsDisplay.ClearDisplay();
        }
    }
}
