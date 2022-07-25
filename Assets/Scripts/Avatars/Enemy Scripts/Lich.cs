using UnityEngine;

/* Lich is a major enemy who wields powerful magic that consumes MP. If the player can survive the Lich's attacks, they will have
   an easier time defeating them when they have no MP. But the lich can steal MP once they're out. */
public class Lich : Enemy
{

    int fireStormSkill = 0;         //hits all heroes, used least often
    int iceBallSkill = 1;           //cold damage, hits 1 target
    int chainLightningSkill = 2;    //hits 1 target, then another target for less damage
    int poisonSkill = 3;            //chance to poison all heroes.
    int siphonSkill = 4;            //steals MP from target
    protected override void Start()
    {
        base.Start();
        canActWhileShieldBroken = true; //allows MP healing
    }

    public override void ExecuteLogic()
    {
        /* Lich casts random spells until MP is gone. Once that happens, the Lich will attempt to steal MP from the heroes. 
            If the Lich is guard crushed, MP is restored.*/
        
        if (status == Status.GuardBroken)
        {
            //restore 33% MP
            float manaHealAmount = Mathf.Round(maxManaPoints * 0.33f);
            RestoreManaPoints(this, manaHealAmount, 1);
        }
        else if (!CanCastMagic())
        {
            //steal MP from a hero
            AttackRandomHero(skills[siphonSkill]);
        }
        else
        {
            float rollValue = Random.value;
            if (rollValue <= 0.1f && manaPoints >= skills[fireStormSkill].manaCost) 
                skills[fireStormSkill].Activate(this, cs.heroesInCombat, skillNameBorderColor);
            else if (rollValue <= 0.2f && manaPoints >= skills[poisonSkill].manaCost) 
                skills[poisonSkill].Activate(this, cs.heroesInCombat, skillNameBorderColor);
            else if (rollValue <= 0.3f && manaPoints >= skills[chainLightningSkill].manaCost)
                AttackRandomHero(skills[chainLightningSkill]);
            else    //40% chance
                AttackRandomHero(skills[iceBallSkill]); 

        }

        //end turn
        base.ExecuteLogic();
    }

    bool CanCastMagic()
    {
        //if mana is lower than the lowest cost spell, then out of magic.
        return manaPoints >= skills[iceBallSkill].manaCost;   
    }
}
