using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public List<TextMeshProUGUI> heroStats;     //displays name, hp and mp
    public List<TextMeshProUGUI> enemyStats;    //displays name and hp
    public TextMeshProUGUI damageDisplay;
    //public TMP_Text damageDisplayComponent;
    [SerializeField]TextMeshProUGUI[] damageDigits;
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

    public void DisplayHealing(string value, Vector3 location)
    {
        damageDisplay.color = healColor;  //default color
        animateDamage = AnimateHealing(value, location);
        StopCoroutine(animateDamage);
        StartCoroutine(animateDamage);
    }

    private IEnumerator AnimateDamage(string value, Vector3 location)
    {
        float displayDuration = 0.5f;
        //animateDamageCoroutineOn = true;
        damageDisplay.gameObject.SetActive(true);
        damageDisplay.transform.position = location;
        damageDisplay.text = value;
        //damageDisplay.ForceMeshUpdate();            //this line is important! It ensures all relevant data is populated.

        //each digit is animated individually
        //TMP_CharacterInfo[] digits = new TMP_CharacterInfo[value.Length];
        Debug.Log("Length " + damageDisplay.text.Length);
        Vector3 initPos = damageDisplay.transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        float gravity = -0.4f;
        while(damageDisplay.transform.position.y < destination.y)
        {
            Vector3 newPos = damageDisplay.transform.position;
            vy = 400 * Time.deltaTime;
            damageDisplay.transform.position = new Vector3(newPos.x, newPos.y + vy + gravity, newPos.z);
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

    private IEnumerator AnimateHealing(string value, Vector3 location)
    {
        float displayDuration = 0.5f;
        damageDisplay.gameObject.SetActive(true);
        damageDisplay.transform.position = location;
        damageDisplay.text = value;

        //each digit is animated individually
        Debug.Log("Length " + damageDisplay.text.Length);
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
        damageDisplay.color = damageColor;        //reset back to defaul
        damageDisplay.gameObject.SetActive(false);
        animateDamageCoroutineOn = false;
    }
}
