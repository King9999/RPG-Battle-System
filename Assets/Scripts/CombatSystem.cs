using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroesInCombat;
    public List<Enemy> enemiesInCombat;

    public List<Avatar> turnOrder;
    public Transform[] enemyLocations;      //avatars are placed in these positions in battle.
    bool[] enemyLocationOccupied;
    public Transform[] heroLocations;
    bool[] heroLocationOccupied;
    bool combatEnabled;         //when true, combat starts and all combat elements apppear in foreground.
    
    public static CombatSystem instance;
    HeroManager hm;
    EnemyManager em;
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
        combatEnabled = false;
        //heroes and enemies must be instantiated here. We check graveyard before instantiating new enemies.
        heroesInCombat.Add(hm.heroes[0]);
        enemiesInCombat.Add(Instantiate(em.enemies[0]));
        
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
        }

        //enemiesInCombat.Add(Instantiate(em.enemies[0]));
        //enemiesInCombat[0].SetTurn(turnState: true);
        //delay = 2;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Time.time > currentTime + delay)
        {
            currentTime = Time.time;
            enemiesInCombat[0].SetTurn(turnState: true);
        }*/
        //combat system runs until either all enemies are defeated or the heroes are wiped out. In the unlikely scenario that both enemies
        //and heroes are defeated, game is over.
        while (combatEnabled)
        {

        }

        //if we get here, combat ended. Check which side won
    }
}
