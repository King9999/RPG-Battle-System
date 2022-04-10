using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A hero is a player-controlled entity. A hero has restrictions on which weapons they can use.
public class Hero : Avatar
{
    public HeroData data;
    public TextAsset statFile;
    Stats stats;                        //contains data from stat table
    public int level;
    public Weapon weapon;
    public Armor armor;
    public Trinket trinket;
    public ActionGaugeData actGauge;        //this comes from equipped weapon. If null, hero can't attack
    public int totalAttackTokens;        //attack token mod + weapon tokens
    public int attackTokenMod;
    public bool swordOK, daggerOK, axeOK, bowOK, staffOK;
    public int currentXp;
    public int xpToNextLevel;   //this will be grabbed from a xp table
    [HideInInspector]public int currentLevel;   //points to current level in the stat table.
    protected Color skillNameBorderColor = new Color(0.2f, 0.4f, 0.95f);
    public bool actionCompleted;
    public bool isAttacking;
    public int currentActions;

    //singletons
    protected HeroManager hm;
    CombatInputManager cim;

    // Start is called before the first frame update
    protected void Start()
    {
        /*currentLevel = 3;
        //pull information from a scriptable object
        statFile = data.statFile;
        className = data.className;
        details = data.details;
        
        skills = data.skills;
        swordOK = data.swordOK;
        daggerOK = data.daggerOK;
        axeOK = data.axeOK;
        bowOK = data.bowOK;
        staffOK = data.staffOK;
        //level = data.level;
        currentXp = 0;
        weapon = data.weapon;
        armor = data.armor;
        trinket = data.trinket;
        attackTokenMod = data.attackTokenMod;
    

        //Get stats from JSON
        stats = JsonUtility.FromJson<Stats>(statFile.text);
        level = stats.tableStats[currentLevel].level;
        maxHitPoints = stats.tableStats[currentLevel].hp;
        hitPoints = maxHitPoints;
        maxManaPoints = stats.tableStats[currentLevel].mp;
        manaPoints = maxManaPoints;
        atp = stats.tableStats[currentLevel].atp;           
        dfp = stats.tableStats[currentLevel].dfp;           
        mag = stats.tableStats[currentLevel].mag;          
        res = stats.tableStats[currentLevel].res;
        xpToNextLevel = stats.tableStats[currentLevel].xpToNextLevel;

         if (level < 1)
            level = 1;
        if (level > data.MaxLevel)
            level = data.MaxLevel;

         //equip check
        if (weapon != null)      
            weapon.Equip(hero: this);
        if (armor != null)
            armor.Equip(hero: this);
        if (trinket != null)
            trinket.Equip(hero: this);
        //Debug.Log("Current Level: " + stats.tableStats[stats.tableStats.Length - 1]);*/
        cs = CombatSystem.instance;
        hm = HeroManager.instance;
        cim = CombatInputManager.instance;
        //status = Status.Blind; 
    }

    public void GetData(HeroData data)
    {
        //pull information from a scriptable object
        statFile = data.statFile;
        className = data.className;
        details = data.details;
        skills = data.skills;
        swordOK = data.swordOK;
        daggerOK = data.daggerOK;
        axeOK = data.axeOK;
        bowOK = data.bowOK;
        staffOK = data.staffOK;
        currentXp = 0;
        weapon = data.weapon;
        armor = data.armor;
        trinket = data.trinket;
        attackTokenMod = data.attackTokenMod;
    

        //Get stats from JSON
        stats = JsonUtility.FromJson<Stats>(statFile.text);
        level = stats.tableStats[currentLevel].level;
        maxHitPoints = stats.tableStats[currentLevel].hp;
        hitPoints = maxHitPoints;
        maxManaPoints = stats.tableStats[currentLevel].mp;
        manaPoints = maxManaPoints;
        atp = stats.tableStats[currentLevel].atp;           
        dfp = stats.tableStats[currentLevel].dfp;           
        mag = stats.tableStats[currentLevel].mag;          
        res = stats.tableStats[currentLevel].res;
        spd = stats.tableStats[currentLevel].spd;
        xpToNextLevel = stats.tableStats[currentLevel].xpToNextLevel;

         if (level < 1)
            level = 1;
        if (level > data.MaxLevel)
            level = data.MaxLevel;

         //equip check
        if (weapon != null)      
            weapon.Equip(hero: this);
        if (armor != null)
            armor.Equip(hero: this);
        if (trinket != null)
            trinket.Equip(hero: this);

        actionCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        /*if (isTheirTurn)
        {
            //show menu
            Debug.Log("Hero's turn");
            //show action gauge if attacking or using skill
            PassTurn();
        }

         if (hitPoints > maxHitPoints)
            hitPoints = maxHitPoints;*/

        //if (cs.turnInProgress)
            //TakeAction();

        if (status == Status.Normal || status == Status.Poisoned || status == Status.Blind)
        {
            /*if (hitPoints <= 0)
            {
                status = Status.Dead;
                //TODO: put in a sprite that indicates hero is dead.
                return;
            }*/

            if (isAttacking && cs.enemiesInCombat.Count > 0)
            {
                int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
                Attack(cs.enemiesInCombat[randTarget]);
            }
           
        }

        /*if (isAttacking && cs.enemiesInCombat.Count > 0)
        {
            int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
            Attack(cs.enemiesInCombat[randTarget]);
        }*/
    }

