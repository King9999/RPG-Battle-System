
[System.Serializable]
 public class Encounter
 {
    public int tableLevel;
    public EnemyEncounter[] encounters;
 }

[System.Serializable]
 public class EnemyEncounter
 {
     public int enemyID;                //corresponds to array position in Enemy Manager
     public string objName;             //name of file/object, NOT the enemy class name.
     public float encounterChance;
 }



[System.Serializable]
public class Encounters
{ 
    public Encounter[] tables;
}