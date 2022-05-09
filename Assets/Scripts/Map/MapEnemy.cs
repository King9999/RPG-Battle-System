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
    int turnsBeforeMoving;                          //how many times the player moves before this enemy moves. If this value is 0, the enemy always moves when player does.
    public bool isStationary {get; set;}            //if true, enemy does not move.
    bool animateMoveCoroutineOn;
    Vector3 destination;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
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

    public void SetTurnsToMove(int count)
    {
        turnsBeforeMoving = count;
    }

    public void Move(Vector3 destination)
    {
        if (turnsBeforeMoving <= 0)
            if (!animateMoveCoroutineOn)
                StartCoroutine(AnimateMovement(destination));
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
    }
}
