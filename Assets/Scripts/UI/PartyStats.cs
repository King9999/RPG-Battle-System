using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* This script manages the party UI in the dungeon outside of combat. All of the data can be acquired from Hero Manager.*/
public class PartyStats : MonoBehaviour
{
    HeroManager hm;
    public Sprite[] heroSprites;
    public TextMeshProUGUI[] heroStats;                 //displays HP, MP, EXP and status
    // Start is called before the first frame update
    void Start()
    {
        hm = HeroManager.instance;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < hm.heroes.Count; i++)
        {
            Hero hero = hm.heroes[i];
            heroStats[i].text = hero.className + " - " + hero.status + "\n" + 
                "<color=#f65974>HP</color> " + hero.hitPoints + "/" + hero.maxHitPoints + "\n" + 
                "<color=#4be4fc>MP</color> " + hero.manaPoints + "/" + hero.maxManaPoints + "\n" +
                "<color=#ebca20>Next Lv</color> " + hero.xpToNextLevel;
        }
        
    }
}