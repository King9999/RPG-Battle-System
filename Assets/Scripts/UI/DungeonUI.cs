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
    public TextMeshProUGUI equipConfirmUI;      //displayed after player equips a new item or can't
    public TextMeshProUGUI statusUI;            //for displaying values/messages over party UI when using items/skills
    public TextMeshProUGUI[] allStatusUI;         //used for when multiple values need to be displayed
    [HideInInspector]public Color healColor, healManaColor, normalColor;
    bool animateStatusCoroutineOn;
    bool equipCoroutineOn;
    public int currentHero {get; set;}      //used to track which hero is selected in the party UI
    public Button resetButton;              //used for when a dungeon can't be completed. Player will remain on the current floor.


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
        foreach(TextMeshProUGUI status in allStatusUI)
            status.gameObject.SetActive(false);

        equipConfirmUI.gameObject.SetActive(false);
        healColor = new Color(0, 0.9f, 0.3f);
        healManaColor = new Color(0, 0.8f, 1);
        normalColor = Color.white;
    }

    public void DisplayStatus(string value, Vector3 location, Color textColor, float displayDuration = 0.5f, float delayDuration = 0)
    {
        if (!animateStatusCoroutineOn)
            StartCoroutine(AnimateStatus(value, location, textColor, displayDuration, delayDuration));
    }

    public void DisplayStatus(int allStatusUiIndex, string value, Vector3 location, Color textColor, float displayDuration = 0.5f)
    {
        StartCoroutine(AnimateStatus(allStatusUiIndex, value, location, textColor, displayDuration));
    }

    public void DisplayEquipStatus(bool toggle, string message = "")
    {
        equipConfirmUI.gameObject.SetActive(toggle);
        equipConfirmUI.text = message;
    }

    /// <summary>
    /// Executes DisplayEquip coroutine
    /// </summary>
    ///<param name="message">The message to display to indicate an item is equipped.</param>
    ///<param name="returnState">the menu state to return to after coroutine is finished.</param>
    public void ConfirmEquip(string message, DungeonMenu.MenuState returnState)
    {
        if (!equipCoroutineOn)
            StartCoroutine(DisplayEquip(message, 1, returnState));
    }

    public void OnResetButtonClicked()
    {
        Dungeon dungeon = Dungeon.instance;
        GameManager gm = GameManager.instance;

        //put remaining enemies in graveyard
        for(int i = 0; i < dungeon.enemies.Count; i++)
        {
            dungeon.enemies[i].SendToGraveyard();
            i--;
        }

        //deactiavte any unrescued heroes
        foreach(Captive captiveHero in dungeon.captiveHeroes)
        {
            captiveHero.ShowObject(false);
            captiveHero.nodeID = -1;
        }

        dungeon.GenerateDungeon(dungeon.nodeCount, updateDungeonLevel: false); //nodeCount in game manager is not used because we're not updating the dungeon level.
    }

    private IEnumerator AnimateStatus(string value, Vector3 location, Color textColor, float displayDuration = 0.5f, float delayDuration = 0)
    {
        yield return new WaitForSeconds(delayDuration);
        animateStatusCoroutineOn = true;
        //float displayDuration = 0.5f;
        statusUI.gameObject.SetActive(true);
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

    private IEnumerator AnimateStatus(int index, string value, Vector3 location, Color textColor, float displayDuration = 0.5f)
    {
        //animateStatusCoroutineOn = true;
        //float displayDuration = 0.5f;
        allStatusUI[index].gameObject.SetActive(true);
        allStatusUI[index].transform.position = location;
        allStatusUI[index].text = value;
        allStatusUI[index].color = textColor;

        //each digit is animated individually
        Vector3 initPos = allStatusUI[index].transform.position;
        Vector3 destination = new Vector3(initPos.x, initPos.y + 20, initPos.z);
        float vy;
        while(allStatusUI[index].transform.position.y < destination.y)
        {
            Vector3 newPos = allStatusUI[index].transform.position;
            vy = 50 * Time.deltaTime;
            allStatusUI[index].transform.position = new Vector3(newPos.x, newPos.y + vy, newPos.z);
            yield return null;
        }

        allStatusUI[index].transform.position = destination;
        
        yield return new WaitForSeconds(displayDuration);
        allStatusUI[index].color = normalColor;        //reset back to default
        allStatusUI[index].gameObject.SetActive(false);
        //animateStatusCoroutineOn = false;
    }

    IEnumerator DisplayEquip(string message, float duration, DungeonMenu.MenuState returnState)
    {
        equipCoroutineOn = true;
        equipConfirmUI.gameObject.SetActive(true);
        equipConfirmUI.text = message;

        yield return new WaitForSeconds(duration);

        equipCoroutineOn = false;
        equipConfirmUI.gameObject.SetActive(false);
        inv.statsDisplay.ClearDisplay();
        DungeonMenu menu = DungeonMenu.instance;
        menu.SetState(returnState);
    }

}
