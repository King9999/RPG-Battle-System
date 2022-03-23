using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Avatar/Hero", fileName = "hero_")]
public class HeroData : AvatarData
{
   public int level;        //can be used to track where hero should be in the xp table 
   public bool swordOK, daggerOK, axeOK, bowOK, staffOK;
   //add a XP table
}
