using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the base class for playable heroes and NPC enemies */
public abstract class Avatar : MonoBehaviour
{
    public new string name;
    public string details;      //description of the enemy/hero
    public float hitPoints;
    public float manaPoints;
    public float atp;           //attack power
    public float dfp;           //defense power
    public float mag;           //magic power
    public float res;           //resistance

    public List<Skill> skills;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
