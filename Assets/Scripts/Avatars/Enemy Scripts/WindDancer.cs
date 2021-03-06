using UnityEngine;

/* Wind Dancer is a speedy enemy that can also boost the speed of its allies, or reduce the heroes' speed */
public class WindDancer : Enemy
{
    int coolWindSkill = 0;
    int heavyWindSkill = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        skillProb = 0.5f;       //chance of executing cool wind
    }

    public override void ExecuteLogic()
    {
        float roll = Random.Range(0, 1f);
        Debug.Log("Roll " + roll);
        if (roll <= 0.1f)
        {
            int randTarget;
            if (cs.heroesInCombat.Count > 1)
            {
                do
                    randTarget = Random.Range(0, cs.heroesInCombat.Count);
                while (cs.heroesInCombat[randTarget].status == Avatar.Status.Hidden);

                Attack(cs.heroesInCombat[randTarget]);
            }
            else
            {
                Attack(cs.heroesInCombat[0]);
            }
            
        }
        else if (roll <= 0.4f)
        {
            //use heavy wind skill
            int randTarget;
            if (cs.heroesInCombat.Count > 1)
            {
                do
                    randTarget = Random.Range(0, cs.heroesInCombat.Count);
                while (cs.heroesInCombat[randTarget].status == Avatar.Status.Hidden);
                skills[heavyWindSkill].Activate(cs.heroesInCombat[randTarget], skillNameBorderColor);
            }
            else
            {
                skills[heavyWindSkill].Activate(cs.heroesInCombat[0], skillNameBorderColor);
            }
        }
        else    //casting cool wind
        {    
            int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
            skills[coolWindSkill].Activate(cs.enemiesInCombat[randTarget], skillNameBorderColor);
        }
        base.ExecuteLogic();
    }
}
