using UnityEngine;
using UnityEngine.UI;

/* Action tokens travel along the action gauge, moving right to left. */
public class ActionToken : MonoBehaviour
{
    protected float moveSpeed;         //how fast it travels along the action gauge;
    protected bool tokenMoving;
    float defaultSpeed {get;} = 400;
    public Image head;           //sword for attacks, book for skills.
    public Sprite attackHead;
    public Sprite skillHead;

    // Start is called before the first frame update
    void Start()
    {
        SetTokenSpeed(defaultSpeed);
    }

    public void StopToken()
    {
        tokenMoving = false;
    }

    public void StartToken()
    {
        tokenMoving = true;
    }

    public virtual void SetTokenSpeed(float speed)
    {
        if (speed <= 0)
            return;

        moveSpeed = speed;
        tokenMoving = true;
    }

    public virtual void SetSpeedToDefault() { moveSpeed = defaultSpeed; }

    public bool TokenIsMoving() { return tokenMoving; }
    public float TokenSpeed() { return moveSpeed; }
}
