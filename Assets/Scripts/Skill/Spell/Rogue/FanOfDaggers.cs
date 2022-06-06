using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//damages all enemies with a physical attack. This attack never misses. Landing on crit panel deals critical damage.
[CreateAssetMenu(menuName = "Skill/Rogue/Fan Of Daggers", fileName = "skill_fanOfDaggers")]
public class FanOfDaggers : Skill
{
     public override void Activate(Avatar user, List<Enemy> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        float dmgMod = 1;
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;

        ReduceMp(user);

        if (cim.buttonPressed)
        {
            //cs.actGauge.actionToken.StopToken();
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    dmgMod = 1;
                    break;

                case ActionGauge.ActionValue.Reduced:
                    dmgMod = 0.5f;
                    break;

                case ActionGauge.ActionValue.Critical:
                    dmgMod = 1.5f;
                    ui.damageDisplay.color = ui.criticalDamageColor;
                    break;
            }

            Vector3[] targetPos = new Vector3[targets.Count];
            for (int i = 0; i < targets.Count; i++)
            {
                totalDamage = dmgMod > 1 ? Mathf.Round(user.atp * 2 * dmgMod) : Mathf.Round(user.atp * 2 * dmgMod - (targets[i].dfp * targets[i].dfpMod));
                totalDamage += Mathf.Round(Random.Range(0, totalDamage * 0.1f));

                targetPos[i] = targets[i].transform.position;
                user.ReduceHitPoints(targets, i, totalDamage);
            }

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
    }

}
