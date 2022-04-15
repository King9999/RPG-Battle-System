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

    //coroutines
    bool highlightAvatarCoroutineOn;

    //singletons
    protected HeroManager hm;
    CombatInputManager cim;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
        //cs = CombatSystem.instance;
        hm = HeroManager.instance;
        cim = CombatInputManager.instance;
        gm = GameManager.instance;
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

        actionCompleted = true;     //not sure what this is for

        //get sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTheirTurn && !isAttacking)
        {
            if (!highlightAvatarCoroutineOn)
                StartCoroutine(HighlightAvatar());
        }

        if (status == Status.Normal || status == Status.Poisoned || status == Status.Blind)
        {
            /*if (hitPoints <= 0)
            {
                status = Status.Dead;
                //remove hero from turn order
                //TODO: put in a sprite that indicates hero is dead.
                return;
            }*/

            if ((isAttacking && currentActions >= totalAttackTokens) || gm.gameState == GameManager.GameState.ShowCombatRewards)
            {
                isAttacking = false;
                cs.actGauge.actionToken.SetSpeedToDefault();
                cs.actGauge.ShowGauge(false);
                isTheirTurn = false;
                Invoke("PassTurn", invokeTime);
            }         
        }

    }

    public override void TakeAction()
    {
        base.TakeAction();

        //which hero is acting?
        cs.currentHero = cs.heroesInCombat.IndexOf(this);

        //do a switch/case here for the different menu options (attack, skill, item, or run)
        UI ui = UI.instance;
        switch(status)
        {
            
            case Status.Normal:
                //open a menu, player chooses next action       
                ui.combatMenu.ShowCombatMenu(true);
                break;
            
            case Status.Paralyzed:
                TryRemoveAilment();
                Invoke("PassTurn", invokeTime);
                break;

            case Status.Blind:
                TryRemoveAilment();
                //player chooses action
                ui.combatMenu.ShowCombatMenu(true);
                break;

            case Status.Charmed:
                //attack a random ally
                Debug.Log(className + " is charmed!");
                int randTarget = Random.Range(0, cs.heroesInCombat.Count);
                Attack(cs.heroesInCombat[randTarget]);
                Invoke("PassTurn", invokeTime);
                break;
        }

        //isAttacking = true;
        
    }

   
    public override void Attack(Avatar target)
    {
        UI ui = UI.instance;
        ui.damageDisplay.color = ui.damageColor;    //reset colour to default
        if (status != Status.Charmed)
        {
           
            //wait for button press to attack. Do this until no more tokens available
            if (cim.buttonPressed)
            {
                float totalDamage = 0;
                //int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
                switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
                {
                    case ActionGauge.ActionValue.Normal:
                        //deal damage to enemy
                        totalDamage = Mathf.Round(atp * atpMod + Random.Range(0, atp * 0.1f) - (target.dfp * target.dfpMod));
                        //ui.damageDisplay.color = ui.damageColor;
                        if (!animateAttackCoroutineOn)
                            StartCoroutine(AnimateAttack());
                        ReduceHitPoints(target, totalDamage);
                        break;

                    case ActionGauge.ActionValue.Reduced:
                        //deal half damage to enemy
                        totalDamage = Mathf.Round((atp * atpMod) / 2 + Random.Range(0, atp * 0.1f) - (target.dfp * target.dfpMod));
                        ui.damageDisplay.color = ui.reducedDamageColor;
                        if (!animateAttackCoroutineOn)
                            StartCoroutine(AnimateAttack());
                        ReduceHitPoints(target, totalDamage);
                        break;

                    case ActionGauge.ActionValue.Miss:
                        //nothing happens
                        //ui.damageDisplay.color = ui.damageColor;
                        if (!animateAttackCoroutineOn)
                            StartCoroutine(AnimateAttack());
                        ReduceHitPoints(target, totalDamage);
                        break;

                    case ActionGauge.ActionValue.Critical:
                        //deal increased damage to enemy. Enemy DFP is ignored
                        //if landed on a shield, deal shield damage
                        totalDamage = Mathf.Round(atp * atpMod * 1.5f + Random.Range(0, atp * 1.5f * 0.1f));
                        ui.damageDisplay.color = ui.criticalDamageColor;
                        if (!animateAttackCoroutineOn)
                            StartCoroutine(AnimateAttack());
                        ReduceHitPoints(target, totalDamage);
                        break;

                    case ActionGauge.ActionValue.Special:
                        //activate weapon skill
                        switch(weapon.weaponSkill.targetType)
                        {
                            case Skill.Target.None:
                                weapon.weaponSkill.Activate(skillNameBorderColor);
                                break;

                            case Skill.Target.Self:
                                weapon.weaponSkill.Activate(this, skillNameBorderColor);
                                break;
                            
                            case Skill.Target.One:
                                weapon.weaponSkill.Activate(this, target, skillNameBorderColor);
                                break;

                            case Skill.Target.All:
                                //weapon.weaponSkill.Activate(this, skillNameBorderColor);
                                break;
                        }
                        
                        break;
                }

                //attack token resets and speeds up by 20%
                cs.actGauge.ResetActionToken();
                float newSpeed = cs.actGauge.actionToken.TokenSpeed() * 1.2f;
                cs.actGauge.actionToken.SetTokenSpeed(newSpeed);
                currentActions++;
                cim.buttonPressed = false;   
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
                totalDamage = Mathf.Round(atp * atpMod * 1.5f + Random.Range(0, atp * 1.5f * 0.1f));
                ui.damageDisplay.color = ui.criticalDamageColor;
            }
            else
            {
                totalDamage = Mathf.Round(atp * atpMod + Random.Range(0, atp * 0.1f) - (target.dfp * target.dfpMod));
                //ui.damageDisplay.color = ui.damageColor;
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

    public void SetupActionGauge(ActionGauge actGauge, ActionGaugeData weaponData)
    {
        if (!actGauge.gameObject.activeSelf)
        {
            actGauge.ShowGauge(true);
            actGauge.UpdateGaugeData(weaponData);
            actGauge.ResetActionToken();

            //certain pips change if player is blind
            if (status == Status.Blind)
            {
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Miss);
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Critical, ActionGauge.ActionValue.Miss);
            }

            totalAttackTokens = (weapon.tokenCount + attackTokenMod < 1) ? 1 : weapon.tokenCount + attackTokenMod;
            currentActions = 0;
            actGauge.actionToken.StartToken();
        }
    }

    public void LevelUp()
    {
        if (currentLevel >= data.MaxLevel - 1)
            return;

        currentLevel++;   
        level = stats.tableStats[currentLevel].level;
        maxHitPoints = stats.tableStats[currentLevel].hp;
        maxManaPoints = stats.tableStats[currentLevel].mp;
        atp = stats.tableStats[currentLevel].atp + weapon.atp;           
        dfp = stats.tableStats[currentLevel].dfp;           
        mag = stats.tableStats[currentLevel].mag + weapon.mag;          
        res = stats.tableStats[currentLevel].res;
        spd = stats.tableStats[currentLevel].spd;
        xpToNextLevel = stats.tableStats[currentLevel].xpToNextLevel;

        //check equipment and add their stats to base

        if (armor != null)
        {
            //armor.Unequip();
            //armor.Equip();
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

    protected override IEnumerator HighlightAvatar()
    {
        highlightAvatarCoroutineOn = true;
        aura.SetActive(true);
        Vector3 initScale = aura.transform.localScale;
        Vector3 destinationScale = new Vector3(initScale.x + 0.2f, initScale.y + 0.2f, initScale.z);
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer auraSr = aura.GetComponent<SpriteRenderer>();
        //auraSr.flipX = sr.flipX;

        //scale up the aura
        while(aura.transform.localScale.x < destinationScale.x)
        {
            Vector3 newScale = aura.transform.localScale;
            float vScale = 0.3f * Time.deltaTime;
            auraSr.color = new Color(auraSr.color.r, auraSr.color.g, auraSr.color.b, auraSr.color.a - 1.3f * Time.deltaTime);
            aura.transform.localScale = new Vector3(newScale.x + vScale, newScale.y + vScale, newScale.z);
            yield return null;
        }

        aura.transform.localScale = initScale;
        auraSr.color = new Color(auraSr.color.r, auraSr.color.g, auraSr.color.b, 1);


        highlightAvatarCoroutineOn = false;
        aura.SetActive(false);
    }

}
