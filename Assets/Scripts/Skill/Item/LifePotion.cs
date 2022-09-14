using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Brings hero back from the dead with some HP. No effect if used on a living hero.
[CreateAssetMenu(menuName = "Skill/Item Skill/Life Potion", fileName = "skill_lifePotion")]
public class LifePotion : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        if (target.status == Avatar.Status.Dead)
        {
            target.status = Avatar.Status.Normal;

            //change target's sprite to normal
            if (target.TryGetComponent(out Hero hero))
            {
                SpriteRenderer sr = hero.GetComponent<SpriteRenderer>();
                sr.sprite = hero.data.sprite;
            }
                
            float healAmount = Mathf.Round(target.maxHitPoints * 0.33f);
            user.RestoreHitPoints(target, healAmount);

            //add back to the queue
            CombatSystem cs = CombatSystem.instance;
            cs.turnOrder.Add(target);
        }
        else
        {
            ui.DisplayStatusUpdate("NO EFFECT", user.transform.position);
        }
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        float healAmount = Mathf.Round(target.maxHitPoints * 0.33f);
        target.RestoreHitPoints(target, healAmount);
    }
}
