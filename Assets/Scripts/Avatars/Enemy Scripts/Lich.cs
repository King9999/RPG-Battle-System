using UnityEngine;

/* Lich is a major enemy who wields powerful magic that consumes MP. If the player can survive the Lich's attacks, they will have
   an easier time defeating them when they have no MP. But the lich can steal MP once they're out. */
public class Lich : Enemy
{
    // Start is called before the first frame update
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
        }
        else
        {
            if (SkillActivated(0.1f))
            {

            }
        }

        //end turn
        base.ExecuteLogic();
    }

    bool CanCastMagic()
    {
        //if mana is lower than the lowest cost spell, then out of magic.
        return false;
    }
}
