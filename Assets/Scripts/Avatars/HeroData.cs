using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Avatar/Hero", fileName = "hero_")]
public class HeroData : AvatarData
{
   public int level = 1;        //can be used to track where hero should be in the xp table
   public int MaxLevel {get;} = 50;
   public int attackTokenMod;    //adjusts how many attacks a hero gets. Can be negative, but minimum attack count is 1. 
   public bool swordOK, daggerOK, axeOK, bowOK, staffOK;
   //add a XP table

}
