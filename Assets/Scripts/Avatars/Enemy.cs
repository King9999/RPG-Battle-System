using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//enemies are NPCs. Heroes must defeat them. Their actions are randomized based on their skill set and battle conditions.
public abstract class Enemy : Avatar
{
    public EnemyData data;
    [HideInInspector]public int shieldTokens;            //current token count
    public int maxShieldTokens;
    public List<ShieldToken> shields;
    public ShieldToken shieldPrefab;
    public bool shieldBroken {get; set;}
    public List<bool> shieldEnabled {get; set;}
    public int xp;
    public int money;
    protected float skillProb;          //odds that the enemy will do certain attacks.
    public Item commonItemDrop;
    public Item rareItemDrop;
    public float commonItemDropChance;
    public float rareItemDropChance;
    float blindHitChance = 0.2f;        //unique to enemies only.

    //protected CombatSystem cs;
    protected EnemyManager em;
    protected Color skillNameBorderColor = new Color(0.7f, 0.1f, 0.1f);       //used to change skill display border color. Always red


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        className = data.className;
        details = data.details;
        maxHitPoints = data.maxHitPoints;
        hitPoints = maxHitPoints;
        maxManaPoints = data.maxManaPoints;
        manaPoints = maxManaPoints;
        maxShieldTokens = data.maxShieldTokens;
        shieldTokens = maxShieldTokens;
        atp = data.atp;           
        dfp = data.dfp;           
        mag = data.mag;          
        res = data.res;
        spd = data.spd;
        skills = data.skills;
        xp = data.xp;
        money = data.money;
        commonItemDrop = data.commonItemDrop;
        rareItemDrop = data.rareItemDrop;
        commonItemDropChance = data.commonItemDropChance;
        rareItemDropChance = data.rareItemDropChance;

        //add shields
        shields = new List<ShieldToken>();
        shieldEnabled = new List<bool>();
        for(int i = 0; i < maxShieldTokens; i++)
        {
            ShieldToken shield = Instantiate(shieldPrefab);
            shields.Add(shield);
            shieldEnabled.Add(true);
        }

