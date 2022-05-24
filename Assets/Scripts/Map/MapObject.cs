using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for all of the interactable dungeon objects. */
public abstract class MapObject : MonoBehaviour
{
    public new string name;
    public int col;
    public int row;
    public Sprite mapSprite;                //the sprite that is displayed on the screen. this sprite can change. Use this instead of SpriteRenderer.

    public bool occupiedByEnemy;            //if true, an enemy is standing on this object.
    public int nodeID;                      //the node ID the object is resting on.

    public virtual void PlaceObject(int col, int row){}
    public void ShowObject(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        mapSprite = sprite;
    }
}
