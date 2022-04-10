using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public List<TextMeshProUGUI> heroStats;     //displays name, hp and mp
    public List<TextMeshProUGUI> enemyStats;    //displays name and hp
    public TextMeshProUGUI damageDisplay;
    [HideInInspector]public Color healColor;
    [HideInInspector]public Color damageColor;
    [HideInInspector]public Color reducedDamageColor;
    [HideInInspector]public Color criticalDamageColor;
    int maxPartySize {get;} = 3;
    int maxEnemyPartySize {get;} = 6;

    public SkillDisplay skillDisplay;

    public static UI instance;

    //coroutine checks
    [HideInInspector]public bool animateDamageCoroutineOn;

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
        healColor = new Color(0, 0.9f, 0.3f);
        damageColor = Color.white;
        reducedDamageColor = new Color(0.6f, 0.6f, 0.6f);
        criticalDamageColor = new Color(1, 0.8f, 0.2f);
        damageDisplay.gameObject.SetActive(false);
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

    public void DisplayDamage(string value, Color textColor)
    {
        damageDisplay.color = textColor;
        StartCoroutine(AnimateDamage(value));
    }
    public void DisplayDamage(string value)
    {
        damageDisplay.color = Color.white;  //default color
        StartCoroutine(AnimateDamage(value));
    }
    private IEnumerator AnimateDamage(string value)
    {
        animateDamageCoroutineOn = true;
        damageDisplay.gameObject.SetActive(true);
        damageDisplay.text = value;
        
        yield return null;

        damageDisplay.gameObject.SetActive(false);
        animateDamageCoroutineOn = false;
    }
}
