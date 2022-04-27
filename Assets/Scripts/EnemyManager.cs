using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script contains every enemy in the game. Enemies are to be instantiated only in combat.
public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemies;
    public List<Enemy> graveyard;   //when enemies die, they go in here to be re-used instead of instantiating new enemies.

    [HideInInspector]public enum EnemyName {Imp, Wizard, Golem, WindDancer}

    //public EnemyName enemyName;

    public static EnemyManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.ResetData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnemy(Enemy enemy)
    {

    }
}
