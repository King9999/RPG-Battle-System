using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    //public List<TextMeshProUGUI> heroStats;     //displays name, hp and mp
    //public List<TextMeshProUGUI> enemyStats;    //displays name and hp
    public TextMeshProUGUI turnOrderList;
    public TextMeshProUGUI damageDisplay;
    public TextMeshProUGUI[] allDamageDisplay;      //used for multiple targets
    public TextMeshProUGUI selectTargetUI;
    public TextMeshProUGUI shieldBlockUI;
    public TextMeshProUGUI statusUI;               //used to display ailments/buffs/debuff notifications
    public TextMeshProUGUI[] allStatusUI;             //used for multiple targets.
    public TextMeshProUGUI bonusListUI;             //displays active bonuses from breaking shields
    //public TMP_Text damageDisplayComponent;
    //[SerializeField]TextMeshProUGUI[] damageDigits;
    [HideInInspector]public Color healColor, healManaColor, damageColor, reducedDamageColor, criticalDamageColor;
    int maxPartySize {get;} = 3;
    int maxEnemyPartySize {get;} = 6;

    public SkillDisplay skillDisplay;
    public CombatStats combatDataDisplay;
    public CombatMenu combatMenu;
    public RewardsDisplay rewardsDisplay;
    public Inventory inventory;

    public static UI instance;
    CombatSystem cs;

    //coroutine checks
    public bool animateDamageCoroutineOn {get; set;}
    IEnumerator animateDamage;

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
        healManaColor = new Color(0, 0.8f, 1);
        damageColor = Color.white;
        reducedDamageColor = new Color(0.6f, 0.6f, 0.6f);
        criticalDamageColor = new Color(1, 0.8f, 0.2f);
        damageDisplay.color = damageColor;
        
        damageDisplay.gameObject.SetActive(false);
        combatMenu.gameObject.SetActive(false);
        selectTargetUI.gameObject.SetActive(false);
        shieldBlockUI.gameObject.SetActive(false);
        statusUI.gameObject.SetActive(false);

        for (int i = 0; i < allStatusUI.Length; i++)
        {
            allStatusUI[i].gameObject.SetActive(false);
            allDamageDisplay[i].gameObject.SetActive(false);
        }
        //bonusListUI.gameObject.SetActive(false);

        //set up bonus list
        bonusListUI.text = "<color=#c827d8>BONUSES</color>\n";

        cs = CombatSystem.instance;
    }

    public void DisplayDamage(string value, Vector3 location, Color textColor)
    {
        damageDisplay.color = textColor;
        animateDamage = AnimateDamage(value, location);
        StopCoroutine(animateDamage);
        StartCoroutine(animateDamage);
    }
    public void DisplayDamage(int allDamageDisplayIndex, string value, Vector3 location, Color textColor)
    {
        allDamageDisplay[allDamageDisplayIndex].color = textColor;
        animateDamage = AnimateDamage(allDamageDisplayIndex, value, location);
        StopCoroutine(animateDamage);
        StartCoroutine(animateDamage);
    }

    public void DisplayHealing(string value, Vector3 location, Color textColor, float delayDuration = 0)
    {
        damageDisplay.color = textColor;  //default color
        animateDamage = AnimateHealing(value, location, textColor, delayDuration);
        StopCoroutine(animateDamage);
        StartCoroutine(animateDamage);
    }

    public void DisplayHealing(int allHealDisplayIndex, string value, Vector3 location, Color textColor)
    {
        damageDisplay.color = textColor;  //default color
        animateDamage = AnimateHealing(allHealDisplayIndex, value, location, textColor);
        StopCoroutine(animateDamage);
        StartCoroutine(animateDamage);
    }

    public void DisplayBlockResult(ShieldToken token)
    {
        StartCoroutine(AnimateBlock(token));
    }

    public void DisplayStatusUpdate(string status, Vector3 location, float delayDuration = 0)
    {
        StartCoroutine(AnimateStatus(status, location, delayDuration));
    }

    ///<param name="allStatusUiIndex">The text mesh to use to display information.</param>
    public void DisplayStatusUpdate(int allStatusUiIndex, string status, Vector3 location, float delayDuration = 0)
    {
        StartCoroutine(AnimateStatus(allStatusUiIndex, status, location, delayDuration));   
    }

    private IEnumerator AnimateDamage(string value, Vector3 location)
    {
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        damageDisplay.gameObject.SetActive(true);
        damageDisplay.transform.position = avatarPos;
        damageDisplay.text = value;

        //each digit is animated individually
        Vector3 initPos = damageDisplay.transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        float gravity = -0.4f;
        while(damageDisplay.transform.position.y < destination.y)
        {
            Vector3 newPos = damageDisplay.transform.position;
            vy = gravity + 400 * Time.deltaTime;
            damageDisplay.transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        damageDisplay.transform.position = initPos;
        
        yield return new WaitForSeconds(displayDuration);
        damageDisplay.gameObject.SetActive(false);
        animateDamageCoroutineOn = false;
    }

    private IEnumerator AnimateDamage(int index, string value, Vector3 location)
    {
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        allDamageDisplay[index].gameObject.SetActive(true);
        allDamageDisplay[index].transform.position = avatarPos;
        allDamageDisplay[index].text = value;

        Vector3 initPos = allDamageDisplay[index].transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        float gravity = -0.4f;
        while(allDamageDisplay[index].transform.position.y < destination.y)
        {
            Vector3 newPos = allDamageDisplay[index].transform.position;
            vy = gravity + 400 * Time.deltaTime;
            allDamageDisplay[index].transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        allDamageDisplay[index].transform.position = initPos;
        yield return new WaitForSeconds(displayDuration);
        allDamageDisplay[index].gameObject.SetActive(false);
    }

    //delay duration is used to display values later to prevent overlapping with other UI
    private IEnumerator AnimateHealing(string value, Vector3 location, Color textColor, float delayDuration = 0)
    {
        yield return new WaitForSeconds(delayDuration);
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        damageDisplay.gameObject.SetActive(true);
        damageDisplay.transform.position = avatarPos;
        damageDisplay.text = value;
        damageDisplay.color = textColor;

        //each digit is animated individually
        Vector3 initPos = damageDisplay.transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        while(damageDisplay.transform.position.y < destination.y)
        {
            Vector3 newPos = damageDisplay.transform.position;
            vy = 50 * Time.deltaTime;
            damageDisplay.transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        damageDisplay.transform.position = destination;
        
        yield return new WaitForSeconds(displayDuration);
        damageDisplay.color = damageColor;        //reset back to default
        damageDisplay.gameObject.SetActive(false);
        animateDamageCoroutineOn = false;
    }

    //used when HP is being restored to multiple targets
    private IEnumerator AnimateHealing(int index, string value, Vector3 location, Color textColor)
    {
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        allDamageDisplay[index].gameObject.SetActive(true);
        allDamageDisplay[index].transform.position = avatarPos;
        allDamageDisplay[index].text = value;
        allDamageDisplay[index].color = textColor;

        //each digit is animated individually
        Vector3 initPos = allDamageDisplay[index].transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        while(allDamageDisplay[index].transform.position.y < destination.y)
        {
            Vector3 newPos = allDamageDisplay[index].transform.position;
            vy = 50 * Time.deltaTime;
            allDamageDisplay[index].transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        allDamageDisplay[index].transform.position = destination;
        
        yield return new WaitForSeconds(displayDuration);
        allDamageDisplay[index].color = damageColor;        //reset back to default
        allDamageDisplay[index].gameObject.SetActive(false);
        animateDamageCoroutineOn = false;
    }

    IEnumerator AnimateStatus(string status, Vector3 location, float delayDuration = 0)
    {
        yield return new WaitForSeconds(delayDuration);
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        statusUI.gameObject.SetActive(true);
        statusUI.transform.position = avatarPos;
        statusUI.text = status;

        //each digit is animated individually
        Vector3 initPos = statusUI.transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        while(statusUI.transform.position.y < destination.y)
        {
            Vector3 newPos = statusUI.transform.position;
            vy = 50 * Time.deltaTime;
            statusUI.transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        statusUI.transform.position = destination;
        
        yield return new WaitForSeconds(displayDuration);
        statusUI.gameObject.SetActive(false);
    }

    IEnumerator AnimateStatus(int index, string status, Vector3 location, float delayDuration = 0)
    {
        yield return new WaitForSeconds(delayDuration);
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        allStatusUI[index].gameObject.SetActive(true);
        allStatusUI[index].transform.position = avatarPos;
        allStatusUI[index].text = status;

        //animate damage values
        Vector3 initPos = allStatusUI[index].transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        while(allStatusUI[index].transform.position.y < destination.y)
        {
            Vector3 newPos = allStatusUI[index].transform.position;
            vy = 50 * Time.deltaTime;
            allStatusUI[index].transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        allStatusUI[index].transform.position = destination;
        
        yield return new WaitForSeconds(displayDuration);
        allStatusUI[index].gameObject.SetActive(false);
       
    }

    //shows block animation. also reduces shield HP. If shield has 0 HP, a different animation is played.
    private IEnumerator AnimateBlock(ShieldToken token)
    {
        shieldBlockUI.gameObject.SetActive(true);

        shieldBlockUI.transform.position = token.transform.position;
        Vector3 destination = shieldBlockUI.transform.position;

        //check shield state
        if (token.hitPoints > 0)
            shieldBlockUI.text = "BLOCKED\n<color=green>" + token.hitPoints + "</color> SHIELD HP";
        else
            shieldBlockUI.text = "SHATTERED!";
        
        //animate text
        while(shieldBlockUI.transform.position.y > destination.y - 10)
        {
            float vy = 10 * Time.deltaTime;
            Vector3 newPos = shieldBlockUI.transform.position;
            shieldBlockUI.transform.position = new Vector3(newPos.x, newPos.y - vy, newPos.z);
            yield return null;
        }

        shieldBlockUI.gameObject.SetActive(false);

        //speed up tokens
        cs.heroesInCombat[cs.currentHero].SpeedUpToken();
    }
}
