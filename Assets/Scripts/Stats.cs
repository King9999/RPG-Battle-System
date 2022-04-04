 [System.Serializable]
 public class Stat
 {
     public int level;
     public float hp;
     public float mp;
     public float atp;
     public float dfp;
     public float spd;
     public float mag;
     public float res;
     public int xpToNextLevel;
 }



[System.Serializable]
public class Stats
{
   
    public Stat[] tableStats;
}
