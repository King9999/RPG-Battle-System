using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroesInCombat;
    public List<Enemy> enemiesInCombat;

    public List<Avatar> turnOrder;
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
        //heroes and enemies must be instantiated here
        heroesInCombat.Add(hm.heroes[0]);
        //set up heroes before adding enemies. The order is important.
        foreach (Hero hero in heroesInCombat)
        {
            Debug.Log("hero data " + hero.atp);
        }

        enemiesInCombat.Add(Instantiate(em.enemies[0]));
        enemiesInCombat[0].SetTurn(turnState: true);
        delay = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > currentTime + delay)
        {
            currentTime = Time.time;
            enemiesInCombat[0].SetTurn(turnState: true);
        }
        //combat system runs until either all enemies are defeated or the heroes are wiped out. In the unlikely scenario that both enemies
        //and heroes are defeated, game is over.
        while (combatEnabled)
        {

        }

        //if we get here, combat ended. Check which side won
    }
}
