using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this must be attached to "dungeon HUD" object
public class DungeonUI : MonoBehaviour
{
    public Inventory inv;
    public DungeonMenu menu;
    public PartyStats partyDisplay;             //handles party UI in dungeon screen.


    public static DungeonUI instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

}
