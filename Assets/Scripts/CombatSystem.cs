using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public List<Hero> heroes;
    public List<Enemy> enemies;

    public List<Avatar> turnOrder;
    bool combatEnabled;         //when true, combat starts and all combat elements apppear in foreground.
    
    public static CombatSystem instance;

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
        heroes.Add(new Hero());
        enemies[0].SetTurn(turnState: true);
        foreach (Hero hero in heroes)
        {
            Debug.Log("hero data " + heroes[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
