using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Avatar/Hero", fileName = "hero_")]
public class HeroData : AvatarData
{
   public TextAsset statFile;    //contains stat table
   public Sprite sprite;
   public enum HeroClass {Barbarian, Rogue, Mage, Cleric}
   public HeroClass heroClass;
   public int level = 1;        //can be used to track where hero should be in the xp table
   public int MaxLevel {get;} = 50;
   public short attackTokenMod;    //adjusts how many attacks a hero gets. Can be negative, but minimum attack count is 1.
   [Header("Starting gear")]
    public Weapon weapon;
    public Armor armor;
    public Trinket trinket; 
   public bool swordOK, daggerOK, axeOK, bowOK, staffOK;
   //add a XP table

}
