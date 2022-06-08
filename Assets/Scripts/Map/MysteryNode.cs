using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Mystery nodes will trigger random events when the player lands on one. */
public class MysteryNode : MapObject
{
    public Image timer;             //used to prevent node from activating immediately in case player wants to avoid.

    //NOTE: "End" is there to mark the end of the enums and is used to get the total number
    public enum NodeEffects {None, ReduceXpToOne, AddMoreEnemies, RestockChests, RestoreAllHp, ReduceAllMp, GlobalStatBuff, End}

    void Start()
    {
        name = "Mystery Node";
        timer.fillAmount = 0;      
    }

    // Update is called once per frame
    void Update()
    {
        Player player = Player.instance;
        if (!occupiedByEnemy && nodeID == player.nodeID)
        {
            //player must remain on mystery node for 1 second before it's activated
            timer.fillAmount += Time.deltaTime;

            if (timer.fillAmount >= 1 && player.row == row && player.col == col)
            {
                Debug.Log("Mystery Node activated");

                //get random effect. Put code here
                int randEffect = Random.Range(0, (int)NodeEffects.End);
                //GetRandomEffect((NodeEffects)randEffect);
                GetRandomEffect(NodeEffects.AddMoreEnemies);

                ShowObject(false);
            }  
        }
        else
        {
            timer.fillAmount = 0;
        }
    }

    public override void PlaceObject(int col, int row)
    {
        Dungeon dungeon = Dungeon.instance;
        Player player = Player.instance;
        Stairs exit = Stairs.instance;

        if (col < 0 || col >= dungeon.mapWidth) return;
        if (row < 0 || row >= dungeon.mapHeight) return;
        if (row == player.row && col == player.col) return;
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

    private void GetRandomEffect(NodeEffects effect)
    {
        DungeonUI ui = DungeonUI.instance;
        HeroManager hm = HeroManager.instance;

        switch(effect)
        {
            case NodeEffects.None:
                ui.notification.DisplayMessage("Nothing happened");
                break;

            case NodeEffects.ReduceXpToOne:
                for(int i = 0; i < hm.heroes.Count; i++)
                {
                    hm.heroes[i].xpToNextLevel = 1;
                    ui.partyDisplay[i].UpdateUI();
                    ui.DisplayStatus(i, "XP BONUS", ui.partyDisplay[i].transform.position, Color.white);
                }
                ui.notification.DisplayMessage("Time to level up");
                break;

            case NodeEffects.AddMoreEnemies:
                //add a random number of enemies, with a chance of adding a major enemy. This effect only occurs after the 5th level.
                Dungeon dungeon = Dungeon.instance;
                if (dungeon.dungeonLevel > 5)
                {
                    int enemyCount = Random.Range(1, (dungeon.nodes.Count / 4) + 1);

                    if (Random.value <= 0.5f)
                        dungeon.GenerateEnemies(enemyCount, generateMajorEnemy: true);
                    else
                        dungeon.GenerateEnemies(enemyCount);
                
                    ui.notification.DisplayMessage("Here come reinforcements");
                }
                else
                    ui.notification.DisplayMessage("No reinforcements...");  
                break;

            case NodeEffects.RestockChests:
                //any open chests are restocked. The items are from a loot table 1 level higher than current.
                //chests have to be enabled for this effect to work.
                ui.notification.DisplayMessage("Restocked");
                break;

            case NodeEffects.RestoreAllHp:
                for(int i = 0; i < hm.heroes.Count; i++)
                {
                    hm.heroes[i].hitPoints = Mathf.Round(hm.heroes[i].maxHitPoints * hm.heroes[i].hpMod);
                    ui.partyDisplay[i].UpdateUI();
                    ui.DisplayStatus(i, hm.heroes[i].hitPoints.ToString(), ui.partyDisplay[i].heroSprite.transform.position, ui.healColor);
                }
                ui.notification.DisplayMessage("A rejuvenating breeze");
                break;

            case NodeEffects.ReduceAllMp:
                for(int i = 0; i < hm.heroes.Count; i++)
                {
                    hm.heroes[i].manaPoints = Mathf.Round(hm.heroes[i].manaPoints * 0.5f);
                    ui.partyDisplay[i].UpdateUI();
                    ui.DisplayStatus(i, hm.heroes[i].manaPoints.ToString(), ui.partyDisplay[i].heroSprite.transform.position, Color.grey);
                }
                ui.notification.DisplayMessage("You feel a bit dizzy");
                break;

            case NodeEffects.GlobalStatBuff:
                //a random stat gets a buff
                int randStat = Random.Range(1, 8);
                if (randStat == 1)
                {
                    ui.notification.DisplayMessage("You feel a little stronger");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minAtpMod += 0.02f;
                        hm.heroes[i].atpMod = hm.heroes[i].minAtpMod;
                        hm.heroes[i].UpdateStats();
                        ui.DisplayStatus(i, "ATP BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }

                else if (randStat == 2)
                {
                    ui.notification.DisplayMessage("You feel more durable");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minDfpMod += 0.02f;
                        hm.heroes[i].dfpMod = hm.heroes[i].minDfpMod;
                        hm.heroes[i].UpdateStats();
                        ui.DisplayStatus(i, "DFP BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }

                else if (randStat == 3)
                {
                    ui.notification.DisplayMessage("You feel a little brighter");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minMagMod += 0.02f;
                        hm.heroes[i].magMod = hm.heroes[i].minMagMod;
                        hm.heroes[i].UpdateStats();
                        ui.DisplayStatus(i, "MAG BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }

                else if (randStat == 4)
                {
                    ui.notification.DisplayMessage("You feel a little wiser");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minResMod += 0.02f;
                        hm.heroes[i].resMod = hm.heroes[i].minResMod;
                        hm.heroes[i].UpdateStats();
                        ui.DisplayStatus(i, "RES BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }

                else if (randStat == 5)
                {
                    ui.notification.DisplayMessage("You feel a little nimbler");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minSpdMod += 0.02f;
                        hm.heroes[i].spdMod = hm.heroes[i].minSpdMod;
                        hm.heroes[i].UpdateStats();
                        ui.DisplayStatus(i, "SPD BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }

                else if (randStat == 6)
                {
                    ui.notification.DisplayMessage("You feel a little tougher");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minHpMod += 0.02f;
                        hm.heroes[i].hpMod = hm.heroes[i].minHpMod;
                        hm.heroes[i].UpdateStats();
                        ui.partyDisplay[i].UpdateUI();
                        ui.DisplayStatus(i, "HP BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }

                else if (randStat == 7)
                {
                    ui.notification.DisplayMessage("You feel more knowledgable");
                    for (int i = 0; i < hm.heroes.Count; i++)
                    {
                        hm.heroes[i].minMpMod += 0.02f;
                        hm.heroes[i].mpMod = hm.heroes[i].minMpMod;
                        hm.heroes[i].UpdateStats();
                        ui.partyDisplay[i].UpdateUI();
                        ui.DisplayStatus(i, "MP BOOST", ui.partyDisplay[i].transform.position, Color.white);
                    }
                }
                                            
                break;
        }
    }
}
