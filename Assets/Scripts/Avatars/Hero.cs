using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    //protected Color skillNameBorderColor = new Color(0.2f, 0.4f, 0.95f);
    public bool actionCompleted;
    public bool isAttacking;
    public int currentActions;
    [HideInInspector]public bool landedOnCritPanel;        //used by rogue. Applied after using Hide skill.
    public enum HeroClass {Barbarian, Rogue, Mage, Cleric}
    public HeroClass heroClass;

    //coroutines
    bool highlightAvatarCoroutineOn;

    //singletons
    protected HeroManager hm;
    CombatInputManager cim;
    Inventory inv;                  //for populating skill inventory menu

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //skills = new List<Skill>();     //used to ensure the skill list is cleared upon starting game
        skillNameBorderColor = new Color(0.2f, 0.4f, 0.95f);
        hm = HeroManager.instance;
        cim = CombatInputManager.instance;
        gm = GameManager.instance;
        //status = Status.Blind; 
    }

    public void GetData(HeroData data)
    {
        skills = new List<Skill>();     //used to ensure the skill list is cleared upon starting game
        //pull information from a scriptable object
        this.data = data;
        statFile = data.statFile;
        className = data.className;
        details = data.details;
        heroClass = (Hero.HeroClass)data.heroClass;
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

        /*if (status == Status.Dead)
        {
            //status = Status.Dead;
            hitPoints = 0;
            //remove hero from turn order
            //TODO: put in a sprite that indicates hero is dead.
            return;
        }*/

        if (status == Status.Normal || status == Status.Poisoned || status == Status.Blind || status == Status.Berserk)
        {
            if ((isAttacking && currentActions >= totalAttackTokens) || gm.gameState == GameManager.GameState.ShowCombatRewards)
            {
                isAttacking = false;
                cs.actGauge.actionToken.SetSpeedToDefault();

                //reset shield token if applicable
                foreach(ShieldToken shield in cs.enemiesInCombat[cs.currentTarget].shields)
                {
                    if (shield.isEnabled)
                    {
                        shield.SetSpeedToDefault();
                    }
                }

                //hide gauge and tokens
                cs.actGauge.ShowGauge(false);

                foreach(ShieldToken shield in cs.enemiesInCombat[cs.currentTarget].shields)
                    shield.ShowToken(false);

                isTheirTurn = false;
                EndTurn();
            }

        }

    }

    public Color SkillBorderColor() { return skillNameBorderColor; }

    public override void UpdateSkillEffects()
    {
        //skill activation check    
        for (int i = 0; i < skillEffects.Count; i++)
        {
            if (skillEffects[i].hasDuration)
            {
                skillEffects[i].ReduceDuration();
                if (skillEffects[i].EffectExpired())
                {
                    skillEffects[i].RemoveEffects(this);
                    skillEffects.Remove(skillEffects[i]);
                    i--;
                }
            }
            else //permanent effect/passive
            {
                skillEffects[i].Activate(this, skillNameBorderColor);
            }  
        }
    }

    public override void OnPointerClick(PointerEventData pointer)
    {
        if (cs.selectingHero)
        {
            //activate item or skill
            Inventory inv = Inventory.instance;
            cs.currentTarget = cs.heroesInCombat.IndexOf(this);
            inv.copiedSlot.ItemInSlot().itemEffect.Activate(cs.heroesInCombat[cs.currentHero], cs.heroesInCombat[cs.currentTarget], skillNameBorderColor);
            inv.copiedSlot.quantity--;

            //delete item
            if(inv.copiedSlot.quantity <= 0)
            {
                inv.copiedSlot.RemoveItem(); //this also deletes the item in the original slot since copied slot holds a reference.
            }
           
            //cs.currentHero = cs.heroesInCombat.IndexOf(this);
            cs.selectingHero = false;
            UI ui = UI.instance;
            ui.selectTargetUI.gameObject.SetActive(false);
            ui.combatMenu.ShowCombatMenu(false);

            //End turn.
            cs.heroesInCombat[cs.currentHero].EndTurn();
            //StartCoroutine(cs.heroesInCombat[cs.currentHero].DelayPassiveSkillActivation());
            //cs.heroesInCombat[cs.currentHero].UpdateSkillEffects();
            
            //cs.heroesInCombat[cs.currentHero].Invoke("PassTurn", invokeTime);
        }

        if (cs.selectingHeroToUseSkillOn)
        {
            //activate item or skill
            Inventory inv = Inventory.instance;
            cs.currentTarget = cs.heroesInCombat.IndexOf(this);

            cs.selectingHeroToUseSkillOn = false;
            UI ui = UI.instance;
            ui.selectTargetUI.gameObject.SetActive(false);
            ui.combatMenu.ShowCombatMenu(false);

            //hero is ready to attack
            Hero hero = cs.heroesInCombat[cs.currentHero];
            hero.SetupActionGaugeForSkill(cs.actGauge, inv.copiedSkillSlot.SkillInSlot().actGaugeData);
            hero.isAttacking = true;    //always have this line even if not attacking enemy. This will let the appropriate code in Update execute.

        }
    }

    public override void TakeAction()
    {
        base.TakeAction();

        //which hero is acting?
        cs.currentHero = cs.heroesInCombat.IndexOf(this);

        //populate skill inventory if possible
        inv = Inventory.instance;
        for (int i = 0; i < inv.skillSlots.Length; i++)
        {
            inv.skillSlots[i].RemoveSkill();
        }
        foreach(Skill skill in skills)
        {
            inv.AddSkill(this, skill);
        }

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
                EndTurn();
                //Invoke("PassTurn", invokeTime);
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
                EndTurn();
                //Invoke("PassTurn", invokeTime);
                break;

            case Status.Berserk:
                //Barbarian attacks a random enemy while under the effects of berserk skill.
                Debug.Log(className + " is berserk!");
                int randEnemy = Random.Range(0, cs.enemiesInCombat.Count);
                Attack(cs.enemiesInCombat[randEnemy]);
                EndTurn();
                break;
        }    
    }

   
    public override void Attack(Avatar target)
    {
        UI ui = UI.instance;
        ui.damageDisplay.color = ui.damageColor;    //reset colour to default
        if (status != Status.Charmed && status != Status.Berserk)
        {
           
            //wait for button press to attack. Do this until no more tokens available
            if (cim.buttonPressed)
            {
                //stop tokens
                cs.actGauge.actionToken.StopToken();

                List<ShieldToken> shields = cs.enemiesInCombat[cs.currentTarget].shields;
                for (int i = 0; i < shields.Count; i++)
                {
                    if (shields[i].isEnabled)
                        shields[i].StopToken();
                }

                //check if the action token landed on any shield
                bool landedOnShield = false;
                int j = 0;
                while (!landedOnShield && j < shields.Count)
                {
                    if (shields[j].isEnabled && cs.actGauge.currentIndex == cs.enemiesInCombat[cs.currentTarget].currentShieldTokenIndex[j])
                    {
                        landedOnShield = true;
                    }
                    else
                    {
                        j++;
                    }
                }

                //check if we landed on the same space as the shield token
                if (landedOnShield)
                {
                    //action is blocked. Shield takes damage if hero attacked.
                    switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
                    {
                        //Can use the code below to test a range of values in switch conditions
                        case ActionGauge.ActionValue.Normal:
                        case ActionGauge.ActionValue.Reduced:
                        case ActionGauge.ActionValue.Critical:
                        //case ActionGauge.ActionValue i when (i >= ActionGauge.ActionValue.Normal && i <= ActionGauge.ActionValue.Critical):
                            shields[j].hitPoints -= 1;
                            if (shields[j].hitPoints <= 0)
                            { 
                                cs.bonusTurns += cs.heroesInCombat.Count + 1;  //+1 ensures all heroes get a bonus, and enemy misses a turn.
                                cs.enemiesInCombat[cs.currentTarget].status = Status.GuardBroken;
                                shields[j].isEnabled = false;
                                shields[j].ShowToken(false);

                                //get a random bonus. If we get the special bonus, that one takes priority
                                int randBonus = Random.Range(0, cs.bonusSystem.bonusValues.Count);

                                if (cs.bonusSystem.bonusValues.Contains(BonusSystem.BonusValue.FullRestore))
                                {
                                    if(Random.value <= 0.02f)
                                    {
                                        randBonus = (int)BonusSystem.BonusValue.FullRestore;
                                    }
                                }

                                cs.bonusSystem.GetBonus((BonusSystem.BonusValue)randBonus);

                                //check if we got certain bonuses and apply them immediately
                                ActionGauge actGauge = ActionGauge.instance;
                                if (randBonus == (int)BonusSystem.BonusValue.AllCriticalPanels)
                                {
                                    //ActionGauge actGauge = ActionGauge.instance;
                                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Critical);
                                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Miss, ActionGauge.ActionValue.Critical);
                                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Reduced, ActionGauge.ActionValue.Critical);
                                }

                                if (randBonus == (int)BonusSystem.BonusValue.ActionGaugeSlowed)
                                {
                                    actGauge.actionToken.SetTokenSpeed(actGauge.actionToken.TokenSpeed() * cs.bonusSystem.actionGaugeMod);
                                }
                            }
                            else    //shield is stunned
                            {
                                shields[j].StunToken();
                                Debug.Log("Shield stunned");
                            }
                            ui.DisplayBlockResult(shields[j]);
                            break;

                        default:
                            ui.DisplayBlockResult(shields[j]);
                            break;
                    }
                }
                else
                {
                    float totalDamage = 0;
                    switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
                    {
                        case ActionGauge.ActionValue.Normal:
                            //deal damage to enemy
                            totalDamage = Mathf.Round(atp * atpMod + Random.Range(0, atp * 0.1f) - (target.dfp * target.dfpMod));
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
                            if (!animateAttackCoroutineOn)
                                StartCoroutine(AnimateAttack());
                            //ReduceHitPoints(target, totalDamage);
                            ui.DisplayStatusUpdate("MISS", target.transform.position);
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
                                
                                case Skill.Target.OneEnemy:
                                    weapon.weaponSkill.Activate(this, target, skillNameBorderColor);
                                    break;

                                case Skill.Target.AllEnemies:
                                    //weapon.weaponSkill.Activate(this, skillNameBorderColor);
                                    break;
                            }
                            SpeedUpToken();
                            break;
                    }
                }

            }
                    
        }
        else    //player is charmed/berserk, do a regular attack with a chance of crit
        {
            //5% chance to inflict a critical. Criticals ignore defense
            float totalDamage;
            float critChance = 0.05f;
            //float roll = Random.Range(0, 1f);

            if (Random.value <= critChance)
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
                //roll = Random.Range(0, 1f);
                if (Random.value > blindHitChance)
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

            //set up shield token if applicable
            if (cs.bonusTurns <= 0 && cs.enemiesInCombat[cs.currentTarget].maxShieldTokens > 0)
            {
                List<ShieldToken> shields = cs.enemiesInCombat[cs.currentTarget].shields;
                foreach(ShieldToken shield in shields)
                {
                    if (!shield.isEnabled)
                    {
                        shield.GenerateToken();
                        shield.SetSpeedToDefault();
                        shield.StartToken();
                    }
                    shield.ShowToken(true);
                    cs.enemiesInCombat[cs.currentTarget].ResetShieldToken(shields.IndexOf(shield));
                }
                
            }

            //check bonuses
            /*bool bonusFound = false;
            int i = 0;
            while (cs.bonusTurns > 0 && !bonusFound && i < cs.bonusSystem.activeBonuses.Count)
            {
                if (cs.bonusSystem.activeBonuses[i] == BonusSystem.BonusValue.AllCriticalPanels)
                {
                    bonusFound = true;
                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Critical);
                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Miss, ActionGauge.ActionValue.Critical);
                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Reduced, ActionGauge.ActionValue.Critical);
                }
                else
                {
                    i++;
                }
            }*/
            if (cs.bonusSystem.allPanelsCritical)
            {
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Critical);
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Miss, ActionGauge.ActionValue.Critical);
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Reduced, ActionGauge.ActionValue.Critical);
            }

            //certain panels change if player is blind
            if (status == Status.Blind)
            {
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Miss);
                actGauge.ChangeActionValue(ActionGauge.ActionValue.Critical, ActionGauge.ActionValue.Miss);
            }

            //Rogue-specific effect
            if (status == Avatar.Status.HideBuffInEffect)
            {
                status = Avatar.Status.Normal;
                if(landedOnCritPanel)
                {
                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Critical);
                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Reduced, ActionGauge.ActionValue.Critical);
                    landedOnCritPanel = false;
                }
                else
                    actGauge.ChangeActionValue(ActionGauge.ActionValue.Normal, ActionGauge.ActionValue.Critical);
                
            }

            totalAttackTokens = (weapon.tokenCount + attackTokenMod < 1) ? 1 : weapon.tokenCount + attackTokenMod;
            currentActions = 0;

            //check for bonus
            actGauge.actionToken.SetTokenSpeed(actGauge.actionToken.TokenSpeed() * cs.bonusSystem.actionGaugeMod);
            actGauge.actionToken.StartToken();
        }
    }

    public void SetupActionGaugeForSkill(ActionGauge actGauge, ActionGaugeData skillData)
    {
        if (!actGauge.gameObject.activeSelf)
        {
            actGauge.ShowGauge(true);
            actGauge.UpdateGaugeData(skillData);
            actGauge.ResetActionToken();
        }

        totalAttackTokens = 1;
        currentActions = 0;

        float skillTokenSpeed = actGauge.actionToken.TokenSpeed() * 1.8f;
        actGauge.actionToken.SetTokenSpeed(skillTokenSpeed);
        actGauge.actionToken.StartToken();
    }

    public void SpeedUpToken()
    {
        cs.actGauge.ResetActionToken();
        float newSpeed = cs.actGauge.actionToken.TokenSpeed() * 1.2f;
        cs.actGauge.actionToken.SetTokenSpeed(newSpeed);
        currentActions++;
        cim.buttonPressed = false;

        //shield token also speeds up, but position is not reset
        foreach(ShieldToken shield in cs.enemiesInCombat[cs.currentTarget].shields)
        {
            if (shield.isEnabled)
            {
                newSpeed = shield.TokenSpeed() * 1.2f;
                shield.SetTokenSpeed(newSpeed);
            }
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
            //armor.Unequip(this);
            //armor.Equip(this);
            dfp += armor.dfp;
            res += armor.res;
            //dfp = stats.tableStats[currentLevel].dfp + armor.dfp;           
           // res = stats.tableStats[currentLevel].res + armor.res;
        }

        if (trinket != null)
        {
            //trinket.Unequip(this);
            //trinket.Equip(this);
            maxHitPoints += trinket.maxHitPoints;
            manaPoints += trinket.maxManaPoints;
            atp += trinket.atp;
            dfp += trinket.dfp;
            mag += trinket.mag;
            res += trinket.res;
            spd += trinket.spd;
            attackTokenMod += trinket.attackTokenMod;
            hpMod += trinket.hpMod;
            mpMod += trinket.mpMod;
            atpMod += trinket.atpMod;
            dfpMod += trinket.dfpMod;
            magMod += trinket.magMod;
            resMod += trinket.resMod;
            spdMod += trinket.spdMod;
        }

        //check if new skill is learned
        if (stats.tableStats[currentLevel].newSkill != null)
        {
            hm = HeroManager.instance;
            cs = CombatSystem.instance;
            //check which hero we're looking at to search the correct array
            if (heroClass == HeroClass.Barbarian)
            {
                foreach(Skill barbSkill in hm.barbSkills)
                {
                    if (barbSkill.name == stats.tableStats[currentLevel].newSkill)
                    {
                        skills.Add(barbSkill);
                        cs.newSkillLearned = true;
                        break;
                    }
                }
            }

            else if (heroClass == HeroClass.Rogue)
            {
                foreach(Skill skill in hm.rogueSkills)
                {
                    if (skill.name == stats.tableStats[currentLevel].newSkill)
                    {
                        skills.Add(skill);
                        cs.newSkillLearned = true;
                        break;
                    }
                }
            }

            else if (heroClass == HeroClass.Mage)
            {
                foreach(Skill skill in hm.mageSkills)
                {
                    if (skill.name == stats.tableStats[currentLevel].newSkill)
                    {
                        skills.Add(skill);
                        cs.newSkillLearned = true;
                        break;
                    }
                }
            }

            else if (heroClass == HeroClass.Cleric)
            {
                foreach(Skill skill in hm.clericSkills)
                {
                    if (skill.name == stats.tableStats[currentLevel].newSkill)
                    {
                        skills.Add(skill);
                        cs.newSkillLearned = true;
                        break;
                    }
                }
            }
        }

    }

    public override void PassTurn()
    {
        base.PassTurn();
        Debug.Log(className + " turn ended");

        UI ui = UI.instance;
        ui.combatMenu.ShowCombatMenu(false);

        //reduce bonus turn
        if (cs.bonusTurns > 0)
            cs.bonusTurns--;
    }

    #region Coroutines

    public override void ResetCoroutines()
    {
        animateAttackCoroutineOn = false;
        highlightAvatarCoroutineOn = false;
    }

    //reset certain values after combat.
    public void ResetData()
    {
        atpMod = 1;
        dfpMod = 1;
        spdMod = 1;
        magMod = 1;
        resMod = 1;

        animateAttackCoroutineOn = false;
        highlightAvatarCoroutineOn = false;

        //remove all effects
        skillEffects.Clear();

        //clear certain status effects
        if (status == Status.Paralyzed || status == Status.Charmed || status == Status.Berserk)
            status = Status.Normal;

        /*foreach(Skill effect in skillEffects)
        {
            effect.RemoveEffects(this);
        }*/
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

        //return to init position
        transform.position = initPos;
        animateAttackCoroutineOn = false;
        SpeedUpToken();

    }

    protected override IEnumerator HighlightAvatar()
    {
        highlightAvatarCoroutineOn = true;
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


        highlightAvatarCoroutineOn = false;
        aura.SetActive(false);
    }

    protected override IEnumerator DelayPassiveSkillActivation()
    {
        yield return new WaitForSeconds(1);

        UpdateSkillEffects();

        yield return new WaitForSeconds(invokeTime);

       PassTurn();
    } 
    #endregion

}
