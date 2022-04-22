
/* Shield tokens travel along the action gauge, moving in the opposite direction of action tokens. Not all enemies will have shield tokens. 
    The enemy can have more than 1 shield, making the target more difficult to hit.
    
    Shield tokens will nullify any action an action token lands on, but they are damaged in the process. Once a shield is broken, the player 
    gets bonuses, such as all miss panels being removed, all normal panels becoming critical panels, an enemy losing their turn, etc. The bonuses the
    player receives are random.
    
     */
public class ShieldToken : ActionToken
{
    float defaultSpeed {get;} = 300;
    float shieldSize;       //affects the length of the shield. Default is 1.
    public int hitPoints {get; set;}      //3 hit points by default
    int defaultHitPoints {get;} = 3;
    public bool isEnabled {get; set;}
    //public bool shieldBroken {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        SetTokenSpeed(defaultSpeed);
        //isEnabled = true;
        hitPoints = defaultHitPoints;
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
