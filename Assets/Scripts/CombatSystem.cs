using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combat occurs in the same scene. It's just overlaid over the game map, and disappears when combat ends.
public class CombatSystem : MonoBehaviour
{
    public Hero[] heroes;
    public Enemy[] enemies;

    public Avatar[] turnOrder;
    bool combatEnabled;         //when true, combat starts and all combat elements apppear in foreground.
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
