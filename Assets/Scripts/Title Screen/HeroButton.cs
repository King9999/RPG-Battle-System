using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

//This script will let the player click on a hero to choose them for the game. Hovering over the hero will give details.
public class HeroButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]public string heroInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        SceneManager.LoadScene("Game");
    }
}
