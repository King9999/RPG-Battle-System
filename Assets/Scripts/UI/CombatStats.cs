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
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayStats(string statData, string skillData)
    {
        gameObject.SetActive(true);
        detailedStats.text = statData;
        this.skillData.text = skillData;
    }

    public void DisplayStats(string statData, string skillData, string itemData)
    {
        gameObject.SetActive(true);
        detailedStats.text = statData;
        this.skillData.text = skillData;
        this.itemData.text = itemData;
    }

    public void HideStats()
    {
        detailedStats.text = "";
        skillData.text = "";
        itemData.text = "";
        gameObject.SetActive(false);
    }
}
