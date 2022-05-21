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
    public int defaultTurnCount {get;} = 2;
    int desinationNodeID;                       //used to update enemy nodeID after coroutine finishes.
    bool animateMoveCoroutineOn;
    Vector3 destination;
    float yOffset = -0.2f;
    public List<Enemy> encounters;              //the enemies the player will face in combat

    //combat against random enemies. The encountered enemies are determined from a table
    public void InitiateCombat()
    {
        CombatSystem cs = CombatSystem.instance;
        cs.SetupCombat(encounters);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            //combat check
            GameManager gm = GameManager.instance;
            if (gm.gameState <= GameManager.GameState.Normal)
            {
                Player player = Player.instance;
                if (nodeID == player.nodeID)
                {
                    //we must temporarily disable camera follow so combatants are displayed properly.
                    gm.SetCameraFollow(false);
                    CombatSystem cs = CombatSystem.instance;
                    cs.SetupCombat(encounters);
                }
            }

            if (gm.gameState == GameManager.GameState.CombatEnded)
            {
                gm.SetState(GameManager.GameState.Normal);
                SendToGraveyard();
            }
        }
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
        return !isStationary && turnsBeforeMoving <= 0;
    }

    public override void PlaceObject(int col, int row)
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
            if (!node.isOccupied && node.row == row && node.col == col)
            {
                transform.position = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                node.isOccupied = true;
                nodeID = node.nodeID;
                break;
            }
        }
    }

    //add specific enemies
    public void AddEncounter(Enemy enemy)
    {
        encounters.Add(enemy);
    }

    public void AddEncounter(int tableLevel)
    {
        //add a random enemy from encounter table
        EnemyManager em = EnemyManager.instance;
        encounters = em.GetEnemies(tableLevel);
    }

    //map enemies are sent to a graveyard so they can be reused.
    public void SendToGraveyard(bool ranAway = false)
    {
        Dungeon dungeon = Dungeon.instance;
        if (dungeon.enemies.Count <= 0) return;
      
        encounters.Clear();
        dungeon.graveyard.Add(this);
        dungeon.enemies.Remove(this);
        gameObject.SetActive(false);
    }

    //resets the sprite and encountered enemies.
    public void ResetEnemy()
    {

    }

    public void SearchForNearestNode()
    {
        //check surrounding indexes to see where we can move
        float directionChance = 0.6f;
        bool goingNorth = false;
        bool goingSouth = false;
        bool goingEast = false;
        bool goingWest = false;

        //TODO: Need to figure out how to get the minimum number of nodes
        Dungeon dungeon = Dungeon.instance;
        while (!goingNorth && !goingSouth && !goingEast && !goingWest)
        {
            goingNorth = (row - 1 >= 0 && dungeon.mapArray[col, row - 1] == true && Random.value <= directionChance) ? true : false;
            goingSouth = (row + 1 < dungeon.mapHeight && dungeon.mapArray[col, row + 1] == true && Random.value <= directionChance) ? true : false;
            goingEast = (col + 1 < dungeon.mapWidth && dungeon.mapArray[col + 1, row] == true && Random.value <= directionChance) ? true : false;
            goingWest = (col - 1 >= 0 && dungeon.mapArray[col - 1, row] == true && Random.value <= directionChance) ? true : false;
        }

        if (goingNorth)
        {
            dungeon.nodes[nodeID].isOccupied = false;
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row - 1 && node.col == col)
                {
                    row = node.row;
                    col = node.col;
                    node.isOccupied = true;
                    //nodeID = node.nodeID;
                    desinationNodeID = node.nodeID;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
        else if (goingSouth)
        {
            dungeon.nodes[nodeID].isOccupied = false;

            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row + 1 && node.col == col)
                {
                    row = node.row;
                    col = node.col;
                    node.isOccupied = true;
                    //nodeID = node.nodeID;
                    desinationNodeID = node.nodeID;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
        else if (goingEast)
        {
            dungeon.nodes[nodeID].isOccupied = false;

            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row && node.col == col + 1)
                {
                    row = node.row;
                    col = node.col;
                    node.isOccupied = true;
                    //nodeID = node.nodeID;
                    desinationNodeID = node.nodeID;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
        else if (goingWest)
        {
            dungeon.nodes[nodeID].isOccupied = false;
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row && node.col == col - 1)
                {
                    row = node.row;
                    col = node.col;
                    node.isOccupied = true;
                    //nodeID = node.nodeID;
                    desinationNodeID = node.nodeID;
                    destination = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                    break;
                }
            }
        }
    }

    public bool StandingOnObject()
    {
        bool standingOnChest = false;
        bool standingOnExit = false;
        bool standingOnCaptive = false;

        //check if enemy is on a chest or on the exit.
        Dungeon dungeon = Dungeon.instance;
        if (nodeID == dungeon.exit.nodeID && !dungeon.exit.occupiedByEnemy)
        {
            standingOnExit = true;
            dungeon.exit.occupiedByEnemy = true;

            //enemy becomes semi-transparent so player can see what's being guarded
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
        }

        //chest
        foreach(TreasureChest chest in dungeon.chests)
        {
            if (chest.heldItem != null && nodeID == chest.nodeID && !chest.occupiedByEnemy)
            {
                standingOnChest = true;
                chest.occupiedByEnemy = true;

                 //enemy becomes semi-transparent so player can see what's being guarded
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
                break;
            }
        }

        //captive
        foreach(Captive captive in dungeon.captiveHeroes)
        {
            if (captive.gameObject.activeSelf && nodeID == captive.nodeID)
            {
                standingOnCaptive = true;
                isStationary = true;
                captive.occupiedByEnemy = true;
                
                //enemy becomes semi-transparent so player can see what's being guarded
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
                break;
            }
        }

        return standingOnChest || standingOnExit || standingOnCaptive;
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

        //after moving, update enemy's node ID so that appropriate actions will trigger.
        nodeID = desinationNodeID;

        //if enemy is now on a treasure chest, exit, or captive, they will guard it
        if (StandingOnObject())
           isStationary = true;

        animateMoveCoroutineOn = false;
        ResetTurns();
    }
}
