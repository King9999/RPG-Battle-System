using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These are NPCs that the player can pick up to gain more party members. Captives do not move.
public class Captive : MapObject
{
    public Sprite barbSprite;
    public Sprite rogueSprite;
    public Sprite mageSprite;
    public Sprite clericSprite;
    float yOffset = 0.2f;
    float activeTime;               //used to prevent Update loop from executing immediately when a captive is instantiated, resulting in incorrect behaviour.
    float currentTime;

    void Start()
    {
        activeTime = 2;
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is standing on this object
        Player player = Player.instance;
        if (!occupiedByEnemy && nodeID == player.nodeID)
        {
            HeroManager hm = HeroManager.instance;

            if (mapSprite == barbSprite)
            {
                hm.AddHero(hm.heroData[hm.barbData]);
            }
            else if (mapSprite == rogueSprite)
            {
                hm.AddHero(hm.heroData[hm.rogueData]);
            }
            else if (mapSprite == mageSprite)
            {
                hm.AddHero(hm.heroData[hm.mageData]);
                //create a staff with a random skill
                //Weapon staff = (Weapon)ScriptableObject.CreateInstance("Weapon");
                ItemManager im = ItemManager.instance;
                Weapon staff = Instantiate(im.weapons[(int)ItemManager.WeaponItem.Staff]);
                staff.GenerateSkill();
                staff.Equip(hm.heroes[hm.heroes.Count - 1]);
            }
            else if (mapSprite == clericSprite)
            {
                hm.AddHero(hm.heroData[hm.clericData]);
            }

            //Update party UI
            Dungeon dungeon = Dungeon.instance;
            DungeonUI ui = DungeonUI.instance;
            
            int currentHero = hm.heroes.Count - 1;
            ui.partyDisplay[currentHero].hero = hm.heroes[currentHero];
            ui.partyDisplay[currentHero].SetSprite(mapSprite);
            ui.partyDisplay[currentHero].heroID = currentHero;
            ui.partyDisplay[currentHero].UpdateUI();
            ui.notification.DisplayMessage(name + " added to party!");

            dungeon.captiveHeroes.Remove(this);
            Destroy(gameObject);
        }
      
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
                transform.position = new Vector3(node.transform.position.x, node.transform.position.y + yOffset, node.transform.position.z);
                node.isOccupied = true;
                nodeID = node.nodeID;
                break;
            }
        }
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        mapSprite = sprite;

        //set the name
        if (mapSprite == barbSprite)
            name = "Barbarian";
        else if (mapSprite == rogueSprite)
            name = "Rogue";
        else if (mapSprite == mageSprite)
            name = "Mage";
        else if (mapSprite == clericSprite)
            name = "Cleric";
    }
}
