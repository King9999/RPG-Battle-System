using UnityEngine;
using UnityEngine.UI;

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
    public ActionToken actionToken;         //TODO: change this into a list.
    public ShieldToken shieldToken;         //used by enemies
    public int tokenCount;                  //this value is acquired by weapon data. 

    [Header("Arrays")]
    public Canvas canvas;
    public ActionValue[] actionValues;
    public Image[] panels;
    public RectTransform[] panelPositions;
    int GaugeSize {get;} = 10;

    float panelSize;
    float totalGaugeWidth;
    float currentGaugeValue;
    [HideInInspector]public int currentIndex;
    float currentSize;
    short actionTokenDirection;     //value is either 1 or -1
    short shieldTokenDirection;
    public bool buttonPressed;              

    // Start is called before the first frame update
    void Start()
    {
        //actGauge.Add()
        actionValues = new ActionValue[GaugeSize];
        panels = new Image[GaugeSize];
        panelSize = normalPanel.rectTransform.sizeDelta.x * normalPanel.rectTransform.localScale.x;
        totalGaugeWidth = panelSize * GaugeSize;
        UpdateGaugeData(data);
        //Debug.Log("Panel width is " + totalGaugeWidth);

        //add action token. It's placed at the left edge of the first panel.
        //Vector3 actionTokenPos = new Vector3(panels[0].transform.position.x - (panelSize / 2), panels[0].transform.position.y + panelSize + 20, transform.position.z);
        //actionToken.transform.position = actionTokenPos;
        //actionTokenDirection = 1;   //moves from left to right by default
        ResetActionToken();
    }

    // Update is called once per frame
    void Update()
    {
        //update the token by moving it along the gauge
        //if it reaches the end of the gauge, the token's direction is reversed
        if (actionToken.TokenIsMoving())   
            currentSize += Time.deltaTime * actionToken.TokenSpeed();
       
        //Debug.Log("Index: " + currentIndex);

        //update token direction when necessary
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
        //action token will travel back and forth along the gauge
        Vector3 actionTokenPos = new Vector3(panels[currentIndex].transform.position.x - (panelSize / 2 * actionTokenDirection) 
        + (currentSize * actionTokenDirection), actionToken.transform.position.y, actionToken.transform.position.z);

        actionToken.transform.position = actionTokenPos;
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

        //add action token. It's placed at the left edge of the first panel.
        //Vector3 actionTokenPos = new Vector3(panels[0].transform.position.x - (panelSize / 2), panels[0].transform.position.y + panelSize + 20, transform.position.z);
        //actionToken.transform.position = actionTokenPos;
        //actionTokenDirection = 1;   //moves from left to right by default
        ResetActionToken();

    }

    public void ResetActionToken()
    {
        Vector3 actionTokenPos = new Vector3(panels[0].transform.position.x - (panelSize / 2), panels[0].transform.position.y + panelSize + 20, transform.position.z);
        actionToken.transform.position = actionTokenPos;
        currentSize = 0;
        currentIndex = 0;
        actionTokenDirection = 1;   //moves from left to right by default
    }

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

    //Stop the action token and perform an action based on where it lands.
    /*public void StopActionToken(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //stop the action token and read current index
            actionToken.StopToken();

            switch(actionValues[currentIndex])
            {
                case ActionValue.Normal:
                    //deal damage to enemy
                    break;

                case ActionValue.Reduced:
                    //deal half damage to enemy
                    break;

                case ActionValue.Miss:
                    //nothing happens
                    break;

                case ActionValue.Critical:
                    //deal increased damage to enemy
                    //if landed on a shield, deal shield damage
                    break;

                case ActionValue.Special:
                    //activate weapon skill
                    break;
            }
        }
    }*/
}
