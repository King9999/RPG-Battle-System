using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script manages the action gauge. They can have different characteristics, and each weapon and skill can have their own.
public class ActionGauge : MonoBehaviour
{
    public Slider actionSlider;
    public ActionGaugeData data;
    public enum ActionValue {Normal, Reduced, Miss, Critical, Special} //Reduced halves damage

    [Header("Meter colours")]
    //public ActionValue actValue;
    public Image normalMeter;
    public Image reducedMeter;
    public Image missMeter;
    public Image critMeter;
    public Image specialMeter;
    public Canvas canvas;
    //public Image[] 
    //public Dictionary<Image, ActionValue> actGauge;
    public ActionValue[] actionValues;
    public Image[] pips;
    public RectTransform[] pipPositions;
    int GaugeSize {get;} = 10;
    // Start is called before the first frame update
    void Start()
    {
        //actGauge.Add()
        actionValues = new ActionValue[GaugeSize];
        pips = new Image[GaugeSize];
        UpdateGauge(data);

        /*for (int i = 0; i < actionValues.Length; i++)
        {
            actionValues[i] = (ActionValue)data.actValues[i];

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

        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //action gauge should be updated as necessary
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
                
            }

        }
    }
}
