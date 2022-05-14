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
    Vector3 destination;
    int desinationNodeID;                       //used to update player nodeID after coroutine finishes.
    bool animateMoveCoroutineOn;

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
   void Start()
    {
        hasControl = true;

        //applying offset
        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

        //setup map sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        mapSprite = sr.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasControl)
        {
            if (!animateMoveCoroutineOn)
            {
                StartCoroutine(AnimateMovement());
            }
        }
    }

    //This method is to prevent captive heroes from being controlled by the player.
    public void SetCaptive(bool isCaptive)
    {
        hasControl = isCaptive;
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        mapSprite = sprite;
    }

    //This is used mainly for captive heroes
    public override void PlaceObject(int col, int row)
    {
        Dungeon dungeon = Dungeon.instance;
        Stairs exit = Stairs.instance;

        if (col < 0 || col >= dungeon.mapWidth) return;
        if (row < 0 || row >= dungeon.mapHeight) return;
        if (row == exit.row && col == exit.col) return;

        this.row = row;
        this.col = col;

        //find node
        foreach(Node node in dungeon.nodes)
        {
            if (!node.isOccupied && node.row == row && node.col == col)
            {
                transform.position = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                node.isOccupied = true;
                nodeID = node.nodeID;
                break;
            }
        }
    }

    public void MovePlayer(int colDestination, int rowDestination)
    {
        if (!hasControl) return;

        //can only move to an adjacent node that has a path.
        row = rowDestination;
        col = colDestination;

        Dungeon dungeon = Dungeon.instance;
        foreach(Node node in dungeon.nodes)
        {
            if (row == node.row && col == node.col)
            {
                dungeon.nodes[nodeID].isOccupied = false;

                //get node's position and begin moving player
                destination = new Vector3(node.transform.position.x, node.transform.position.y + yOffset, node.transform.position.z);
                //nodeID = node.nodeID;
                desinationNodeID = node.nodeID;
                node.isOccupied = true;
                hasControl = false;
                //transform.position = new Vector3(node.transform.position.x, node.transform.position.y + yOffset, 0);
                break;
            }
        }

        //update enemy movement
        foreach(MapEnemy enemy in dungeon.enemies)
        {   
            enemy.turnsBeforeMoving--;
            if (enemy.CanMove())
            {
                //move enemy to a random node that's close to them
                enemy.SearchForNearestNode();
                enemy.Move();

                //if enemy is now on a treasure chest or exit, they will guard it
                if (enemy.StandingOnObject())
                    enemy.isStationary = true;
            }
        }
        
    }

    IEnumerator AnimateMovement()
    {
        animateMoveCoroutineOn = true;
        Vector3 direction = destination - transform.position;
        float moveSpeed = 2;

        while (transform.position != destination)
        {
            float vx = moveSpeed * direction.x * Time.deltaTime;
            float vy = moveSpeed * direction.y * Time.deltaTime;

            transform.position = new Vector3(transform.position.x + vx, transform.position.y + vy, transform.position.z);

            //check if we're close to destination
            float diffX = Mathf.Abs(destination.x - transform.position.x);
            float diffY = Mathf.Abs(destination.y - transform.position.y);
            if (diffX >= 0 && diffX <= 0.05f && diffY >= 0 && diffY <= 0.05f)
                transform.position = destination;

            yield return null;
        }
        
        //after moving, update player's node ID so that appropriate actions will trigger.
        nodeID = desinationNodeID;
        animateMoveCoroutineOn = false;
        hasControl = true;
    }
}
