using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for all of the interactable dungeon objects. */
public abstract class MapObject : MonoBehaviour
{
    public new string name;
    public int col {get; set;}
    public int row {get; set;}
    public Sprite mapSprite;                //the sprite that is displayed on the screen. this sprite can change.

    protected Dungeon dungeon;              //singleton
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        dungeon = Dungeon.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
