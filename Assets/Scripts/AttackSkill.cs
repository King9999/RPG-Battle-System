using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for all offensive skills in the game

[CreateAssetMenu(menuName = "Skill/Attack Skill", fileName = "Skill_")]
public class AttackSkill : Skill
{
   public float multiplier;     //power scale of a skill. 1 is the standard.
   public int hitCount;     //how many times does the attack hit per turn.
   public Element element;
   public EffectID effect;
}
