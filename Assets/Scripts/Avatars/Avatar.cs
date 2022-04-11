using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* This is the base class for playable heroes and NPC enemies */
public abstract class Avatar : MonoBehaviour
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
    protected float invokeTime = 1.5f;          //used to call PassTurn method after elapsed time
    public TextMeshProUGUI statsUI;                        //displays HP and MP underneath sprite

    //coroutine check
    protected bool animateAttackCoroutineOn;
   
    protected CombatSystem cs;

    public List<Skill> skills;
    public enum Status
    {
        Normal, Poisoned, Paralyzed, Blind, Charmed, Dead
    }
    public Status status;

    public void RestoreHitPoints(Avatar target, float amount)
    {
        target.hitPoints += amount;
        if (target.hitPoints > target.maxHitPoints)
        {
            target.hitPoints = target.maxHitPoints;
        }

        //show healed amount
        UI ui = UI.instance;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        ui.DisplayHealing(amount.ToString(), targetPos);
        //ui.damageDisplay.color = ui.damageColor;    //change back in case next attack is damage

        target.UpdateStatsUI();
    }

    /*protected virtual void Update()
    {
        if (hitPoints < 0)
            hitPoints = 0;
    }*/

    public void ReduceHitPoints(Avatar target, float amount)
    {
        target.hitPoints -= amount;
        if (target.hitPoints < 0)
        {
            target.hitPoints = 0;
        }

        //show damage
        UI ui = UI.instance;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        ui.DisplayDamage(amount.ToString(), targetPos, ui.damageDisplay.color);

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
    public void PassTurn()
    {
        isTheirTurn = false;
        turnTaken = true;
        cs.turnInProgress = false;
    }

    public virtual void Attack(Avatar target) {}

    //All enemy logic must go in here
    public virtual void TakeAction()
    {
        //turnTaken = true;
        cs.turnInProgress = true;
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

    #region Coroutines
    protected virtual IEnumerator AnimateAttack() {yield return null;}
    #endregion
    
    
}
