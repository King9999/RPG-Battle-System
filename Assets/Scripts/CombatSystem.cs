using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroesInCombat;
    public List<Enemy> enemiesInCombat;
    int totalEnemies {get;} = 6;
    public int currentTarget;               //index of an avatar being targeted.
    public int currentHero;                 //index of hero who is taking action
    string group = "ABCDE";                 //used to rename enemies if there are duplicates
    //public List<Item> loot;
    public Dictionary<Item, int> loot;
    public int xpPool;                      //total amount of XP from defeated enemies
    public int moneyPool;

    public ActionGauge actGauge;

    public List<Avatar> turnOrder;
    [HideInInspector]public int currentTurn;                        //iterator for turnOrder
    public Transform[] enemyLocations;      //avatars are placed in these positions in battle.
    bool[] enemyLocationOccupied;
    public Transform[] heroLocations;
    bool[] heroLocationOccupied;
    bool combatEnabled;         //when true, combat starts and all combat elements apppear in foreground.
    bool combatEnded;           //if true, will run the EndCombat method and prevent it from running more than once.
    public bool turnInProgress;
    public Transform actGaugeLocation;

    //combat states. Used to determine which steps can be taken during combat
    public enum CombatPhase {SelectTargetToAttack}

    public bool selectingTargetToAttack;
    
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
        //int count = 0;
        for (int i = 0; i < randCount; i++)
        {
            int randomEnemy = Random.Range(0, em.enemies.Length);
            Enemy enemy = Instantiate(em.enemies[randomEnemy]);
            enemiesInCombat.Add(enemy);
        }

        //check enemy names and append a letter to duplicates
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
                //Debug.Log(enemiesInCombat[i].className + " occurrences: " + count);
                enemiesInCombat[i].className += "-" + group.Substring(count - 1, 1);
            }

        }
        
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
        turnOrder = turnOrder.OrderByDescending(x => x.spd).ToList();   //IMPORTANT: Lambda operations should not execute in update loop
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

        //check whose turn is next
        if (!turnInProgress)
        {
            if (!turnOrder[currentTurn].TurnTaken())
            {
                //Debug.Log(turnOrder[currentTurn].className + "'s turn");
                turnOrder[currentTurn].TakeAction();
                //turnInProgress = true;
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
        float roll = Random.Range(0, 1f);

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


        //award XP, money and items
        string xpAndMoney = xpPool + " EXP\n" + moneyPool + " Money";
        string[] partyXp = new string[heroesInCombat.Count];

        //XP gain.
        for (int i = 0; i < heroesInCombat.Count; i++)
        {
            heroesInCombat[i].gameObject.SetActive(false);
            heroesInCombat[i].xpToNextLevel -= xpPool / heroesInCombat.Count;
            if (heroesInCombat[i].xpToNextLevel <= 0)
            {
                //Debug.Log(heroesInCombat[i].className + " leveled up!");
                heroesInCombat[i].LevelUp();
                partyXp[i] = heroesInCombat[i].className + " Level Up!\nEXP Gained: " + xpPool / heroesInCombat.Count + "\nTo Next Level " 
                + heroesInCombat[i].xpToNextLevel;
            }
            else
            {
                partyXp[i] = heroesInCombat[i].className + "\nEXP Gained: " + xpPool / heroesInCombat.Count + "\nTo Next Level " 
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
                ui.inventory.AddItem(item.Key, item.Value);
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
        xpPool = 0;
        moneyPool = 0;

       
        //hide heroes for next time
        /*foreach(Hero hero in heroesInCombat)
        {
            hero.gameObject.SetActive(false);
        }*/
        
        gameObject.SetActive(false);
    }

    public void UpdateTurnOrderUI()
    {
        ui.turnOrderList.text = "";
        int rank = 1;
        foreach(Avatar a in turnOrder)
        {
            if (rank == 1)
                ui.turnOrderList.text += rank + " <color=#0ffc7e>" + a.className + "</color>\n";
            else
                ui.turnOrderList.text += rank + " " + a.className + "\n";
            rank++;
        }
    }

    //resets the battle state to default
    public void Reset()
    {

    }

    

    
}
