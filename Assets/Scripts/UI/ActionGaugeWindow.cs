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
   
    public void ShowWindow(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void UpdateGaugeData(ActionGaugeData data)
    {

        for (int i = 0; i < data.actValues.Length; i++)
        {
            
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

        }
    }
}
