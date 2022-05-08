using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script handles the player actions on the dungeon map. The player must click on a node adjacent to the avatar's current position
    to move to that node. The player sprite will be based on who they pick as their first hero. */
public class Player : MapObject
{
    //public int currentCol, currentRow;             //position in map array
    bool hasControl;                                //when false, no input is accepted.
    float yOffset = 0.2f;                       //used to position player object so they aren't sticking outside of the node.

    //map sprites
    public Sprite barbSprite;
    public Sprite rogueSprite;
    public Sprite wizardSprite;
    public Sprite clericSprite;

    public static Player instance;

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
    protected override void Start()
    {
        base.Start();
        hasControl = true;

        //player always begins at the first node
        //transform.position = new Vector3(dungeon.nodes[0].transform.position.x, dungeon.nodes[0].transform.position.y + yOffset, 0);
        //MovePlayer(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(int rowDestination, int colDestination)
    {
        //can only move to an adjacent node that has a path.
        row = rowDestination;
        col = colDestination;

        Dungeon dungeon = Dungeon.instance;
        foreach(Node node in dungeon.nodes)
        {
            if (row == node.row && col == node.col)
            {
                transform.position = new Vector3(node.transform.position.x, node.transform.position.y + yOffset, 0);
                //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                break;
            }
        }
        
    }

    IEnumerator AnimateMovement()
    {
        yield return null;
    }
}
