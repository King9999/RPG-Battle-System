using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by any enemy that wants to escape combat. The conditions will vary, but those conditions are taken care of in each enemy script
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Run", fileName = "skill_run")]
public class Run : Skill
{
    public override void Activate(Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        //Destroy(target.gameObject);     //TODO: Need to hide the object and place it in graveyard
        target.RunAway(); 
    }

}
