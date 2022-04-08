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
    public float res;           //resistance
    protected bool isTheirTurn; //if true, avatar can perform actions.
    protected bool turnTaken;
    public TextMeshProUGUI statsUI;                        //displays HP and MP underneath sprite
   
    protected CombatSystem cs;

    public List<Skill> skills;
    public enum Status
    {
        Normal, Poisoned, Paralyzed, Blind, Charmed, Dead
    }
    public Status status;

    public void RestoreHitPoints(float amount)
    {
        hitPoints += amount;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
    }

    /*protected virtual void Update()
    {
        if (!isTheirTurn)
            return;
    }*/

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
        statsUI.text = "HP " + hitPoints + "/" + maxHitPoints + "\n" + "MP " + manaPoints + "/" + maxManaPoints;
    }
    
    
}
