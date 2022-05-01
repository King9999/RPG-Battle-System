using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for all of the interactable dungeon objects. */
public abstract class MapObject : MonoBehaviour
{
    public new string name;
    protected int currentCol, currentRow;
    public Sprite mapSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
