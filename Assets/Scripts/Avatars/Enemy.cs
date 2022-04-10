using UnityEngine;
using System.Collections;

//enemies are NPCs. Heroes must defeat them. Their actions are randomized based on their skill set and battle conditions.
public abstract class Enemy : Avatar
{
    public EnemyData data;
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
    protected virtual void Start()
    {
        className = data.className;
        details = data.details;
        maxHitPoints = data.maxHitPoints;
        hitPoints = maxHitPoints;
        maxManaPoints = data.maxManaPoints;
        manaPoints = maxManaPoints;
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
    }

    public override void Attack(Avatar target)
    {
        //enemy has a 5% chance to inflict a critical. Criticals ignore defense
        float totalDamage;
        float critChance = 0.05f;
        float roll = Random.Range(0, 1f);

        if (roll <= critChance)
        {
            totalDamage = atp + Mathf.Round(Random.Range(0, atp * 0.1f));
        }
        else
        {
            totalDamage = atp + Mathf.Round(Random.Range(0, atp * 0.1f)) - target.dfp;
        }

        //if enemy is blind, high chance they do 0 damage
        if (status == Status.Blind)
        {
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
    public void SendToGraveyard()
    {
        if (hitPoints > 0) return;
      
        em.graveyard.Add(this);
        cs.enemiesInCombat.Remove(this);    //need to make sure the correct enemy is being removed when there are duplicates
        cs.turnOrder.Remove(this);

        //rewards
        cs.xpPool += xp;
        cs.moneyPool += money;
        cs.RollForLoot(this);

        gameObject.SetActive(false);
    }

    public override void TakeAction()
    {
        base.TakeAction();
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
        }
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
    
}
