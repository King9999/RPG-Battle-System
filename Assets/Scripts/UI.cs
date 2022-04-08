using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public List<TextMeshProUGUI> heroStats;     //displays name, hp and mp
    public List<TextMeshProUGUI> enemyStats;    //displays name and hp
    int maxPartySize {get;} = 3;
    int maxEnemyPartySize {get;} = 6;

    public SkillDisplay skillDisplay;

    public static UI instance;

   private void Awake()
   {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);    //Only want one instance of UI
            return;
        }

        instance = this;
   }

    // Start is called before the first frame update
    void Start()
    {
        //heroStats = new TextMeshProUGUI[maxPartySize];
        //enemyStats = new TextMeshProUGUI[maxEnemyPartySize];
    }

    // Update is called once per frame
    void Update()
    {
        //use the following lines to display skill name
        //if (!skillDisplay.displaySkillCoroutineOn)
            //StartCoroutine(skillDisplay.DisplaySkillName("Test", Color.red));
    }
}
