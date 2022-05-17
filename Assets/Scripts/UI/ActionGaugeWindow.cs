using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* This is used to display action gauge data when player view weapons in inventory. */
public class ActionGaugeWindow : MonoBehaviour
{
    public Image normalPanel;
    public Image reducedPanel;
    public Image missPanel;
    public Image critPanel;
    public Image specialPanel;
    public Image[] panels;
    public ActionGaugeData data;

    //public enum ActionValue {Normal, Reduced, Miss, Critical, Special}
    //public ActionValue[] actionValues;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWindow(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void UpdateGaugeData(ActionGaugeData data)
    {
        this.data = data;
        //actionValues = new ActionValue[GaugeSize];

        for (int i = 0; i < data.actValues.Length; i++)
        {
            //actValues[i] = (ActionValue)data.actValues[i];

            //draw a panel on the screen
            /*if (panels[i] == null)
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
            {*/
                //replace existing panels with panels from the new gauge
                switch(data.actValues[i])
                {
                    case ActionGaugeData.ActionValue.Normal:
                        panels[i].sprite = normalPanel.sprite;
                        break;
                    case ActionGaugeData.ActionValue.Reduced:
                        panels[i].sprite = reducedPanel.sprite;
                        break;
                    case ActionGaugeData.ActionValue.Miss:
                        panels[i].sprite = missPanel.sprite;
                        break;
                    case ActionGaugeData.ActionValue.Critical:
                        panels[i].sprite = critPanel.sprite;
                        break;
                    case ActionGaugeData.ActionValue.Special:
                        panels[i].sprite = specialPanel.sprite;
                        break;
                }
            //}

        }
    }
}
