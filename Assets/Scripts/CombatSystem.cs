using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;            //used to grey out inventory items that can't be used in combat

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroesInCombat;
    public List<Enemy> enemiesInCombat;
    public Dictionary<int, string> modifiedEnemyNames;  //modified names of duplicate enemies.
    int totalEnemies {get;} = 6;
    public int currentEnemyTarget {get; set;}    //index of an avatar being targeted.
    public int currentHeroTarget {get; set;}
    public int currentHero {get; set;}      //index of hero who is taking action
    string group = "ABCDE";                 //used to rename enemies if there are duplicates
    //public List<Item> loot;
    public Dictionary<Item, int> loot;
    public int xpPool;                      //total amount of XP from defeated enemies
    public int moneyPool;

    public ActionGauge actGauge;
    public int bonusTurns {get; set;}       //awarded when enemy shield is crushed.
    public BonusSystem bonusSystem;         //governs the bonuses received in battle.

    public List<Avatar> turnOrder;
    [HideInInspector]public int currentTurn;                        //iterator for turnOrder
    public Transform[] enemyLocations;      //avatars are placed in these positions in battle.
    bool[] enemyLocationOccupied;
    public Transform[] heroLocations;
    bool[] heroLocationOccupied;
    bool combatEnabled;         //when true, combat starts and all combat elements apppear in foreground.
    bool combatEnded;           //if true, will run the EndCombat method and prevent it from running more than once.
    public bool turnInProgress {get; set;}
    public bool speedChanged {get; set;}        //if true, the turn order is reshuffled.
    public bool playerRanAway {get; set;}                 //if true, no rewards given.
    public bool newSkillLearned {get; set;}     //used to display new skills added after leveling up
    public Transform actGaugeLocation;

    //combat states. Used to determine which steps can be taken during combat
    public enum PlayerState {None, SelectingTargetToAttack, InventoryOpen, SkillOpen}
    public PlayerState playerState;

    public bool selectingTargetToAttack {get; set;}
    public bool selectingTargetToAttackWithSkill {get; set;}
    public bool selectingHero {get; set;}           //when true, player is using an item or a skill on a hero
    public bool selectingHeroToUseSkillOn {get; set;}
    public bool heroUsingSkill;                     //used in combat input manager to prevent standard attack.
    
    public static CombatSystem instance;
    HeroManager hm;
    EnemyManager em;
    GameManager gm;
    UI ui;
    float currentTime;
    float delay;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    public void SetupCombat(List<Enemy> enemies, int enemyPosition = -1)
    {
        hm = HeroManager.instance;
        em = EnemyManager.instance;
        gm = GameManager.instance;
        ui = UI.instance;
        gm.SetState(GameManager.GameState.Combat);
        
        //use this instead of Start() to determine the enemies encountered and to set up combat
        modifiedEnemyNames = new Dictionary<int, string>();

        //action gauge setup
        actGauge.gameObject.SetActive(false);
        actGauge.actionToken.SetSpeedToDefault();

        actGauge.transform.position = actGaugeLocation.position;

        loot = new Dictionary<Item, int>();

        //heroes and enemies must be instantiated here. We check graveyard before instantiating new enemies.
        foreach(Hero hero in hm.heroes)
        {
            hero.gameObject.SetActive(true);
            //hero.ResetCoroutines();
            //hero.ResetData();
            //hero.transform.SetParent(transform);
            heroesInCombat.Add(hero);
        }

        //add enemies. Enemies are re-used when able.
        for (int i = 0; i < enemies.Count; i++)
        {
            bool enemyInstantiated = false;
            int enemyID = enemies[i].enemyID;
            Enemy enemy = null;

            //search through graveyard
            foreach(Enemy e in em.graveyard)
            {
                if (e.className == em.enemies[enemyID].className)
                {
                    //restore enemy from graveyard
                    enemy = e;
                    enemy.gameObject.SetActive(true);
                    enemy.ResetData();
                    //reset shield tokens if applicable  TODO: May not need to do this step
                    for (int x = 0; x < enemy.shieldTokens; x++)
                    {
                        //enemy.shields[x].gameObject.SetActive(true);
                        //enemy.shields[x].isEnabled = true;
                        enemy.shields[x].ResetToDefault();
                        //enemy.shields[x].SetSpeedToDefault();

                    }

                    em.graveyard.Remove(e);
                    enemyInstantiated = true;
                    break;
                }
            }
            if (!enemyInstantiated)
            {
                enemy = Instantiate(em.enemies[enemyID]);
                enemy.transform.SetParent(transform);
            }
            enemiesInCombat.Add(enemy);
        }

        //check enemy names and append a letter to duplicates
        CheckDuplicateNames();
        
        //place heroes and enemies in random positions
        heroLocationOccupied = new bool[heroLocations.Length];
        enemyLocationOccupied = new bool[enemyLocations.Length];

        foreach (Hero hero in heroesInCombat)
        {
            int randIndex = Random.Range(0, heroLocationOccupied.Length);

            while(heroLocationOccupied[randIndex] == true)
            {
                randIndex = Random.Range(0, heroLocationOccupied.Length);
            }
            hero.transform.position = heroLocations[randIndex].position;

            //The code below is used to shift the heroes' position a bit because I'm using a smaller screen.
            /*#if UNITY_WEBGL
                hero.transform.position = new Vector3(hero.transform.position.x + 2, hero.transform.position.y, 1);
            #endif*/

            heroLocationOccupied[randIndex] = true;

            if (hero.status != Avatar.Status.Dead)
                turnOrder.Add(hero);

            //UI setup
            hero.GetComponent<ClampText>().UpdateUIPosition();
            hero.UpdateStatsUI();
        }

      

        if (enemyPosition < 0)
        {
            foreach (Enemy enemy in enemiesInCombat)
            {
                int randIndex = Random.Range(0, enemyLocationOccupied.Length);

                while(enemyLocationOccupied[randIndex] == true)
                {
                    randIndex = Random.Range(0, enemyLocationOccupied.Length);
                }
                enemy.transform.position = enemyLocations[randIndex].position;
                enemyLocationOccupied[randIndex] = true;

                turnOrder.Add(enemy);

                //UI setup
                enemy.GetComponent<ClampText>().UpdateUIPosition();
                enemy.UpdateStatsUI();
            }
        }
        else //There's only 1 enemy, and they are placed in a specific location
        {
            Enemy enemy = enemiesInCombat[0];
            enemy.transform.position = enemyLocations[enemyPosition].position;

            turnOrder.Add(enemy);

            //UI setup
            enemy.GetComponent<ClampText>().UpdateUIPosition();
            enemy.UpdateStatsUI();
        }

        //get turn order and set up UI.
        ShuffleTurnOrder();              
        //turnOrder = turnOrder.OrderByDescending(x => x.spd * x.spdMod).ToList();   //IMPORTANT: Lambda operations should not execute in update loop
        UpdateTurnOrderUI();

        //reset variables 
        currentTurn = 0;
        currentEnemyTarget = -1;
        currentHero = -1;
        currentHeroTarget = -1;
        combatEnded = false;
        turnInProgress = false;
        playerRanAway = false;

        //check player inventory for any items that can't be used in combat and grey them out.
        Inventory inv = Inventory.instance;
        foreach(ItemSlot slot in inv.itemSlots)
        {
            if (slot.ItemInSlot() == null) return;
            if (slot.ItemInSlot().cannotUseInCombat)
            {
                TextMeshProUGUI currentSlot = slot.GetComponentInChildren<TextMeshProUGUI>();
                currentSlot.text = "<color=#555555>" + slot.ItemInSlot().itemName + " -- " + slot.quantity + "</color>";
            }
        }

    }

    
    // Update is called once per frame
    void Update()
    {
        if (combatEnded)
            return;
        
        if (AllHeroesDefeated())
        {
            gm.GameOver();
            //return;
        }

        if (!combatEnded && enemiesInCombat.Count <= 0)
        {
            EndCombat();
            combatEnded = true;
            //return;
        }

        if (!combatEnded && playerRanAway)
        {
            combatEnded = true;

            //send remaining enemies to graveyard
            for (int i = 0; i < enemiesInCombat.Count; i++)
            {
                enemiesInCombat[i].SendToGraveyard();
                i--;
            }
            
            CloseCombatSystem();
        }

        if (!combatEnded && bonusSystem.bonusTurnsActive && bonusTurns <= 0)
        {
            bonusTurns = 0;
            //bonusSystem.bonusTurnsActive = false;
            bonusSystem.RemoveAllBonuses();
        }

        //check whose turn is next
        if (!combatEnded && !turnInProgress)
        {
            if (!turnOrder[currentTurn].TurnTaken())
            {
                turnOrder[currentTurn].TakeAction();
            }
            else   //if turn has ended, put avatar to end of list
            {
                Avatar copy = turnOrder[currentTurn];

                for (int i = currentTurn + 1; i < turnOrder.Count; i++)
                {
                    turnOrder[i - 1] = turnOrder[i];
                }

                turnOrder[turnOrder.Count - 1] = copy;
                turnOrder[turnOrder.Count - 1].SetTurnTaken(false); //need to do this step or we eventually end up in a loop

                //reshuffle turn order if SPD was updated for any avatar
                //if the avatar who just took their turn has raised their speed, they're placed in a higher position.
                if (speedChanged)
                {
                    speedChanged = false;

                    int i = turnOrder.Count - 1;
                    bool greaterSpdFound = false; 
                    while (i > 0 && !greaterSpdFound)
                    {
                        //compare speeds
                        float currentAvatarSpd = turnOrder[i - 1].spd * turnOrder[i - 1].spdMod;
                        if (copy.spd * copy.spdMod > currentAvatarSpd)
                        {
                            //move the target ahead
                            Avatar temp = turnOrder[i - 1];
                            turnOrder[i - 1] = copy;
                            turnOrder[i] = temp;
                            i--;
                        }
                        else
                        {
                            greaterSpdFound = true;
                        }
                    }
                }

                //display new turn order
                UpdateTurnOrderUI();
            }
        }     
    }

    public void RollForLoot(Enemy enemy)
    {
        if (enemy.commonItemDrop == null && enemy.rareItemDrop == null)
            return;

        //roll for rare drop then common drop in that order
        float roll = Random.Range(0, 1f) - bonusSystem.rareDropMod;

        if (enemy.rareItemDrop != null && roll <= enemy.rareItemDropChance)
        {
            //award rare item to player
            if (loot.ContainsKey(enemy.rareItemDrop))
            {
                loot[enemy.rareItemDrop] += 1;
            }
            else
            {
                loot.Add(enemy.rareItemDrop, 1);
            }
        }
        else if (enemy.commonItemDrop != null && roll <= enemy.commonItemDropChance)
        {
            if (loot.ContainsKey(enemy.commonItemDrop))
            {
                loot[enemy.commonItemDrop] += 1;
            }
            else
            {
                loot.Add(enemy.commonItemDrop, 1);
            }
        }
        
    }

    void EndCombat(bool escapedCombat = false)
    {
        //set game state
        gm.gameState = GameManager.GameState.ShowCombatRewards;
        actGauge.ShowGauge(false);
        turnOrder.Clear();
        ui.turnOrderList.text = "";
        bonusTurns = 0;
        
        //apply any bonuses
        int bonus = bonusSystem.xpMoneyMod <= 0 ? 0 : xpPool / bonusSystem.xpMoneyMod;
        xpPool += bonus;
        moneyPool += bonus;

        //award XP, money and items
        string xpAndMoney = xpPool + " EXP\n" + moneyPool + " Money";
        string[] partyXp = new string[heroesInCombat.Count];

        //XP gain.
        //dead heroes do not receive XP
        int liveHeroCount = 0;
        int currentHero = 0;
        while (currentHero < heroesInCombat.Count)
        {
            if (heroesInCombat[currentHero].status != Avatar.Status.Dead && !heroesInCombat[currentHero].AtMaxLevel())
                liveHeroCount++;

            currentHero++;
        }

        //int xpShare = xpPool / liveHeroCount;
        for (int i = 0; i < heroesInCombat.Count; i++)
        {
            int xpShare;
            heroesInCombat[i].gameObject.SetActive(false);

            if (heroesInCombat[i].status == Avatar.Status.Dead || heroesInCombat[i].AtMaxLevel())
                xpShare = 0;
            else
                xpShare = xpPool / liveHeroCount;
    
            heroesInCombat[i].xpToNextLevel -= xpShare;
            if (heroesInCombat[i].xpToNextLevel <= 0 && !heroesInCombat[i].AtMaxLevel())
            {
                //keep leveling up as long as the xp share exceeds to next level.
                int levelUpCount = 0;
                while (heroesInCombat[i].xpToNextLevel <= 0 && !heroesInCombat[i].AtMaxLevel())
                {
                    heroesInCombat[i].LevelUp();
                    levelUpCount++;

                    if (heroesInCombat[i].AtMaxLevel())
                        xpShare = 0;

                    heroesInCombat[i].xpToNextLevel -= xpShare;   
                }
                
                if (newSkillLearned)
                {
                    newSkillLearned = false;
                    int newestSkillLearned = heroesInCombat[i].skills.Count - 1;
                    partyXp[i] = heroesInCombat[i].className + " Level +" + levelUpCount + "\nLearned \"" + heroesInCombat[i].skills[newestSkillLearned].skillName
                        + "\"\nEXP Gained: " + xpShare + "\nTo Next Level " + heroesInCombat[i].xpToNextLevel;
                }
                else
                    partyXp[i] = heroesInCombat[i].className + " Level +" + levelUpCount + "\nEXP Gained: " + xpShare + "\nTo Next Level " 
                        + heroesInCombat[i].xpToNextLevel;
            }
            else
            {
                partyXp[i] = heroesInCombat[i].className + "\nEXP Gained: " + xpShare + "\nTo Next Level " 
                    + heroesInCombat[i].xpToNextLevel;
            }
        }

        //show loot
        string lootString = "Dropped Items\n";

        
        //Debug.Log("Battle over!");

        //Debug.Log((xpPool / heroesInCombat.Count) + " XP awarded\n" + moneyPool + " money awarded");
        if (loot.Count <= 0)
            lootString += "<No Loot>";
        else
        {
            foreach(KeyValuePair<Item,int> item in loot)
            {
                lootString += item.Key.itemName + " x" + item.Value + "\n";

                //add to inventory
                if (item.Key.itemType == Item.ItemType.Consumable)
                    ui.inventory.AddItem((Consumable)item.Key, item.Value);

                if (item.Key.itemType == Item.ItemType.Weapon)
                    ui.inventory.AddItem((Weapon)item.Key, item.Value);

                if (item.Key.itemType == Item.ItemType.Armor)
                    ui.inventory.AddItem((Armor)item.Key, item.Value);

                if (item.Key.itemType == Item.ItemType.Trinket)
                    ui.inventory.AddItem((Trinket)item.Key, item.Value);
            }
        }

        ui.rewardsDisplay.ShowRewards(xpAndMoney, partyXp, lootString);

        foreach(Hero hero in heroesInCombat)
        {
            hero.currentXp += xpPool / heroesInCombat.Count;
            hero.ResetData();
        }

        //add money
        ui.inventory.AddMoney(moneyPool);

    }

    bool AllHeroesDefeated()
    {
        float totalHitPoints = 0;

        foreach(Hero hero in heroesInCombat)
        {
            totalHitPoints += hero.hitPoints;
        }
        
        return totalHitPoints <= 0;
        
    }

    public void CloseCombatSystem()
    {
        //clean up
        loot.Clear();
        modifiedEnemyNames.Clear();
        xpPool = 0;
        moneyPool = 0;
        bonusSystem.ResetBonuses();
        heroesInCombat.Clear();
        enemiesInCombat.Clear();
        turnOrder.Clear();

        gm = GameManager.instance;
        gm.SetCameraFollow(true);

        if (!playerRanAway)
            gm.SetState(GameManager.GameState.CombatEnded);
        else
            gm.SetState(GameManager.GameState.CombatEndedPlayerRanAway);

        //reset item text colour in inventory.
        Inventory inv = Inventory.instance;
        foreach(ItemSlot slot in inv.itemSlots)
        {
            if (slot.ItemInSlot() == null) return;
            TextMeshProUGUI currentSlot = slot.GetComponentInChildren<TextMeshProUGUI>();
            currentSlot.text = "<color=#ffffff>" + slot.ItemInSlot().itemName + " -- " + slot.quantity + "</color>";
        }
        //gameObject.SetActive(false);
    }

    public void UpdateTurnOrderUI()
    {
        ui.turnOrderList.text = "";
        int rank = 1;
        foreach(Avatar a in turnOrder)
        {
            string modName;
            if (rank == 1)
            {
                //if the current avatar is an enemy, update their name if it's a duplicate
                if (a.TryGetComponent(out Enemy enemy))
                {
                    if (modifiedEnemyNames.TryGetValue(enemiesInCombat.IndexOf(enemy), out modName))
                        ui.turnOrderList.text += rank + " <color=#0ffc7e>" + modName + "</color>\n";                 
                    else
                        ui.turnOrderList.text += rank + " <color=#0ffc7e>" + a.className + "</color>\n";
                }
                else
                    ui.turnOrderList.text += rank + " <color=#0ffc7e>" + a.className + "</color>\n";
            }
            else
            {
                //if the current avatar is an enemy, update their name if it's a duplicate
                if (a.TryGetComponent(out Enemy enemy))
                {
                    if (modifiedEnemyNames.TryGetValue(enemiesInCombat.IndexOf(enemy), out modName))
                        ui.turnOrderList.text += rank + " " + modName + "\n";
                    else
                        ui.turnOrderList.text += rank + " " + a.className + "\n";
                }
                else
                    ui.turnOrderList.text += rank + " " + a.className + "\n";
            }
            rank++;
        }
    }

    //relocates leading avatar to a new position in turn order. All other avatars below the current one get pushed back.
    public void ShuffleTurnOrder()
    {
        turnOrder = turnOrder.OrderByDescending(x => x.spd * x.spdMod).ToList();   //IMPORTANT: Lambda operations should not execute in update loop
        //UpdateTurnOrderUI(); 
    }

    public void CheckDuplicateNames()
    {
        for (int i = 0; i < enemiesInCombat.Count; i++)
        {
            int j = 0;
            int count = 0;
            while (j < i)
            {
                if (enemiesInCombat[i].GetType() == enemiesInCombat[j].GetType())   //cannot compare names because they may change
                {
                    count++;
                }
                j++;
            }

            //if we have more than 0 occurrences, update name
            if (count > 0)
            {
                modifiedEnemyNames.Add(i, enemiesInCombat[i].className += "-" + group.Substring(count - 1, 1));
                //enemiesInCombat[i].className += "-" + group.Substring(count - 1, 1);
                enemiesInCombat[i].className = modifiedEnemyNames[i];
                //turnOrder[turnOrder.IndexOf(enemiesInCombat[i])].className = enemiesInCombat[i].className;
            }

        }
    }

    //resets the battle state to default
    public void Reset()
    {

    }

    

    
}
