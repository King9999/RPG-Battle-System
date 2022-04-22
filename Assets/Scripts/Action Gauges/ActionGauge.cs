using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//This script manages the action gauge. They can have different characteristics, and each weapon and skill can have their own.
public class ActionGauge : MonoBehaviour
{
    //public Slider actionSlider;
    public ActionGaugeData data;

    /*Action Gauge attributes:
    * Normal - equipped weapon deals 100% of its damage. 
    * Reduced - equipped weapon deals 50% damage 
    * Miss - no damage dealt/skill effect fails
    * Critical - equipped weapon deals 50% increased damage and enemy DFP is ignored.
    * Special - activates weapon's special skill if it has one. 
    
    Breaking shield tokens can alter the action gauge by transforming panels.
    
    */
    public enum ActionValue {Normal, Reduced, Miss, Critical, Special}

    [Header("Panel colours")]
    public Image normalPanel;
    public Image reducedPanel;
    public Image missPanel;
    public Image critPanel;
    public Image specialPanel;

    [Header("Tokens")]
    public ActionToken actionToken;         
    public ShieldToken shieldToken;         //used by enemies
    public int tokenCount;                  //this value is acquired by weapon data. 
    public int bonusTurns;                  //how many turns bonuses are active. When this is 0 and enemy had a shield, the shield is restored.

    [Header("Arrays")]
    public Canvas canvas;
    public ActionValue[] actionValues;
    public Image[] panels;
    public RectTransform[] panelPositions;
    int GaugeSize {get;} = 10;

    [HideInInspector]public float panelSize;
    float totalGaugeWidth;
    float currentGaugeValue;
    [HideInInspector]public int currentIndex;               //action gauge index
    [HideInInspector]public List<int> currentShieldTokenIndex;
    float currentSize;
    [HideInInspector]public List<float> currentShieldTokenSize;
    short actionTokenDirection;     //value is either 1 or -1
    [HideInInspector]public List<short> shieldTokenDirection;
    //public bool buttonPressed;
              
    public static ActionGauge instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        actionValues = new ActionValue[GaugeSize];
        panels = new Image[GaugeSize];
        panelSize = normalPanel.rectTransform.sizeDelta.x * normalPanel.rectTransform.localScale.x;
        totalGaugeWidth = panelSize * GaugeSize;
        UpdateGaugeData(data);
        //Debug.Log("Panel width is " + totalGaugeWidth);

