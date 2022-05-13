using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this must be attached to "dungeon HUD" object
public class DungeonUI : MonoBehaviour
{
    public Inventory inv;
    public DungeonMenu menu;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
