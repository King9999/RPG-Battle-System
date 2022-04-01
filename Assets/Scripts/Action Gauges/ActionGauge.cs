using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//This script manages the action gauge. They can have different characteristics, and each weapon and skill can have their own.
public class ActionGauge : MonoBehaviour
{
    //public Slider actionSlider;
    public ActionGaugeData data;

    /*Action Gauge attributes:
    * Normal - equipped weapon deals 100% of its damage. 
    * Reduced - equipped weapon deals 50% damage 
    * Miss - no damage dealt/skill effect fails
    * Critical - equipped weapon deals 200% of its damage and enemy DFP is ignored.
    * Special - activates weapon's special skill if it has one. */
    public enum ActionValue {Normal, Reduced, Miss, Critical, Special}

    [Header("Meter colours")]
    public Image normalMeter;
    public Image reducedMeter;
    public Image missMeter;
    public Image critMeter;
    public Image specialMeter;

    [Header("Tokens")]
    public ActionToken actionToken;         //TODO: change this into a list.
    public ShieldToken shieldToken;         //used by enemies
    public int tokenCount;                  //this value is acquired by weapon data. 

    [Header("Arrays")]
    public Canvas canvas;
    public ActionValue[] actionValues;
    public Image[] pips;
    public RectTransform[] pipPositions;
    int GaugeSize {get;} = 10;

    float pipSize;
    float totalGaugeWidth;
    float currentGaugeValue;
    int currentIndex;
    float currentSize;
    short actionTokenDirection;     //value is either 1 or -1              

    // Start is called before the first frame update
    void Start()
    {
        //actGauge.Add()
        actionValues = new ActionValue[GaugeSize];
        pips = new Image[GaugeSize];
        UpdateGauge(data);
        pipSize = normalMeter.rectTransform.sizeDelta.x * normalMeter.rectTransform.localScale.x;
        totalGaugeWidth = pipSize * GaugeSize;
        Debug.Log("Meter width is " + totalGaugeWidth);

        //add action token. It's placed at the left edge of the first pip.
        Vector3 actionTokenPos = new Vector3(pips[0].transform.position.x - (pipSize / 2), pips[0].transform.position.y + pipSize + 20, transform.position.z);
        actionToken.transform.position = actionTokenPos;
        actionTokenDirection = 1;   //moves from left to right by default

    }

    // Update is called once per frame
    void Update()
    {
        //update the token by moving it along the gauge
        //if it reaches the end of the gauge, the token's direction is reversed   
        currentSize += Time.deltaTime * actionToken.moveSpeed;
       
        Debug.Log("Index: " + currentIndex);

        //update token direction when necessary
        if (actionTokenDirection > 0)
        {
            if (currentSize >= pipSize)
            {
                if (currentIndex + 1 < pips.Length)
                    currentIndex++;
                else
                    actionTokenDirection *= -1;

                currentSize = 0;
            }
        }
        else
        {
            if (currentSize >= pipSize)
            {
                if (currentIndex - 1 >= 0)
                    currentIndex--;
                else
                    actionTokenDirection *= -1;

                currentSize = 0;
            }
        }
        //action token will travel back and forth along the gauge
        Vector3 actionTokenPos = new Vector3(pips[currentIndex].transform.position.x - (pipSize / 2 * actionTokenDirection) 
        + (currentSize * actionTokenDirection), actionToken.transform.position.y, actionToken.transform.position.z);

        actionToken.transform.position = actionTokenPos;
    }

    //updates the values and the pip images
    public void UpdateGauge(ActionGaugeData data)
    {
        actionValues = new ActionValue[GaugeSize];

        for (int i = 0; i < actionValues.Length; i++)
        {
            actionValues[i] = (ActionValue)data.actValues[i];

            //draw a pip on the screen
            if (pips[i] == null)
            {
                //draw a pip on the screen
                Image pip = null;
                switch(actionValues[i])
                {
                    case ActionValue.Normal:
                        pip = normalMeter;
                        break;
                    case ActionValue.Reduced:
                        pip = reducedMeter;
                        break;
                    case ActionValue.Miss:
                        pip = missMeter;
                        break;
                    case ActionValue.Critical:
                        pip = critMeter;
                        break;
                    case ActionValue.Special:
                        pip = specialMeter;
                        break;
                }
                pips[i] = Instantiate(pip, pipPositions[i].position, Quaternion.identity);
                pips[i].transform.SetParent(canvas.transform);
            }
            else
            {
                //replace existing pips with pips from the new gauge
                switch(actionValues[i])
                {
                    case ActionValue.Normal:
                        pips[i] = normalMeter;
                        break;
                    case ActionValue.Reduced:
                        pips[i] = reducedMeter;
                        break;
                    case ActionValue.Miss:
                        pips[i] = missMeter;
                        break;
                    case ActionValue.Critical:
                        pips[i] = critMeter;
                        break;
                    case ActionValue.Special:
                        pips[i] = specialMeter;
                        break;
                }
            }

        }
    }

    //Stop the action token and perform an action based on where it lands.
    public void StopActionToken(InputAction.CallbackContext context)
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
    }
}