        //add tokens, list setup
        ResetActionToken();
        //ResetShieldToken();
        //currentShieldTokenSize = new List<float>();
        //currentShieldTokenIndex = new List<int>();
        //shieldTokenDirection = new List<short>();
    }

    // Update is called once per frame
    void Update()
    {
        //update the token by moving it along the gauge
        //if it reaches the end of the gauge, the token's direction is reversed
        if (actionToken.TokenIsMoving())   
            currentSize += Time.deltaTime * actionToken.TokenSpeed();

        /*CombatSystem cs = CombatSystem.instance;
        List<ShieldToken> shields = cs.enemiesInCombat[cs.currentTarget].shields;
        for (int i = 0; i < shields.Count; i++)
        {
            if (shields[i].isEnabled && shields[i].TokenIsMoving())
            {
                currentShieldTokenSize[i] += Time.deltaTime * shields[i].TokenSpeed();

                //check token direction and index
                if (shieldTokenDirection[i] > 0)
                {
                    if (currentShieldTokenSize[i] >= panelSize)
                    {
                        if (currentShieldTokenIndex[i] + 1 < panels.Length)
                            currentShieldTokenIndex[i]++;
                        else
                            shieldTokenDirection[i] *= -1;

                        currentShieldTokenSize[i] = 0;
                    }
                }
                else
                {
                    if (currentShieldTokenSize[i] >= panelSize)
                    {
                        if (currentShieldTokenIndex[i] - 1 >= 0)
                            currentShieldTokenIndex[i]--;
                        else
                            shieldTokenDirection[i] *= -1;

                        currentShieldTokenSize[i] = 0;
                    }
                }

                //update shield token position
                Vector3 shieldTokenPos = new Vector3(panels[currentShieldTokenIndex[i]].transform.position.x - (panelSize / 2 * shieldTokenDirection[i])
                    + (currentShieldTokenSize[i] * shieldTokenDirection[i]), shields[i].transform.position.y, shields[i].transform.position.z);

                shields[i].transform.position = shieldTokenPos;
            }
        }*/
       
        //Debug.Log("Index: " + currentIndex);

        //update action token index & direction when necessary
        if (actionTokenDirection > 0)
        {
            if (currentSize >= panelSize)
            {
                if (currentIndex + 1 < panels.Length)
                    currentIndex++;
                else
                    actionTokenDirection *= -1;

                currentSize = 0;
            }
        }
        else
        {
            if (currentSize >= panelSize)
            {
                if (currentIndex - 1 >= 0)
                    currentIndex--;
                else
                    actionTokenDirection *= -1;

                currentSize = 0;
            }
        }

        //shield token check
        /*if (shieldTokenDirection > 0)
        {
            if (currentShieldTokenSize >= panelSize)
            {
                if (currentShieldTokenIndex + 1 < panels.Length)
                    currentShieldTokenIndex++;
                else
                    shieldTokenDirection *= -1;

                currentShieldTokenSize = 0;
            }
        }
        else
        {
            if (currentShieldTokenSize >= panelSize)
            {
                if (currentShieldTokenIndex - 1 >= 0)
                    currentShieldTokenIndex--;
                else
                    shieldTokenDirection *= -1;

                currentShieldTokenSize = 0;
            }
        }*/
        //action & shield token will travel back and forth along the gauge
        Vector3 actionTokenPos = new Vector3(panels[currentIndex].transform.position.x - (panelSize / 2 * actionTokenDirection) 
        + (currentSize * actionTokenDirection), actionToken.transform.position.y, actionToken.transform.position.z);

        actionToken.transform.position = actionTokenPos;

        //Vector3 shieldTokenPos = new Vector3(panels[currentShieldTokenIndex].transform.position.x - (panelSize / 2 * shieldTokenDirection)
        //+ (currentShieldTokenSize * shieldTokenDirection), shieldToken.transform.position.y, shieldToken.transform.position.z);

        //shieldToken.transform.position = shieldTokenPos;
    }

    //updates the values and the panel images
    public void UpdateGaugeData(ActionGaugeData data)
    {
        this.data = data;
        actionValues = new ActionValue[GaugeSize];

        for (int i = 0; i < actionValues.Length; i++)
        {
            actionValues[i] = (ActionValue)data.actValues[i];

            //draw a panel on the screen
            if (panels[i] == null)
            {
                //draw a panel on the screen
                Image panel = null;
                switch(actionValues[i])
                {
                    case ActionValue.Normal:
                        panel = normalPanel;
                        break;
                    case ActionValue.Reduced:
                        panel = reducedPanel;
                        break;
                    case ActionValue.Miss:
                        panel = missPanel;
                        break;
                    case ActionValue.Critical:
                        panel = critPanel;
                        break;
                    case ActionValue.Special:
                        panel = specialPanel;
                        break;
                }
                panels[i] = Instantiate(panel, panelPositions[i].position, Quaternion.identity);
                panels[i].transform.SetParent(canvas.transform);
            }
            else
            {
                //replace existing panels with panels from the new gauge
                switch(actionValues[i])
                {
                    case ActionValue.Normal:
                        panels[i].sprite = normalPanel.sprite;
                        break;
                    case ActionValue.Reduced:
                        panels[i].sprite = reducedPanel.sprite;
                        break;
                    case ActionValue.Miss:
                        panels[i].sprite = missPanel.sprite;
                        break;
                    case ActionValue.Critical:
                        panels[i].sprite = critPanel.sprite;
                        break;
                    case ActionValue.Special:
                        panels[i].sprite = specialPanel.sprite;
                        break;
                }
            }

        }

        ResetActionToken();
        //ResetShieldToken();
    }

    public void ResetActionToken()
    {
        Vector3 actionTokenPos = new Vector3(panels[0].transform.position.x - (panelSize / 2), 
            panels[0].transform.position.y + panelSize + 20, transform.position.z);
        actionToken.transform.position = actionTokenPos;
        currentSize = 0;
        currentIndex = 0;
        actionTokenDirection = 1;   //moves from left to right by default
    }

    /*public void ResetShieldToken()
    {
        //shield tokens start at a random position to make it difficult for player to predict. The token does not start
        //at the 0 index.
        int randPanel = Random.Range(1, panels.Length);
        Vector3 shieldTokenPos = new Vector3(panels[randPanel].transform.position.x /*+ (panelSize / 2), 
            panels[randPanel].transform.position.y, transform.position.z);
        shieldToken.transform.position = shieldTokenPos;
        //token.transform.position = shieldTokenPos;
        currentShieldTokenSize = 0;
        currentShieldTokenIndex = randPanel;
        shieldTokenDirection = -1;   //moves from right to left by default
    }*/

    //change all values in a gauge to another value
    public void ChangeActionValue(ActionValue valueToChange, ActionValue newValue)
    {
        for (int i = 0; i < actionValues.Length; i++)
        {
            if (actionValues[i] == valueToChange)
            {
                actionValues[i] = newValue;

                //update the gauge
                switch(actionValues[i])
                {
                    case ActionValue.Normal:
                        panels[i].sprite = normalPanel.sprite;
                        break;
                    case ActionValue.Reduced:
                        panels[i].sprite = reducedPanel.sprite;
                        break;
                    case ActionValue.Miss:
                        panels[i].sprite = missPanel.sprite;
                        break;
                    case ActionValue.Critical:
                        panels[i].sprite = critPanel.sprite;
                        break;
                    case ActionValue.Special:
                        panels[i].sprite = specialPanel.sprite;
                        break;
                }
            }      
        }
    }

    //change a single value in the gauge
    public void ChangeActionValue(ActionValue newValue, int index)
    {
        if (index < 0 || index >= actionValues.Length)
            return;
        
        actionValues[index] = newValue;
        switch(actionValues[index])
        {
            case ActionValue.Normal:
                panels[index].sprite = normalPanel.sprite;
                break;
            case ActionValue.Reduced:
                panels[index].sprite = reducedPanel.sprite;
                break;
            case ActionValue.Miss:
                panels[index].sprite = missPanel.sprite;
                break;
            case ActionValue.Critical:
                panels[index].sprite = critPanel.sprite;
                break;
            case ActionValue.Special:
                panels[index].sprite = specialPanel.sprite;
                break;
        }
    }

    public void ShowGauge(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

}