    public override void TakeAction()
    {
        base.TakeAction();

        //do a switch/case here for the different menu options (attack, skill, item, or run)
        switch(status)
        {
            case Status.Normal:
                //open a menu, player chooses next action
                break;
            
            case Status.Paralyzed:
                TryRemoveAilment();
                Invoke("PassTurn", invokeTime);
                break;

            case Status.Blind:
                TryRemoveAilment();
                //player chooses action
                break;

            case Status.Charmed:
                //attack a random ally
                Debug.Log(className + " is charmed!");
                int randTarget = Random.Range(0, cs.heroesInCombat.Count);
                Attack(cs.heroesInCombat[randTarget]);
                Invoke("PassTurn", invokeTime);
                break;
        }

        isAttacking = true;
        
    }

   
    public override void Attack(Avatar target /*ActionGauge.ActionValue actValue*/)
    {
        if (status != Status.Charmed)
        {
            if (!cs.actGauge.gameObject.activeSelf)
            {
                cs.actGauge.gameObject.SetActive(true);
                cs.actGauge.UpdateGaugeData(weapon.actGauge);
                cs.actGauge.ResetActionToken();

                //TODO: add code for when player is blind. all normal and critical pips become miss pips
                if (status == Status.Blind)
                {
                    cs.actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Miss);
                    cs.actGauge.ChangeActionValue(ActionGauge.ActionValue.Critical, ActionGauge.ActionValue.Miss);
                }

                totalAttackTokens = weapon.tokenCount + attackTokenMod;
                currentActions = 0;
            }

            if (currentActions < totalAttackTokens)
            {
                isAttacking = true;
                //wait for button press to attack. Do this until no more tokens available
                if (cim.buttonPressed)
                {
                    float totalDamage = 0;
                    //int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
                    switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
                    {
                        case ActionGauge.ActionValue.Normal:
                            //deal damage to enemy
                            totalDamage = atp + Mathf.Round(Random.Range(0, atp * 0.1f)) - target.dfp;
                            break;

                        case ActionGauge.ActionValue.Reduced:
                            //deal half damage to enemy
                            totalDamage = (atp / 2) + Mathf.Round(Random.Range(0, atp * 0.1f)) - target.dfp;
                            break;

                        case ActionGauge.ActionValue.Miss:
                            //nothing happens
                            break;

                        case ActionGauge.ActionValue.Critical:
                            //deal increased damage to enemy. Enemy DFP is ignored
                            //if landed on a shield, deal shield damage
                            totalDamage = Mathf.Round(atp * 1.5f) + Mathf.Round(Random.Range(0, atp * 1.5f * 0.1f));
                            break;

                        case ActionGauge.ActionValue.Special:
                            //activate weapon skill
                            //weapon.weaponSkill.Activate();
                            break;
                    }

                    if (!animateAttackCoroutineOn)
                        StartCoroutine(AnimateAttack());

                    //deal final damage to enemy
                    Debug.Log(className + " deals " + totalDamage + " damage to " + target.className);
                    ReduceHitPoints(target, totalDamage);

                    //attack token resets and speeds up by 20%
                    cs.actGauge.ResetActionToken();
                    float newSpeed = cs.actGauge.actionToken.TokenSpeed() * 1.2f;
                    cs.actGauge.actionToken.SetTokenSpeed(newSpeed);
                    currentActions++;
                    cim.buttonPressed = false;   
                }
                    
            }
            else
            {
                isAttacking = false;
                cs.actGauge.actionToken.SetSpeedToDefault();
                cs.actGauge.gameObject.SetActive(false);
                Invoke("PassTurn", invokeTime);
            }
        }
        else    //player is charmed, do a regular attack with a chance of crit
        {
            //5% chance to inflict a critical. Criticals ignore defense
            float totalDamage;
            float critChance = 0.05f;
            float roll = Random.Range(0, 1f);

            if (roll <= critChance)
            {
                totalDamage = Mathf.Round(atp * 1.5f) + Mathf.Round(Random.Range(0, atp * 1.5f * 0.1f));
            }
            else
            {
                totalDamage = atp + Mathf.Round(Random.Range(0, atp * 0.1f)) - target.dfp;
            }

            //if player is blind, high chance they do 0 damage
            if (status == Status.Blind)
            {
                float blindHitChance = 0.2f;
                Debug.Log(className + " is blind!");
                roll = Random.Range(0, 1f);
                if (roll > blindHitChance)
                {
                    totalDamage = 0;
                }
            }
            
            if (totalDamage < 0)
                totalDamage = 0;
            
            if (!animateAttackCoroutineOn)
               StartCoroutine(AnimateAttack());

            ReduceHitPoints(target, totalDamage);
            Debug.Log(totalDamage + " damage to " + target.className);
        }

    }

    //static hero sprite dashes forward and back
    protected override IEnumerator AnimateAttack()
    {
        animateAttackCoroutineOn = true;
        Vector3 initPos = transform.position;
        Vector3 destination = new Vector3(initPos.x + 2, initPos.y, initPos.z);

        while(transform.position.x < destination.x)
        {
            float vx = 30 * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + vx, transform.position.y, transform.position.z);
            yield return null;
        }

        //return
        while(transform.position.x > initPos.x)
        {
            float vx = 30 * Time.deltaTime;
            transform.position = new Vector3(transform.position.x - vx, transform.position.y, transform.position.z);
            yield return null;
        }

        //resturn to init position
        transform.position = initPos;
        animateAttackCoroutineOn = false;
    }

}
