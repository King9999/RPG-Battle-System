using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Map enemies occupy nodes in a dungeon. They will move around a map after a specified number of turns, or they may always be stationary
(to protect something, for example). When an enemy is touched, combat is initiated. The enemies encountered can be random or they can be fixed. */
public class MapEnemy : MapObject
{
    public Sprite minorEnemySprite;
    public Sprite majorEnemySprite;
    public Sprite bossSprite;
    public int turnsBeforeMoving;                   //how many times the player moves before this enemy moves. If this value is 0, the enemy always moves when player does.
    public int turnCounter;                         //the maximum number of turns before enemy moves.
    public bool isStationary {get; set;}            //if true, enemy does not move.
    bool animateMoveCoroutineOn;
    public Vector3 destination;
    float yOffset = -0.2f;

    // Start is called before the first frame update
    void Start()
    {
       
        turnsBeforeMoving = turnCounter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //combat against random enemies. The encountered enemies are determined from a table
    public void InitiateCombat()
    {
        CombatSystem cs = CombatSystem.instance;
        //check which enemies can be generated from a table.
        //get a random number of enemies, proportional to the number of heroes.
    }

    //used to battle specific enemies
    public void InitiateCombat(Enemy[] enemies)
    {
        //pick an enemy from Enemy Manager
        CombatSystem cs = CombatSystem.instance;
    }

    public void ResetTurns()
    {
        turnsBeforeMoving = turnCounter;
    }

    public void SetTurnCounter(int counter)
    {
        turnCounter = counter;
        turnsBeforeMoving = turnCounter;
    }

    public void Move()
    {   
        if (!animateMoveCoroutineOn)
            StartCoroutine(AnimateMovement(destination));     
    }

    public bool CanMove()
    {
        return turnsBeforeMoving <= 0;
    }

    public void PlaceEnemy(int col, int row)
    {
        Dungeon dungeon = Dungeon.instance;
        Player player = Player.instance;

        if (col < 0 || col >= dungeon.mapWidth) return;
        if (row < 0 || row >= dungeon.mapHeight) return;
        if (row == player.row && col == player.col) return;

        this.row = row;
        this.col = col;

        //find node TODO: make sure an enemy isn't occupied in the same space
        
        foreach(Node node in dungeon.nodes)
        {
            if (node.row == row && node.col == col)
            {
                transform.position = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                break;
            }
        }
    }

    public void SearchForNearestNode()
    {
        //check surrounding indexes to see where we can move
        float directionChance = 0.8f;
        bool goingNorth = false;
        bool goingSouth = false;
        bool goingEast = false;
        bool goingWest = false;

        //TODO: Need to figure out how to get the minimum number of nodes
        Dungeon dungeon = Dungeon.instance;
        while (!goingNorth && !goingSouth && !goingEast && !goingWest)
        {
            goingNorth = (row - 1 >= 0 && dungeon.mapArray[col, row-1] == true && Random.value <= directionChance) ? true : false;
            goingSouth = (row + 1 < dungeon.mapHeight && dungeon.mapArray[col, row+1] == true && Random.value <= directionChance) ? true : false;
            goingEast = (col + 1 < dungeon.mapWidth && dungeon.mapArray[col+1, row] == true && Random.value <= directionChance) ? true : false;
            goingWest = (col - 1 >= 0  && dungeon.mapArray[col-1, row] == true && Random.value <= directionChance) ? true : false;
        }

        if (goingNorth)
        {
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row - 1 && node.col == col)
                {
                    row = node.row;
                    col = node.col;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
        else if (goingSouth)
        {
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row + 1 && node.col == col)
                {
                    row = node.row;
                    col = node.col;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
        else if (goingEast)
        {
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row && node.col == col + 1)
                {
                    row = node.row;
                    col = node.col;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
        else if (goingWest)
        {
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row && node.col == col - 1)
                {
                    row = node.row;
                    col = node.col;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
    }

    IEnumerator AnimateMovement(Vector3 destination)
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
       
        animateMoveCoroutineOn = false;
        ResetTurns();
    }
}
