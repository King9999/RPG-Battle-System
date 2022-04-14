using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInputManager : MonoBehaviour
{
    public ActionGauge actGauge;
    [HideInInspector]public bool buttonPressed;

    public static CombatInputManager instance;
    CombatSystem cs;

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
        cs = CombatSystem.instance;
    }

    //Stop the action token and perform an action based on where it lands.
    public void StopActionToken(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //stop the action token and read current index
            actGauge.actionToken.StopToken();
            buttonPressed = true;
        }
    }

    //This should be used to navigate the steps taken to get to the attack state. Use enums
    public void CombatMenuSelected(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
           //check which menu option was selected
           
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
           if (actGauge.gameObject.activeSelf)
           {
               Hero hero = cs.heroesInCombat[cs.currentHero];
               Enemy enemy = cs.enemiesInCombat[cs.currentTarget];
               hero.Attack(enemy);
           }   
        }
    }
}
