using UnityEngine;

//Wizard primarily uses magic and will only use regular attacks if unable to cast.
public class Wizard : Enemy
{
    int fireballSkill = 0;
    int healSkill = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        skillProb = 0.35f;       //chance that wizard casts heal
        
    }

    public override void ExecuteLogic()
    {
        //if Wizard has no mana left, then attack.
        if (manaPoints >= skills[healSkill].manaCost || manaPoints >= skills[fireballSkill].manaCost)
        {
            //check if an ally needs healing, including self
            bool allyNeedsHealing = false;
            int i = 0;
            while (!allyNeedsHealing && i < cs.enemiesInCombat.Count)
            {   
                if (cs.enemiesInCombat[i].hitPoints <= cs.enemiesInCombat[i].maxHitPoints * 0.3f)
                {
                    allyNeedsHealing = true;
                }
                else
                {
                    i++;
                }
            }

            if (allyNeedsHealing)
            {
                //float roll = Random.Range(0, 1f);
                if (SkillActivated(skillProb))
                {
                    skills[healSkill].Activate(this, cs.enemiesInCombat[i], skillNameBorderColor);
                }
                else
                {
                    //cast fireball to a target
                    int randHero;
                    if (cs.heroesInCombat.Count > 1)
                    {
                        do
                            randHero = Random.Range(0, cs.heroesInCombat.Count);
                        while (cs.heroesInCombat[randHero].status == Avatar.Status.Hidden);

                        skills[fireballSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
                    }
                    else
                    {
                        skills[fireballSkill].Activate(this, cs.heroesInCombat[0], skillNameBorderColor);
                    }
                }
            }
            else
            {
                //cast fireball to a target
                int randHero;
                if (cs.heroesInCombat.Count > 1)
                {
                    do
                        randHero = Random.Range(0, cs.heroesInCombat.Count);
                    while (cs.heroesInCombat[randHero].status == Avatar.Status.Hidden);

                    skills[fireballSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
                }
                else
                {
                    skills[fireballSkill].Activate(this, cs.heroesInCombat[0], skillNameBorderColor);
                }
            }
        }
        else    //do regular attack
        {
            int randHero;
            if (cs.heroesInCombat.Count > 1)
            {
                do
                    randHero = Random.Range(0, cs.heroesInCombat.Count);
                while (cs.heroesInCombat[randHero].status == Avatar.Status.Hidden);

                Attack(cs.heroesInCombat[randHero]);
            }
            else
            {
                Attack(cs.heroesInCombat[0]);
            }
        }

        //end turn
        base.ExecuteLogic();
    }
}
