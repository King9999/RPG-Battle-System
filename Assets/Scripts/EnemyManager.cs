using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script contains every enemy in the game. Enemies are to be instantiated only in combat.
public class EnemyManager : MonoBehaviour
{
    Encounters encounterTable;
    public TextAsset tableFile;
    public Enemy[] enemies;
    public List<Enemy> graveyard;   //when enemies die, they go in here to be re-used instead of instantiating new enemies.
    int maxEnemies {get;} = 6;

    [HideInInspector]public enum EnemyName {Imp, Wizard, Golem, WindDancer}


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

    //Retrieve random enemy from encounter table.
    public List<Enemy> GetEnemies(int tableLevel)
    {
        encounterTable = JsonUtility.FromJson<Encounters>(tableFile.text);   //treat this like opening file stream except you don't have to close
        
        //search given table for the requested item
        if (tableLevel < 0 || tableLevel >= encounterTable.tables.Length) return null;
        List<Enemy> enemyEncounters = new List<Enemy>();

        //get all enmeies in the current table
        int enemyCount = encounterTable.tables[tableLevel].encounters.Length;

        //get a number of encounters that's proportional to the number of heroes in the party
        int randEnemies;
        HeroManager hm = HeroManager.instance;
        if (hm.heroes.Count <= 1)
            randEnemies = Random.Range(1, maxEnemies - 3);    //2 enemies total
        else if (hm.heroes.Count <= 2)
            randEnemies = Random.Range(1, maxEnemies - 2);    //3 enemies total
        else if (hm.heroes.Count <= 3)
            randEnemies = Random.Range(2, maxEnemies);        //5 enemies total
        else
            randEnemies = Random.Range(2, maxEnemies + 1);    //6 enemies total

        //check which enemies are encountered
        for (int i = 0; i < randEnemies; i++)
        {
            float roll = Random.value;
            for (int j = 0; j < enemyCount; j++)
            {
                if (roll <= encounterTable.tables[tableLevel].encounters[j].encounterChance)
                {
                    Debug.Log("Generated " + encounterTable.tables[tableLevel].encounters[j].objName);
                    int enemyID = encounterTable.tables[tableLevel].encounters[j].enemyID;
                    enemies[enemyID].enemyID = enemyID;
                    enemyEncounters.Add(enemies[enemyID]);
                    break;
                }
            }
            
        }

        return enemyEncounters;
    }
}
