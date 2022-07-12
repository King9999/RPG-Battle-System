using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Displays detailed avatar information when mouse hovers over them.
public class CombatStats : MonoBehaviour
{
    public TextMeshProUGUI detailedStats;
    public TextMeshProUGUI skillData;
    public TextMeshProUGUI itemData;
    public TextMeshProUGUI resistsValues;         //elemental resists
    public TextMeshProUGUI effectsData;         //current buffs/debuffs and their durations

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }


    public void DisplayStats(string statData, string skillData, string resistsData, string effectsData)
    {
        gameObject.SetActive(true);
        detailedStats.text = statData;
        this.skillData.text = skillData;
        resistsValues.text = resistsData;
        this.effectsData.text = effectsData;
    }

    public void DisplayStats(string statData, string skillData, string resistsData, string effectsData, string itemData)
    {
        gameObject.SetActive(true);
        detailedStats.text = statData;
        this.skillData.text = skillData;
        resistsValues.text = resistsData;
        this.effectsData.text = effectsData;
        this.itemData.text = itemData;
    }

    public void HideStats()
    {
        detailedStats.text = "";
        skillData.text = "";
        itemData.text = "";
        effectsData.text = "";
        resistsValues.text = "";
        gameObject.SetActive(false);
    }
}
