using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Stairs allows the player to move to next level upon contact.*/
public class Stairs : MapObject
{
    public static Stairs instance;
    public Image advanceTimer;             //controls when player advances to new level

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        name = "Stairs";
        //advanceTimer.gameObject.SetActive(false);
        advanceTimer.fillAmount = 0;

        
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

        //set timer's position
        /*ClampMapImage clamp = GetComponent<ClampMapImage>();
        Vector3 timerPos = new Vector3(transform.position.x + 10, transform.position.y + 10, transform.position.z);
        advanceTimer.transform.position = timerPos;
        clamp.Clamp(advanceTimer.transform.position);*/
    }
    // Update is called once per frame
    void Update()
    {
        Player player = Player.instance;
        if (!occupiedByEnemy && nodeID == player.nodeID)
        {
            //player must remain on stairs for 1 second before advancing. This allows player
            //to move to another node without worrying about missing anything of interest.
            
            advanceTimer.fillAmount += Time.deltaTime;
            if (advanceTimer.fillAmount >= 1 && player.row == row && player.col == col) //if player moves off stairs last minute, don't advance.
            {
                //advance to next level
            }
            Debug.Log("Found stairs");
        }
        else
        {
            advanceTimer.fillAmount = 0;
        }
    }
}
