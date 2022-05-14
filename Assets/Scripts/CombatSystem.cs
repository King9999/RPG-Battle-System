using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroesInCombat;
    public List<Enemy> enemiesInCombat;
    public Dictionary<int, string> modifiedEnemyNames;  //modified names of duplicate enemies.
    int totalEnemies {get;} = 6;
    public int currentTarget {get; set;}    //index of an avatar being targeted.
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
    public Transform actGaugeLocation;

    //combat states. Used to determine which steps can be taken during combat
    public enum PlayerState {None, SelectingTargetToAttack, InventoryOpen, SkillOpen}
    public PlayerState playerState;

    public bool selectingTargetToAttack {get; set;}
    public bool selectingHero {get; set;}           //when true, player is using an item or a skill on a hero
    
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

    void Start()
    {
        hm = HeroManager.instance;
        em = EnemyManager.instance;
        gm = GameManager.instance;
        ui = UI.instance;
        modifiedEnemyNames = new Dictionary<int, string>();

        //action gauge setup
        actGauge.gameObject.SetActive(false);

        //actGauge = null;
        actGauge.transform.position = actGaugeLocation.position;

        loot = new Dictionary<Item, int>();

        //heroes and enemies must be instantiated here. We check graveyard before instantiating new enemies.
        foreach(Hero hero in hm.heroes)
        {
            heroesInCombat.Add(hero);
        }

        //add random enemies
        int randCount = Random.Range(1, totalEnemies + 1);
        for (int i = 0; i < 2; i++)
        {
            int randomEnemy = Random.Range(0, em.enemies.Length);
            //Enemy enemy = Instantiate(em.enemies[randomEnemy]);
            Enemy enemy = Instantiate(em.enemies[(int)EnemyManager.EnemyName.Imp]);
            enemiesInCombat.Add(enemy);
        }

        //check enemy names and append a letter to duplicates
        CheckDuplicateNames();
        /*for (int i = 0; i < enemiesInCombat.Count; i++)
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
                //Debug.Log(enemiesInCombat[i].className + " occurrences: " + count);
                enemiesInCombat[i].className += "-" + group.Substring(count - 1, 1);
            }

        }*/
        
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
            heroLocationOccupied[randIndex] = true;
            turnOrder.Add(hero);
            hero.UpdateStatsUI();
        }

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
            enemy.UpdateStatsUI();
        }

        //get turn order and set up UI.
        ShuffleTurnOrder();              
        //turnOrder = turnOrder.OrderByDescending(x => x.spd * x.spdMod).ToList();   //IMPORTANT: Lambda operations should not execute in update loop
        UpdateTurnOrderUI();
        currentTurn = 0;
        currentTarget = -1;
        currentHero = -1;

        //gm.gameState = GameManager.GameState.Combat;
    }

    public void SetupCombat()
    {
        gm.gameState = GameManager.GameState.Combat;
        
        //use this instead of Start() to determine the enemies encountered and to set up combat
        modifiedEnemyNames = new Dictionary<int, string>();

        //action gauge setup
        actGauge.gameObject.SetActive(false);

        actGauge.transform.position = actGaugeLocation.position;

        loot = new Dictionary<Item, int>();

        //heroes and enemies must be instantiated here. We check graveyard before instantiating new enemies.
        foreach(Hero hero in hm.heroes)
        {
            heroesInCombat.Add(hero);
        }

        //add random enemies
        int randCount = Random.Range(1, totalEnemies + 1);
        for (int i = 0; i < 2; i++)
        {
            int randomEnemy = Random.Range(0, em.enemies.Length);
            //Enemy enemy = Instantiate(em.enemies[randomEnemy]);
            Enemy enemy = Instantiate(em.enemies[(int)EnemyManager.EnemyName.Imp]);
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
            heroLocationOccupied[randIndex] = true;
            turnOrder.Add(hero);
            hero.UpdateStatsUI();
        }

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
            enemy.UpdateStatsUI();
        }

        //get turn order and set up UI.
        ShuffleTurnOrder();              
        //turnOrder = turnOrder.OrderByDescending(x => x.spd * x.spdMod).ToList();   //IMPORTANT: Lambda operations should not execute in update loop
        UpdateTurnOrderUI();
        currentTurn = 0;
        currentTarget = -1;
        currentHero = -1;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (combatEnded)
            return;
        /*if (AllHeroesDefeated())
        {
            gm.GameOver();
            return;
        }*/

        if (!combatEnded && enemiesInCombat.Count <= 0)
        {
            EndCombat();
            combatEnded = true;
            //return;
        }

        if (bonusSystem.bonusTurnsActive && bonusTurns <= 0)
        {
            bonusTurns = 0;
            //bonusSystem.bonusTurnsActive = false;
            bonusSystem.RemoveAllBonuses();
        }

        //check whose turn is next
        if (!turnInProgress)
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
                if (speedChanged)
                {
                    ShuffleTurnOrder();
                    speedChanged = false;
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
        for (int i = 0; i < heroesInCombat.Count; i++)
        {
            heroesInCombat[i].gameObject.SetActive(false);
            int xpShare = xpPool / heroesInCombat.Count;
    
            heroesInCombat[i].xpToNextLevel -= xpShare;
            if (heroesInCombat[i].xpToNextLevel <= 0)
            {
                //keep leveling up as long as the xp share exceeds to next level.
                int levelUpCount = 0;
                while (heroesInCombat[i].xpToNextLevel <= 0)
                {
                    heroesInCombat[i].LevelUp();
                    levelUpCount++;
                    heroesInCombat[i].xpToNextLevel -= xpShare;   
                }
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
       
        gameObject.SetActive(false);
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
