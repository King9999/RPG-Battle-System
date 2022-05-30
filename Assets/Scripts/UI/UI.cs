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
    [HideInInspector]public Color healColor, damageColor, reducedDamageColor, criticalDamageColor;
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
        //damageDisplay.gameObject.SetActive(false);
        StopCoroutine(animateDamage);
        StartCoroutine(animateDamage);
    }
    public void DisplayDamage(string value, Vector3 location)
    {
        damageDisplay.color = Color.white;  //default color
        animateDamage = AnimateDamage(value, location);
        //damageDisplay.gameObject.SetActive(false);
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

    public void DisplayBlockResult(ShieldToken token)
    {
        StartCoroutine(AnimateBlock(token));
    }

    public void DisplayStatusUpdate(string status, Vector3 location)
    {
        StartCoroutine(AnimateStatus(status, location));
    }

    ///<param name="allStatusUiIndex">The text mesh to use to display information.</param>
    public void DisplayStatusUpdate(int allStatusUiIndex, string status, Vector3 location)
    {
        StartCoroutine(AnimateStatus(allStatusUiIndex, status, location));   
    }

    private IEnumerator AnimateDamage(string value, Vector3 location)
    {
        float displayDuration = 0.5f;
        Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        damageDisplay.gameObject.SetActive(true);
        damageDisplay.transform.position = avatarPos;
        damageDisplay.text = value;
        //damageDisplay.ForceMeshUpdate();            //this line is important! It ensures all relevant data is populated.

        //each digit is animated individually
        //TMP_CharacterInfo[] digits = new TMP_CharacterInfo[value.Length];
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
        //TODO: Animate digits individually! Need to modify the code below
        /*for (int i = 0; i < value.Length; i++)
        {
            
           digits[i] = damageDisplay.textInfo.characterInfo[i];    //getting each character in the string\
            Debug.Log("Contained char " + digits[i].character);

            //vertices of each character. These will be manipulated to animate each individual character
            Vector3[] digitVertices = damageDisplay.textInfo.meshInfo[digits[i].materialReferenceIndex].vertices;

            Debug.Log("character " + digits[i].character);
            //change position of digit by manipulating each vertex in the digit
            for (int j = 0; j < 4; j++)
            {
                Vector3 initVertexPos = digitVertices[digits[i].vertexIndex + j];
                Vector3 destination = new Vector3(initVertexPos.x, initVertexPos.y + 2, initVertexPos.z);

                while(digitVertices[digits[i].vertexIndex + j].y < destination.y)
                {
                    Vector3 newPos = digitVertices[digits[i].vertexIndex + j];
                    float vy = 5 * Time.deltaTime;
                    digitVertices[digits[i].vertexIndex + j] = new Vector3(newPos.x, newPos.y + vy, newPos.z);
                    yield return null; 
      
                }

                /*TMP_MeshInfo meshInfo = damageDisplay.textInfo.meshInfo[i];
                    meshInfo.mesh.vertices = meshInfo.vertices;
                    damageDisplay.UpdateGeometry(meshInfo.mesh, i);
               
               digitVertices[digits[i].vertexIndex + j] = destination;
            }
            
        }*/

         //update mesh vertices
        /*for (int i = 0; i < damageDisplay.textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = damageDisplay.textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            damageDisplay.UpdateGeometry(meshInfo.mesh, i);
        }*/

        
        yield return new WaitForSeconds(displayDuration);
        damageDisplay.gameObject.SetActive(false);
        animateDamageCoroutineOn = false;
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

    IEnumerator AnimateStatus(string status, Vector3 location)
    {
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

    IEnumerator AnimateStatus(int index, string status, Vector3 location)
    {
        //for (int i = 0; i < targets.Count; i++)
        //{
            float displayDuration = 0.5f;
            Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
            allStatusUI[index].gameObject.SetActive(true);
            allStatusUI[index].transform.position = avatarPos;
            allStatusUI[index].text = status;

            //each digit is animated individually
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
        //}
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
