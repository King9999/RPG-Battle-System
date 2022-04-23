using UnityEngine;

/* Shield tokens travel along the action gauge, moving in the opposite direction of action tokens. Not all enemies will have shield tokens. 
    The enemy can have more than 1 shield, making the target more difficult to hit.
    
    Shield tokens will nullify any action an action token lands on, but they are damaged in the process. Once a shield is broken, the player 
    gets bonuses, such as all miss panels being removed, all normal panels becoming critical panels, an enemy losing their turn, etc. The bonuses the
    player receives are random.
    
     */
public class ShieldToken : ActionToken
{
    float defaultSpeed {get;} = 300;
    public int hitPoints {get; set;}      //3 hit points by default
    int defaultHitPoints {get;} = 3;
    public bool isEnabled {get; set;}
    float stunDuration;         //number of seconds a shield does not move when hit
    float currentTime;
    bool isStunned; 

    // Start is called before the first frame update
    void Start()
    {
        SetTokenSpeed(defaultSpeed);
        //isEnabled = true;
        hitPoints = defaultHitPoints;
        stunDuration = 2;
        isStunned = false;
    }

    void Update()
    {
        if (isStunned && Time.time > currentTime + stunDuration)
        {
            isStunned = false;
            StartToken();
        }
    }

    public void StunToken()
    {
        tokenMoving = false;
        Debug.Log(tokenMoving);
        isStunned = true;
        currentTime = Time.time;
    }

    public override void SetTokenSpeed(float speed)
    {
        if (speed <= 0)
            return;

        moveSpeed = speed;
        if (!isStunned) tokenMoving = true;
    }

    public override void SetSpeedToDefault() { moveSpeed = defaultSpeed; }

    public void ShowToken(bool toggle)
    {
        gameObject.SetActive(toggle);
        //isEnabled = toggle == true ? true : false;
    }

    public void GenerateToken(int hitPoints = 3)
    {
        if (hitPoints < 0) return;

        if (hitPoints > defaultHitPoints)
            this.hitPoints = hitPoints;
        else
            this.hitPoints = defaultHitPoints;

        isEnabled = true;
        gameObject.SetActive(true);
    }


}
