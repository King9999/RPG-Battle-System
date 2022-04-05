using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CombatSystem combatSystem;
    // Start is called before the first frame update
    void Start()
    {
        combatSystem = CombatSystem.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
