using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Action tokens travel along the action gauge, moving right to left. */
public class ActionToken : MonoBehaviour
{
    float moveSpeed;         //how fast it travels along the action gauge;
    bool tokenMoving;
    float defaultSpeed {get;} = 400;

    //public Image token;
    // Start is called before the first frame update
    void Start()
    {
        //moveSpeed = 400;
        SetTokenSpeed(defaultSpeed);
    }

    public void StopToken()
    {
        //moveSpeed = 0;
        tokenMoving = false;
    }

    public void StartToken()
    {
        tokenMoving = true;
    }

    public void SetTokenSpeed(float speed)
    {
        if (speed <= 0)
            return;

        moveSpeed = speed;
        tokenMoving = true;
    }

    public void SetSpeedToDefault() { moveSpeed = defaultSpeed; }

    public bool TokenIsMoving() { return tokenMoving; }
    public float TokenSpeed() { return moveSpeed; }
}
