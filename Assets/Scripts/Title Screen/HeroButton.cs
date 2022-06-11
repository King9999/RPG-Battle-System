using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script will let the player click on a hero to choose them for the game. Hovering over the hero will give details.
public class HeroButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]public string heroInfo;
    //public enum HeroClass {Barbarian, Rogue, Mage, Cleric}
    //public HeroClass heroClass;
    public Hero heroPrefab;
    public HeroData heroData;

    public void OnPointerEnter(PointerEventData pointer)
    {
        //show text window & give hero details
        HeroDetails details = HeroDetails.instance;
        details.ShowWindow(true);
        details.ShowHeroDetails(heroInfo);
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        //hide text window
        HeroDetails details = HeroDetails.instance;
        details.ShowWindow(false);
    }

    public void OnHeroClicked()
    {
        //record which hero was selected and move to game screen.
        TitleManager tm = TitleManager.instance;
        tm.selectedHeroSprite = GetComponent<Image>().sprite;
        
        //Hero hero = Instantiate(heroPrefab);
        //hero.GetData(heroData);
        tm.selectedHeroData = heroData;
        

        SceneManager.LoadScene("Game");
    }
}
