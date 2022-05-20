using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this must be attached to "dungeon HUD" object
public class DungeonUI : MonoBehaviour
{
    public Inventory inv;
    public DungeonMenu menu;
    public PartyStats[] partyDisplay;             //handles party UI in dungeon screen.
    public Notification notification;
    public TextMeshProUGUI dungeonLevelUI;
    public TextMeshProUGUI selectTargetUI;      //appears above the party UI
    public TextMeshProUGUI statusUI;         //for displaying values/messages over party UI when using items/skills
    [HideInInspector]public Color healColor, normalColor;
    bool animateStatusCoroutineOn;
    public int currentHero {get; set;}      //used to track which hero is selected in the party UI


    public static DungeonUI instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        selectTargetUI.gameObject.SetActive(false);
        statusUI.gameObject.SetActive(false);
        healColor = new Color(0, 0.9f, 0.3f);
        normalColor = Color.white;
    }

    public void DisplayStatus(string value, Vector3 location, Color textColor)
    {
        if (!animateStatusCoroutineOn)
            StartCoroutine(AnimateStatus(value, location, textColor));
    }

    private IEnumerator AnimateStatus(string value, Vector3 location, Color textColor)
    {
        animateStatusCoroutineOn = true;
        float displayDuration = 0.5f;
        //Vector3 avatarPos = Camera.main.WorldToScreenPoint(location);
        statusUI.gameObject.SetActive(true);
        //statusUI.transform.position = avatarPos;
        statusUI.transform.position = location;
        statusUI.text = value;
        statusUI.color = textColor;

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
        statusUI.color = normalColor;        //reset back to default
        statusUI.gameObject.SetActive(false);
        animateStatusCoroutineOn = false;
    }

}
