using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInputManager : MonoBehaviour
{
    public ActionGauge actGauge;
    public bool buttonPressed;

    public static CombatInputManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    //Stop the action token and perform an action based on where it lands.
    public void StopActionToken(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //stop the action token and read current index
            actGauge.actionToken.StopToken();
            buttonPressed = true;

            /*switch(actGauge.actionValues[actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    //deal damage to enemy
                    break;

                case ActionGauge.ActionValue.Reduced:
                    //deal half damage to enemy
                    break;

                case ActionGauge.ActionValue.Miss:
                    //nothing happens
                    break;

                case ActionGauge.ActionValue.Critical:
                    //deal increased damage to enemy
                    //if landed on a shield, deal shield damage
                    break;

                case ActionGauge.ActionValue.Special:
                    //activate weapon skill
                    break;
            }*/
        }
    }
}
