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
    GameManager gm;
    UI ui;

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
        gm = GameManager.instance;
        ui = UI.instance;
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
               //Hero hero = cs.heroesInCombat[cs.currentHero];
               //Enemy enemy = cs.enemiesInCombat[cs.currentEnemyTarget];

               if (!cs.heroUsingSkill)
               {
                    Hero hero = cs.heroesInCombat[cs.currentHero];
                    Enemy enemy = cs.enemiesInCombat[cs.currentEnemyTarget];
                    hero.Attack(enemy);
               }
                else    //activate skill
                {
                    Hero hero = cs.heroesInCombat[cs.currentHero];
                    Inventory inv = Inventory.instance;

                    switch(inv.copiedSkillSlot.SkillInSlot().targetType)
                    {
                        case Skill.Target.OneEnemy:
                            Enemy enemy = cs.enemiesInCombat[cs.currentEnemyTarget];
                            inv.copiedSkillSlot.SkillInSlot().Activate(hero, enemy, hero.SkillBorderColor());
                            break;
                        
                        case Skill.Target.AllEnemies:
                            inv.copiedSkillSlot.SkillInSlot().Activate(hero, cs.enemiesInCombat, hero.SkillBorderColor());
                            break;

                        case Skill.Target.Self:
                            inv.copiedSkillSlot.SkillInSlot().Activate(hero, hero.SkillBorderColor());
                            break;
                        
                        case Skill.Target.OneHero:
                            Hero ally = cs.heroesInCombat[cs.currentHeroTarget];
                            inv.copiedSkillSlot.SkillInSlot().Activate(hero, ally, hero.SkillBorderColor());
                            break;

                        case Skill.Target.AllHeroes:
                            inv.copiedSkillSlot.SkillInSlot().Activate(hero, cs.heroesInCombat, hero.SkillBorderColor());
                            break;
                    }
                    
                    cs.heroUsingSkill = false;
                }
           }   
        }
    }

    public void CloseRewards(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
           if (gm.gameState == GameManager.GameState.ShowCombatRewards)
           {
               //close rewards screen and end combat
               Debug.Log("Closing rewards");
               ui.rewardsDisplay.HideDisplay();
               cs.CloseCombatSystem();
           }   
        }
    }
}