        cs = CombatSystem.instance;
        em = EnemyManager.instance;
    }

    void Update()
    {
        //base.Update();
        //if at any point the enemy's HP reaches 0, it dies
        if (hitPoints <= 0)
        {
            Debug.Log(className + " is defeated");
            SendToGraveyard();
        }

        //if enemy is guard crushed, it ends when bonus turns reaches 0
        if (status == Status.GuardBroken && cs.actGauge.bonusTurns <= 0)
        {            
            status = Status.Normal;
            shieldTokens = maxShieldTokens;
            //shieldBroken = false;          
        }
    }

    public override void Attack(Avatar target)
    {
        UI ui = UI.instance;
        //ui.damageDisplay.color = ui.damageColor;
        //enemy has a 5% chance to inflict a critical. Criticals ignore defense
        float totalDamage;
        float critChance = 0.05f;
        float roll = Random.Range(0, 1f);

        if (roll <= critChance)
        {
            totalDamage = Mathf.Round(atp * atpMod + Random.Range(0, atp * 0.1f));
            ui.damageDisplay.color = ui.criticalDamageColor;
        }
        else
        {
            totalDamage = Mathf.Round(atp * atpMod + Random.Range(0, atp * 0.1f) - (target.dfp * target.dfpMod));
            //ui.damageDisplay.color = ui.damageColor;
        }

        //if enemy is blind, high chance they do 0 damage
        if (status == Status.Blind)
        {
            Debug.Log(className + " is blind!");
            roll = Random.Range(0, 1f);
            if (roll > blindHitChance)
            {
                totalDamage = 0;
                //ui.damageDisplay.color = ui.damageColor;
            }
        }
        
        if (totalDamage < 0)
            totalDamage = 0;
        
        if (!animateAttackCoroutineOn)
            StartCoroutine(AnimateAttack());
            
        ReduceHitPoints(target, totalDamage);
        //Debug.Log(totalDamage + " damage to " + target.className);
    }

    //Used whenever enemy is not instantiated but need a fresh copy
    public virtual void ResetData()
    {
        //Start();
        className = data.className;
        details = data.details;
        maxHitPoints = data.maxHitPoints;
        hitPoints = maxHitPoints;
        maxManaPoints = data.maxManaPoints;
        manaPoints = maxManaPoints;
        maxShieldTokens = data.maxShieldTokens;
        shieldTokens = maxShieldTokens;
        atp = data.atp;           
        dfp = data.dfp;           
        mag = data.mag;          
        res = data.res;
        spd = data.spd;
        skills = data.skills;
        xp = data.xp;
        money = data.money;
        commonItemDrop = data.commonItemDrop;
        rareItemDrop = data.rareItemDrop;
        commonItemDropChance = data.commonItemDropChance;
        rareItemDropChance = data.rareItemDropChance;
    }

    //when enemy dies, they are sent to graveyard
    public void SendToGraveyard(bool ranAway = false)
    {
        if (hitPoints > 0 && !ranAway) return;
      
        em.graveyard.Add(this);
        cs.enemiesInCombat.Remove(this);    //need to make sure the correct enemy is being removed when there are duplicates
        cs.turnOrder.Remove(this);
        cs.UpdateTurnOrderUI();

        //choose another target
        if (cs.enemiesInCombat.Count > 0)
            cs.currentTarget = 0;
        else
            cs.currentTarget = -1;

        //rewards
        if (!ranAway)
        {
            cs.xpPool += xp;
            cs.moneyPool += money;
            cs.RollForLoot(this);
        }

        gameObject.SetActive(false);
    }

    public override void TakeAction()
    {
        base.TakeAction();
        StartCoroutine(HighlightAvatar()); //once this completes, action is taken
        //check status and peform action based on result
        /*switch(status)
        {
            case Status.Normal:
                //call method to execute enemy logic
                ExecuteLogic();
                break;

            case Status.Paralyzed:
                TryRemoveAilment();
                Invoke("PassTurn", invokeTime);
                break;

            case Status.Blind:
                TryRemoveAilment();
                ExecuteLogic();
                break;

            case Status.Charmed:
                //attack a random ally
                Debug.Log(className + " is charmed!");
                int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
                Attack(cs.enemiesInCombat[randTarget]);
                Invoke("PassTurn", invokeTime);
                break;
        }*/
    }

    public override void OnPointerEnter(PointerEventData pointer)
    {
        UI ui = UI.instance;

        string avatarStats = className + "\nATP " + atp + "\nDFP " + dfp + "\nMAG " + mag + "\nRES " + res + "\nSPD "
            + spd + "\n\nSTATUS\n" + status;
        
        string skillSet = "SKILLS\n";

        //display skills
        if (skills.Count <= 0)
            skillSet += "<NONE>";
        else
        { 
            foreach(Skill skill in skills)
            {
                skillSet += skill.skillName + "\n";
            }
        }

        //display dropped items
        string items = "DROPPED ITEMS\n";

        if (commonItemDrop != null)
            items += "(C) " + commonItemDrop.itemName + "\n";
        if (rareItemDrop != null)
            items += "(R) " + rareItemDrop.itemName + "\n";

        ui.combatDataDisplay.DisplayStats(avatarStats, skillSet, items);
    }

    //When an enemy is clicked, it is targeted for attack by the player.
    public override void OnPointerClick(PointerEventData pointer)
    {
        if (cs.selectingTargetToAttack)
        {
            cs.currentTarget = cs.enemiesInCombat.IndexOf(this);
            cs.selectingTargetToAttack = false;
            UI ui = UI.instance;
            ui.selectTargetUI.gameObject.SetActive(false);
            ui.combatMenu.ShowCombatMenu(false);
            
            //hero is ready to attack
            Hero hero = cs.heroesInCombat[cs.currentHero];
            hero.SetupActionGauge(cs.actGauge, hero.weapon.actGauge);
            hero.isAttacking = true;
            //Debug.Log(className + " at index " + cs.currentTarget);
        }
    }

    public void ResetShieldToken(int tokenIndex)
    {
        //shield tokens start at a random position to make it difficult for player to predict. The token does not start
        //at the 0 index.
        ActionGauge actGauge = ActionGauge.instance;
        int randPanel = Random.Range(1, actGauge.panels.Length);
        Vector3 shieldTokenPos = new Vector3(actGauge.panels[randPanel].transform.position.x /*+ (panelSize / 2)*/, 
            actGauge.panels[randPanel].transform.position.y, transform.position.z);

        shields[tokenIndex].transform.position = shieldTokenPos;
        actGauge.currentShieldTokenSize[tokenIndex] = 0;
        actGauge.currentShieldTokenIndex[tokenIndex] = randPanel;
        actGauge.shieldTokenDirection[tokenIndex] = -1;   //moves from right to left by default
    }

    public virtual void ExecuteLogic() { Invoke("PassTurn", invokeTime); }

    protected override IEnumerator AnimateAttack()
    {
        animateAttackCoroutineOn = true;
        Vector3 initPos = transform.position;
        Vector3 destination = new Vector3(initPos.x - 2, initPos.y, initPos.z);

        while(transform.position.x > destination.x)
        {
            float vx = 30 * Time.deltaTime;
            transform.position = new Vector3(transform.position.x - vx, transform.position.y, transform.position.z);
            yield return null;
        }

        //return
        while(transform.position.x < initPos.x)
        {
            float vx = 30 * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + vx, transform.position.y, transform.position.z);
            yield return null;
        }

        //resturn to init position
        transform.position = initPos;
        animateAttackCoroutineOn = false;
    }

    protected override IEnumerator HighlightAvatar()
    {
        //highlightAvatarCoroutineOn = true;
        aura.SetActive(true);
        Vector3 initScale = aura.transform.localScale;
        Vector3 destinationScale = new Vector3(initScale.x + 0.2f, initScale.y + 0.2f, initScale.z);
        SpriteRenderer auraSr = aura.GetComponent<SpriteRenderer>();

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
        aura.SetActive(false);

        //check status and peform action based on result
        switch(status)
        {
            case Status.Normal:
                //call method to execute enemy logic
                ExecuteLogic();
                break;

            case Status.Paralyzed:
                TryRemoveAilment();
                Invoke("PassTurn", invokeTime);
                break;

            case Status.Blind:
                TryRemoveAilment();
                ExecuteLogic();
                break;

            case Status.Charmed:
                //attack a random ally
                Debug.Log(className + " is charmed!");
                int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
                Attack(cs.enemiesInCombat[randTarget]);
                Invoke("PassTurn", invokeTime);
                break;

            case Status.GuardBroken:
                //nothing happens
                Debug.Log(className + " is guard crushed!");
                Invoke("PassTurn", invokeTime);
                break;
        }
    }

    protected override IEnumerator AnimateRun()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while(sr.color.a > 0)
        {
            float vAlpha = sr.color.a - 2 * Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, vAlpha);
            yield return null;
        }  

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1); 
        SendToGraveyard(ranAway: true);
          
    }
    
}
