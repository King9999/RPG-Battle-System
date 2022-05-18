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

    void Start()
    {
        //disable sprite display except the first one since that one will always be populated.
        /*for (int i = 1; i < heroSprites.Length; i++)
        {
            heroSprites[i].gameObject.SetActive(false);
        }*/
        heroSprite.gameObject.SetActive(false);
        
    }

    public void UpdateUI()
    {
        if (heroSprite)
        //hm = HeroManager.instance;
        //for (int i = 0; i < hm.heroes.Count; i++)
        //{
            //Hero hero = hm.heroes[i];
            heroStats.text = hero.className + " Lv " + hero.level + 
                "\n<color=#0fbe1f>Status</color> " +  hero.status +
                "\n<color=#f65974>HP</color> " + hero.hitPoints + "/" + hero.maxHitPoints + 
                "\n<color=#4be4fc>MP</color> " + hero.manaPoints + "/" + hero.maxManaPoints + 
                "\n<color=#ebca20>Next Lv</color> " + hero.xpToNextLevel;
        //}
        
    }

    public void SetSprite(Sprite sprite)
    {
        if (!heroSprite.gameObject.activeSelf)
            heroSprite.gameObject.SetActive(true);

        heroSprite.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData pointer)
    {
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        //check which hero the mouse is on.
    }

    public void OnPointerExit(PointerEventData pointer)
    {
    }
}
