using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A hero is a player-controlled entity.
public class Hero : Avatar
{
    AvatarData data;

    // Start is called before the first frame update
    protected void Start()
    {
        //pull information from a scriptable object
        className = data.className;
        details = data.details;
        maxhitPoints = data.maxhitPoints;
        hitPoints = maxhitPoints;
        maxManaPoints = data.maxManaPoints;
        manaPoints = maxManaPoints;
        atp = data.atp;           
        dfp = data.dfp;           
        mag = data.mag;          
        res = data.res;
        skills = data.skills;
    }

    // Update is called once per frame
    void Update()
    {
        //user input
    }
}
