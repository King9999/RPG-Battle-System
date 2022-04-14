using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//displays name of a skill. Includes name and border
public class SkillDisplay : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public Image border;
    float currentTime;
    float displayDuration;
    [HideInInspector]public bool displaySkillCoroutineOn;

    // Start is called before the first frame update
    void Start()
    {
        displayDuration = 1f;
        gameObject.SetActive(false);        //hidden by default
    }

    private IEnumerator DisplaySkillName(string name, Color borderColor)
    {
        displaySkillCoroutineOn = true;
        //gameObject.SetActive(true);
        //Debug.Log("Coroutine started");
        border.color = borderColor;
        skillName.text = name;
        
        yield return new WaitForSeconds(displayDuration);

        //hide display
        gameObject.SetActive(false);
        displaySkillCoroutineOn = false;
        //Debug.Log("Coroutine ended");
    }

    public void ExecuteSkillDisplay(string name, Color borderColor)
    {
        //if (!displaySkillCoroutineOn)
        //{
            gameObject.SetActive(true); //must enable the game object before coroutine can run
            StartCoroutine(DisplaySkillName(name, borderColor));
        //}
    }

}
