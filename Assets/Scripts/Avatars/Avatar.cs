using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/* This is the base class for playable heroes and NPC enemies */
public abstract class Avatar : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    public string className;
    public string details;      //description of the enemy/hero
    public float hitPoints;
    public float maxHitPoints;
    public float manaPoints;
    public float maxManaPoints;
    public float atp;           //attack power
    public float dfp;           //defense power
    public float spd;           //speed
    public float mag;           //magic power
    public float res;           //resistance against magic and ailments
    float ailmentCureChance = 0.05f;        //base value for curing certain ailments naturally
    protected bool isTheirTurn; //if true, avatar can perform actions.
    protected bool turnTaken;
    protected float invokeTime = 1f;          //used to call PassTurn method after elapsed time
    public TextMeshProUGUI statsUI;             //displays HP and MP underneath sprite
    protected GameObject aura;                  //used to highlight sprite
    public GameObject auraPrefab;
    bool mouseOverAvatar;

    //stat modifiers
    public float hpMod = 1;
    public float mpMod = 1;
    public float atpMod = 1;
    public float dfpMod = 1;
    public float spdMod = 1;
    public float magMod = 1;
    public float resMod = 1;

    //coroutine check
    protected bool animateAttackCoroutineOn;
   
    

    public List<Skill> skills;

    [Header("Ailment Status")]
    public bool resistPoison;
    public bool resistParalysis;
    public bool resistBlind;
    public bool resistCharm;
    
    public enum Status
    {
        Normal, Poisoned, Paralyzed, Blind, Charmed, Dead, GuardBroken      //last ailment occurs when shield token is destroyed
    }
    public Status status;

    //singletons
    protected CombatSystem cs;
    protected GameManager gm;

    protected virtual void Start()
    {
        //aura setup
        aura = Instantiate(auraPrefab, transform.position, Quaternion.identity);
        //SpriteRenderer auraSr = aura.GetComponent<SpriteRenderer>();
        //auraSr.enabled = false;
        aura.SetActive(false);
        skills = new List<Skill>();
        gm = GameManager.instance;
        cs = CombatSystem.instance;
    }

    public void RestoreHitPoints(Avatar target, float amount)
    {
        target.hitPoints += amount;
        if (target.hitPoints > target.maxHitPoints)
        {
            target.hitPoints = target.maxHitPoints;
        }

        //show healed amount
        UI ui = UI.instance;
        //Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        ui.DisplayHealing(amount.ToString(), target.transform.position);

        target.UpdateStatsUI();
    }

    /*protected virtual void Update()
    {
        if (hitPoints < 0)
            hitPoints = 0;
    }*/

    //detailed info is displayed if player mouses over an avatar
    public virtual void OnPointerEnter(PointerEventData pointer)
    {
        UI ui = UI.instance;

        string avatarStats = className + "\nATP " + atp + "\nDFP " + dfp + "\nMAG " + mag + "\nRES " + res + "\nSPD "
            + spd + "\n\nSTATUS\n" + status;
        
        string skillSet = "SKILLS\n";

        //display skills
        if (skills.Count <= 0)
            skillSet += "<NONE>";
        else
        { 
            foreach(Skill skill in skills)
            {
                skillSet += skill.skillName + "\n";
            }
        }

        ui.combatDataDisplay.DisplayStats(avatarStats, skillSet);

        mouseOverAvatar = true;
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        UI ui = UI.instance;
        ui.combatDataDisplay.HideStats();

        mouseOverAvatar = false;
    }

    public virtual void OnPointerClick(PointerEventData pointer) { }

    public void ReduceHitPoints(Avatar target, float amount)
    {
        if (amount < 0) amount = 0;

        target.hitPoints -= amount;
        if (target.hitPoints < 0)
        {
            target.hitPoints = 0;
        }

        //show damage
        UI ui = UI.instance;
        //Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        ui.DisplayDamage(amount.ToString(), target.transform.position, ui.damageDisplay.color);

        target.UpdateStatsUI();
    }

    public bool TheirTurn() { return isTheirTurn; }
    public bool TurnTaken() {return turnTaken;}

    public void SetTurnTaken(bool state) { turnTaken = state;}
    
    public void SetTurn(bool turnState) 
    {
         isTheirTurn = turnState;
         //turnTaken = (isTheirTurn == true) ? false : true;
         //cs.turnInProgress = (isTheirTurn == true) ? true : false; 
    }
    public virtual void PassTurn()
    {
        isTheirTurn = false;
        turnTaken = true;
        cs.turnInProgress = false;
    }

    public virtual void Attack(Avatar target) {}

    //All enemy logic must go in here
    public virtual void TakeAction()
    {
        isTheirTurn = true;
        cs.turnInProgress = true;

        //add an outline to show it's the avatar's turn
        aura.SetActive(true);
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer auraSr = aura.GetComponent<SpriteRenderer>();
        auraSr.sprite = sr.sprite;
        auraSr.transform.position = transform.position;
        auraSr.flipX = sr.flipX;
        auraSr.enabled = true;

        //reset damage colour to default
        UI ui = UI.instance;
        ui.damageDisplay.color = ui.damageColor;

        //call coroutine to animate the aura. Avatar takes action after this coroutine finishes.
        
    }

    public void UpdateStatsUI()
    {
        statsUI.text = "<color=#f65974>HP</color> " + hitPoints + "/" + maxHitPoints + "\n" + "<color=#4be4fc>MP</color> " + manaPoints + "/" + maxManaPoints;
    }

    //chance to remove an ailment naturally. Only works against paralysis, blind, and charm
    public void TryRemoveAilment()
    {
        if(status == Status.Poisoned || status == Status.Dead || status == Status.Normal)
            return;
        
        float roll = Random.Range(0, 1f);

        //every 10 points of res adds 1% to cure chance
        float totalCureChance = ailmentCureChance + (res / 1000);
        if (roll <= totalCureChance)
        {
            Debug.Log(status + " ailment removed from " + className);

            //show an effect that ailment is removed
            status = Status.Normal;
        }
    }

    public void RunAway()
    {
        StartCoroutine(AnimateRun());
    }

    public bool SkillActivated(float probability)
    {
        float roll = Random.Range(0, 1f);
        return roll <= probability;
    }

    #region Coroutines
    protected virtual IEnumerator AnimateAttack() { yield return null; }

    protected virtual IEnumerator HighlightAvatar(){ yield return null; }
    protected virtual IEnumerator AnimateRun() { yield return null; } 
     
    #endregion
    
    
}
