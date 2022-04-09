using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroesInCombat;
    public List<Enemy> enemiesInCombat;
    public List<Item> loot;
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
    public bool turnInProgress;
    public Transform actGaugeLocation;
    
    public static CombatSystem instance;
    HeroManager hm;
    EnemyManager em;
    GameManager gm;
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

        //action gauge setup
        actGauge.gameObject.SetActive(false);

        //actGauge = null;
        actGauge.transform.position = actGaugeLocation.position;


        //heroes and enemies must be instantiated here. We check graveyard before instantiating new enemies.
        heroesInCombat.Add(hm.heroes[0]);

        //add random enemies
        for (int i = 0; i < 2; i++)
        {
            int randomEnemy = Random.Range(0, em.enemies.Length);
            Enemy enemy = Instantiate(em.enemies[randomEnemy]);
            enemiesInCombat.Add(enemy);
        }

        //Enemy testEnemy = Instantiate(em.enemies[(int)EnemyManager.EnemyName.Wizard]);
        //enemiesInCombat.Add(testEnemy);
        //delay = 3;
        
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

        //get turn order.              
        turnOrder = turnOrder.OrderByDescending(x => x.spd).ToList();   //IMPORTANT: Lambda operations should not execute in update loop
        foreach(Avatar p in turnOrder)
            Debug.Log(p + "Speed: " + p.spd);
        currentTurn = 0;
        
    }

    
    // Update is called once per frame
    void Update()
    {
        /*if (Time.time > currentTime + delay)
        {
            currentTime = Time.time;
            enemiesInCombat[0].SetTurn(turnState: true);
        }*/

        /*if (AllHeroesDefeated())
        {
            gm.GameOver();
            return;
        }*/

        if (enemiesInCombat.Count <= 0)
        {
            EndCombat();
            return;
        }

        //check whose turn is next
        if (!turnInProgress)
        {
            if (!turnOrder[currentTurn].TurnTaken())
            {
                //turnOrder[currentTurn].SetTurn(turnState: true);
                Debug.Log(turnOrder[currentTurn].className + "'s turn");
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

                Debug.Log("New turn order");
                foreach(Avatar a in turnOrder)
                {
                    Debug.Log(a.className);
                }
            }
        }

        //if it's a hero's turn, display their weapon's action gauge

       
        //combat system runs until either all enemies are defeated or the heroes are wiped out. In the unlikely scenario that both enemies
        //and heroes are defeated, game is over.
        while (combatEnabled)
        {

        }

        //if we get here, combat ended. Check which side won

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
            loot.Add(enemy.rareItemDrop);
        }
        else if (enemy.commonItemDrop != null && roll <= enemy.commonItemDropChance)
        {
            loot.Add(enemy.commonItemDrop);
        }
        
    }

    void EndCombat()
    {
        //award XP, money and items
        Debug.Log("Battle over!");

        Debug.Log((xpPool / heroesInCombat.Count) + " XP awarded\n" + moneyPool + " money awarded");
        foreach(Item item in loot)
        {
            Debug.Log("Obtained " + item.itemName);
            //TODO: add this item to player inventory once that's set up
        }

        foreach(Hero hero in heroesInCombat)
        {
            hero.currentXp += xpPool / heroesInCombat.Count;
            //add money to inventory
        }

        //clean up
        loot.Clear();
        xpPool = 0;
        moneyPool = 0;

        gameObject.SetActive(false);
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

    //resets the battle state to default
    public void Reset()
    {

    }

    
}
