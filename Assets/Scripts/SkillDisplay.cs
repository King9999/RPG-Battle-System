using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//displays name of a skill. Includes name and border
public class SkillDisplay : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public Image border;
    //Color borderColor;                  //blue for hero skill, red for enemy skill
    float currentTime;
    float displayDuration;

    // Start is called before the first frame update
    void Start()
    {
        displayDuration = 2;
        gameObject.SetActive(false);        //hidden by default
    }

    public IEnumerator DisplaySkillName(string name, Color borderColor)
    {
        //this.borderColor = borderColor;
        border.color = borderColor;
        skillName.text = name;
        gameObject.SetActive(true);
        currentTime = Time.time;

        while (Time.time < currentTime + displayDuration)
        {
            yield return null;
        }

        //hide display
        gameObject.SetActive(false);
    }
}
