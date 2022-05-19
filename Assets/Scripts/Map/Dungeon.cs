using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/* A dungeon consists of nodes, paths, and interactable objects. Everything within a dungeon is randomized. 2D arrays are used for the map layout
    and the object placement. 
    
    0 - Empty
    1 - Node
    2 - Path (E-W)
    3 - Path (N-S)
    
    */

public class Dungeon : MonoBehaviour
{
    public List<Node> nodes;
    public Node nodePrefab;
    public Player playerPrefab;
    public Captive captivePrefab;
    public List<Captive> captiveHeroes;
    bool captivesGenerated;                 //prevents adding captive heroes to list more than once.
    public MapEnemy enemyPrefab;
    public Stairs stairsPrefab;
    public TreasureChest chestPrefab;
    public Stairs exit;
    public Player player;
    public List<MapEnemy> enemies;
    public List<MapEnemy> graveyard;    //defeated map enemies go in here
    public List<TreasureChest> chests;
    public CameraFollow cameraFollow;   //used to keep camera focused on player
    float heroAppearanceChance;        //the odds that a captive hero appears in a dungeon. Player should have at least 2 by the time they reach level 5.

    public bool[,] mapArray;
    public int mapWidth {get; set;}     //columns
    public int mapHeight {get; set;}    //rows
    int minMapSize {get;} = 4;          //applies to both width and height
    float xOffset, yOffset = 3;
    const float offset = 2;
    int nodeCount;
    int minNodeCount {get;} = 10;
    int totalNodes;                 //number of nodes in dungeon.
    byte nodeID;
    int dungeonLevel;

    public static Dungeon instance;

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
        mapWidth = minMapSize;
        mapHeight = minMapSize;
        mapArray = new bool[mapWidth, mapHeight];
        nodeCount = minNodeCount;
        heroAppearanceChance = 0;                   //this increases by 10% if no captive is generated, then resets to 0 after success.

        //get seed
        System.Random rnd = new System.Random();
        int p = rnd.Next();
        Random.InitState(p);        
        Debug.Log("Seed: " + p);
        

        File.WriteAllText(@"C:\_Projects\RPG Battle System\Logs\seeds.txt", p.ToString());

        //create the map
        GenerateDungeon(nodeCount);  
    }

    void Update()
    {
        GameManager gm = GameManager.instance;
        if (gm.gameState != GameManager.GameState.Combat)
        {
            //check if player is standng on an enemy, treasure chest, exit, or captive hero. Enemies are always checked first.
            /*foreach (MapEnemy enemy in enemies)
            {
                if (player.nodeID == enemy.nodeID)
                {
                    //start combat
                    gm.gameState = GameManager.GameState.Combat;
                    break;
                }
            }*/

            //treasure chest
            /*foreach(TreasureChest chest in chests)
            {
                if(chest.heldItem != null && player.nodeID == chest.nodeID)
                {
                    //take chest's contents
                }
            }*/
        }
        
    }

    //creates captive heroes for the player to rescue.
    void GenerateCaptives()
    {
        HeroManager hm = HeroManager.instance;
        Hero hero = hm.heroes[0];

        Captive captive;
        if (hero.heroClass == Hero.HeroClass.Barbarian)
        {
            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.rogueSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.mageSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.clericSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);
        }
        else if (hero.heroClass == Hero.HeroClass.Rogue)
        {
            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.mageSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.clericSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.barbSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);
        }
        else if (hero.heroClass == Hero.HeroClass.Mage)
        {
            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.clericSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.barbSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.rogueSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);
        }
        else if (hero.heroClass == Hero.HeroClass.Cleric)
        {
            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.barbSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.rogueSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);

            captive = Instantiate(captivePrefab);
            captive.SetSprite(captive.mageSprite);
            captive.transform.SetParent(transform);
            captiveHeroes.Add(captive);
        }

        //deactivate all captives until they're needed
        foreach(Captive captiveHero in captiveHeroes)
        {
            captiveHero.gameObject.SetActive(false);
        }   
    }

    public void GenerateDungeon(int nodeCount)
    {
        mapArray = new bool[mapWidth, mapHeight];
        this.nodeCount = nodeCount;

        //once map is generated, create rooms
        GenerateNode(this.nodeCount);

        //loop through array and create nodes. ANy existing nodes in the nodes list are re-used instead of instantiating new ones.
        bool firstNode = true;
        int currentNode = 0;
        bool nodeInstantiated = false;
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (mapArray[i, j] == true)
                {
                    Node node;
                    if (nodes.Count <= 0 || currentNode >= nodes.Count)
                    {
                        node = Instantiate(nodePrefab);
                        nodeInstantiated = true;
                    }
                    else
                    {
                        node = nodes[currentNode];
                    }
                    node.nodeID = nodeID++;
                    node.row = j;
                    node.col = i;
                    currentNode++;

                    //check adajacent spots in the array for other rooms. If this node is the first, then there can only be a path to the east and south.
                    if (firstNode)
                    {
                        while (!node.paths[node.eastPath].PathVisible() && !node.paths[node.southPath].PathVisible())
                        {
                            float pathChance = 0.5f;
                            bool eastPathVisible = mapArray[i + 1, j] == true && Random.value <= pathChance ? true : false;
                            bool southPathVisible = mapArray[i, j + 1] == true && Random.value <= pathChance ? true : false;
                            node.paths[node.eastPath].ShowPath(eastPathVisible);
                            node.paths[node.southPath].ShowPath(southPathVisible);
                        }

                        firstNode = false;
                    }
                    else
                    {
                        //check for any adjacent nodes
                        //check if there's a path leading to the node
                        bool northNode = (j - 1 >= 0 && mapArray[i, j - 1] == true) ? true : false;
                        bool southNode = (j + 1 < mapHeight && mapArray[i, j + 1] == true) ? true : false;
                        bool eastNode = (i + 1 < mapWidth && mapArray[i + 1, j] == true) ? true : false;
                        bool westNode = (i - 1 >= 0 && mapArray[i - 1, j] == true) ? true : false;

                        //if there are 2 or more adjacent rooms, see if all paths are opened.
                        float pathChance = 0.6f;
                        if (northNode)
                        {
                            node.paths[node.northPath].ShowPath(true);
                            if (southNode)
                                node.paths[node.southPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (eastNode)
                                node.paths[node.eastPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (westNode)
                                node.paths[node.westPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        else if (southNode)
                        {
                            node.paths[node.southPath].ShowPath(true);
                            if (northNode)
                                node.paths[node.northPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (eastNode)
                                node.paths[node.eastPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (westNode)
                                node.paths[node.westPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        else if (eastNode)
                        {
                            node.paths[node.eastPath].ShowPath(true);
                            if (northNode)
                                node.paths[node.northPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (southNode)
                                node.paths[node.southPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (westNode)
                                node.paths[node.westPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        else if (westNode)
                        {
                            node.paths[node.westPath].ShowPath(true);
                            if (northNode)
                                node.paths[node.northPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (southNode)
                                node.paths[node.southPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (eastNode)
                                node.paths[node.eastPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        
                    }

                    //place node in game
                    node.transform.position = new Vector3(transform.position.x + i, transform.position.y - j, 0);
                    node.transform.position *= offset;  //not quite sure why I have to apply offset this way

                    if (nodeInstantiated)
                    {
                        node.transform.SetParent(transform);
                        nodes.Add(node);
                    }
                    
                }
            }
        }

        //generate map objects, including player, enemies, chests, etc.
        GenerateMapObjects();

        //validate the dungeon, adding paths where necessary to reach the exit.
        ValidateDungeon();

        //if everything is good, we assign a level
        dungeonLevel++;
        DungeonUI ui = DungeonUI.instance;
        ui.dungeonLevelUI.text = "Level " + dungeonLevel + "F";
    }

    void ValidateDungeon()
    {
        //test the dungeon by creating a path from the start to the exit, checking each node for valid paths.
        //every time we test a node, we must record which nodes we've been to.
        //if there are no new nodes to traverse, then that means there's a missing path somewhere.
        //once this method is finished executing, the dungeon should be valid.

        if (nodes.Count <= 0) return;

        List<Node> previousNodes = new List<Node>();
        int row = 0;
        int col = 0;    //used to navigate the map array.
        int currentNode = 0;
        List<int> nodeIndexRecord = new List<int>();    //indexes of which nodes in the node list we've been to.
        bool exitFound = false;
        bool goingInCircles = false;
        int loopCount = 0;                              //tracks the number of times we go through the while loop. If it goes past 100, this dungeon is abandoned.
        while (!exitFound && loopCount <= 100)
        {
            loopCount++;
            if (row == 0 && col == 0)
            {
                currentNode = 0;
            }
            else
            {
                foreach(Node n in nodes)
                {
                    if (n.row == row && n.col == col)
                    {
                        currentNode = nodes.IndexOf(n);
                        break;
                    }
                }
            }
            Node node = nodes[currentNode];

            //check previous nodes list to see if this node exists
            foreach(Node n in previousNodes)
            {
                if (n.nodeID == node.nodeID)
                {
                    //we're going in circles, find out where a path needs to be added.
                    Debug.Log("going in circles");
                    goingInCircles = true;
                    break;
                }
            }

            if (!goingInCircles)
            { 
                previousNodes.Add(node);

                //We check for a path that leads to the exit node. Column must be checked first, then row.
                if (col < exit.col && node.paths[node.eastPath].PathVisible())
                {
                    col += 1;
                    currentNode = UpdateNode(currentNode, col, row);
                    node = nodes[currentNode];
                }
                else if (col > exit.col && node.paths[node.westPath].PathVisible())
                {
                    col -= 1;
                    currentNode = UpdateNode(currentNode, col, row);
                    node = nodes[currentNode];
                }

                if (row < exit.row && node.paths[node.southPath].PathVisible())
                {
                    row += 1;
                    currentNode = UpdateNode(currentNode, col, row);
                    node = nodes[currentNode];
                }
                else if (row > exit.row && node.paths[node.northPath].PathVisible())
                {
                    row -= 1;
                    currentNode = UpdateNode(currentNode, col, row);
                    node = nodes[currentNode];
                }

               
                
                //are we at the exit?
                if (row == exit.row && col == exit.col)
                {
                    exitFound = true;
                    Debug.Log("Found exit!");
                }
            }
            else
            {
                //There's a path missing. check previous nodes and look for a path to create
                foreach(Node n in previousNodes)
                {
                    //check for a node to the east
                    if (n.col + 1 < mapWidth && !n.paths[n.eastPath].PathVisible() && mapArray[n.col + 1, n.row] == true)
                    {
                        n.paths[n.eastPath].ShowPath(true);
                        row = n.row;
                        col = n.col + 1;
                        goingInCircles = false;
                        break;
                    }

                    //check west
                    else if (n.col - 1 >= 0 && !n.paths[n.westPath].PathVisible() && mapArray[n.col - 1, n.row] == true)
                    {
                        n.paths[n.westPath].ShowPath(true);
                        row = n.row;
                        col = n.col - 1;
                        goingInCircles = false;
                        break;
                    }

                    //check north
                    else if (n.row - 1 >= 0 && !n.paths[n.northPath].PathVisible() && mapArray[n.col, n.row - 1] == true)
                    {
                        n.paths[n.northPath].ShowPath(true);
                        row = n.row - 1;
                        col = n.col;
                        goingInCircles = false;
                        break;
                    }

                    //check south
                    else if (n.row + 1 < mapHeight && !n.paths[n.southPath].PathVisible() && mapArray[n.col, n.row + 1] == true)
                    {
                        n.paths[n.southPath].ShowPath(true);
                        row = n.row + 1;
                        col = n.col;
                        goingInCircles = false;
                        break;
                    }
                }
            }
        }

        Debug.Log("Loops " + loopCount);
        //if we get here, there was a problem with this dungeon and it must be abandoned.
        if (loopCount >= 100)
        {
            Debug.Log("Bad dungeon");
            //GenerateDungeon(nodeCount);
        }
    }

    //used with ValidateNode method
    private int UpdateNode(int nodeIndex, int playerCol, int playerRow)
    {
        foreach(Node n in nodes)
        {
            if (n.row == playerRow && n.col == playerCol)
            {
                nodeIndex = nodes.IndexOf(n);
                break;
            }
        }

        return nodeIndex;
    }

    void GenerateNode(int nodeCount)
    {
        if (nodeCount < minNodeCount) return;
        if (nodeCount > mapHeight * mapWidth) return;

        int i = 0;
        int j = 0;
        int totalNodes = 0;                 //tracks how many nodes have been generated, and will not exceed nodeCount
        int iPrevious = 0, jPrevious = 0;   //tracks which index I was in previously so I don't return there.

        //set index 0,0 to true and then have at least one adjacent index set to true
        mapArray[i,j] = true;
        totalNodes++;

        //there must be at least one path leading to the next room
        while(mapArray[i + 1, j] == false && mapArray[i, j + 1] == false)
        {
            mapArray[i + 1, j] = Random.value <= 0.5f ? true : false;  //east room
            mapArray[i, j + 1] = Random.value <= 0.5f ? true : false;  //south room
        }

        //find out where we're going next
        if (mapArray[i + 1, j] == true && mapArray[i, j + 1] == true)
        {
           totalNodes += 2;
           if (Random.value <= 0.5f)
           {
               iPrevious = i;
               i += 1;  //going east
           }
           else
           {
               jPrevious = j;
               j += 1;  //going south
           }

        }
        else if (mapArray[i + 1, j] == true)
        {
            iPrevious = i;
            i += 1;
            totalNodes++;
        }
        else if (mapArray[i, j + 1] == true)
        {
            jPrevious = j;
            j += 1;
            totalNodes++;
        }

        while(totalNodes < nodeCount)
        {
            //check surrounding indexes to see where we can move
            float directionChance = 0.8f;
            bool goingNorth = false;
            bool goingSouth = false;
            bool goingEast = false;
            bool goingWest = false;

            //TODO: Need to figure out how to get the minimum number of nodes
            while (!goingNorth && !goingSouth && !goingEast && !goingWest)
            {
                goingNorth = (j - 1 >= 0 && (i != iPrevious && j - 1 != jPrevious) && Random.value <= directionChance) ? true : false;
                goingSouth = (j + 1 < mapHeight && (i != iPrevious && j + 1 != jPrevious) && Random.value <= directionChance) ? true : false;
                goingEast = (i + 1 < mapWidth && (i + 1 != iPrevious && j != jPrevious) && Random.value <= directionChance) ? true : false;
                goingWest = (i - 1 >= 0  && (i - 1 != iPrevious && j != jPrevious) && Random.value <= directionChance) ? true : false;

                //if we get to the edge of the array, start over.
                if (i >= mapWidth - 1 && j >= mapHeight - 1)
                {
                    i = 0;
                    j = 0;
                }
                //Debug.Log("North: " + goingNorth + "\nSouth: " + goingSouth + "\nEast: " + goingEast + "\nWest: " + goingWest + "\n------\n");
            }

            if (goingNorth)
            {
                mapArray[i, j - 1] = true;
                totalNodes++;
            }
            if (goingSouth)
            {
                mapArray[i, j + 1] = true;
                totalNodes++;
            }
            if (goingEast)
            {
                mapArray[i + 1, j] = true;
                totalNodes++;
            }
            if (goingWest)
            {
                mapArray[i - 1, j] = true;
                totalNodes++;
            }

            //where do we move next?
            bool pathChosen = false;
            directionChance = 0.4f;
            while(pathChosen == false)
            {
                if (goingNorth && Random.value <= directionChance)
                {    
                    jPrevious = j;
                    j -= 1;
                    pathChosen = true;
                }
                else if (goingSouth && Random.value <= directionChance)
                {
                    jPrevious = j;
                    j += 1;
                    pathChosen = true;     
                }
                else if (goingEast && Random.value <= directionChance)
                {
                    iPrevious = i;
                    i += 1;
                    pathChosen = true;
                }
                else if (goingWest && Random.value <= directionChance)
                {
                    iPrevious = i;
                    i -= 1;
                    pathChosen = true;
                }
                directionChance += 0.2f;    //I do this to limit the time spent in this loop, and in case there's only one direction.
            }
        }
        
    }

    public void GenerateMapObjects()
    {
        /* When generating objects, I must take care to reuse existing objects instead of instantiating new ones whenever new dungeons are generated. */      
        
        enemies = new List<MapEnemy>();
        chests = new List<TreasureChest>();
        
        /****Player & Party setup****/
        DungeonUI ui = DungeonUI.instance;
        HeroManager hm = HeroManager.instance;
        if (player == null)
        {
            player = Instantiate(playerPrefab);
            player.SetSprite(player.barbSprite);        //TODO: this must change to whatever player picked at the beginning
            player.transform.SetParent(transform);
            cameraFollow.objectTransform = player.transform;
            
            ui.partyDisplay[0].hero = hm.heroes[0];
            ui.partyDisplay[0].SetSprite(player.mapSprite); //TODO: Why isn't this working correctly??
        }
        ui.partyDisplay[0].UpdateUI();
        player.PlaceObject(0, 0);
        
        
        /****create exit (stairs)****/
        //check if there's an existing object
        if (exit == null)
        {
            exit = Instantiate(stairsPrefab);
            exit.transform.SetParent(transform);
        }
        
        int randNode = Random.Range(nodes.Count - 3, nodes.Count);
        exit.PlaceObject(nodes[randNode].col, nodes[randNode].row);

        /****Create chests****/
        //It's possible for a dungeon to have no chests.
        int chestCount = Random.Range(0, nodes.Count / 4);
        for (int i = 0; i < chestCount; i++)
        {
            TreasureChest chest;
            bool chestInstantiated = false;
            if (i >= chests.Count)
            {
                chest = Instantiate(chestPrefab);
                chestInstantiated = true;
            }
            else
            {
                chest = chests[i];
            }

            //find a random node to occupy
            int randRow;
            int randCol;
            Node node = null;
            do
            {
                randRow = Random.Range(0, mapHeight);
                randCol = Random.Range(0, mapWidth);
                
                foreach(Node n in nodes)
                {
                    if (n.row == randRow && n.col == randCol)
                    {
                        node = n;
                        break;
                    }
                }
            }
            while (mapArray[randCol, randRow] == false || node.isOccupied);
            
            //generate item
            int tableLevel;
            if (dungeonLevel <= 5)
                tableLevel = 0;
            else if (dungeonLevel <= 10)
                tableLevel = 1;
            else
                tableLevel = 2;

            chest.GenerateLoot(tableLevel);
            chest.PlaceObject(randCol, randRow);
            
            if (chestInstantiated)
            {
                chest.transform.SetParent(transform);
                chests.Add(chest);
            }

        }

        /****Create and place captive heroes****/
        if (!captivesGenerated)
        {
            GenerateCaptives();

            //disable party UI sprites for the remaining missing heroes
            ui = DungeonUI.instance;
            for (int i = 1; i < ui.partyDisplay.Length; i++)
            {
                ui.partyDisplay[i].ShowSprite(false);
            }
        
            captivesGenerated = true;
        }

        if (captiveHeroes.Count > 0)
        {
            //check if a captive is placed in the dungeon. One is guaranteed if dungeon level is 5.
            //GameManager gm = GameManager.instance;
            heroAppearanceChance = 1;
            if (Random.value <= heroAppearanceChance || dungeonLevel == 5)
            {
                int randCaptive = Random.Range(0, captiveHeroes.Count);
                Captive captive = captiveHeroes[randCaptive];
                captive.gameObject.SetActive(true);
                int randRow;
                int randCol;
                Node node = null;

                do
                {
                    randRow = Random.Range(0, mapHeight);
                    randCol = Random.Range(0, mapWidth);
                    
                    foreach(Node n in nodes)
                    {
                        if (n.row == randRow && n.col == randCol)
                        {
                            node = n;
                            break;
                        }
                    }
                }
                while (mapArray[randCol, randRow] == false || node.isOccupied);
                captive.PlaceObject(randCol, randRow);
                heroAppearanceChance = 0;
            }
            else
            {
                //raise appearance chance for next time.
                heroAppearanceChance += 0.1f;
            }
        }

        /****create enemy. The number of enemies is (total nodes / 4)****/
        int enemyCount = nodes.Count / 4;
        int majorEnemyCount = enemyCount < 4 ? 0 : enemyCount / 4;
        bool forcedMajorEnemy = false;  
        for (int i = 0; i < enemyCount; i++)
        {
            MapEnemy enemy;
            bool enemyInstantiated = false;
            if (i >= enemies.Count)
            {
                enemy = Instantiate(enemyPrefab);
                enemyInstantiated = true;
            }
            else
            {
                enemy = enemies[i];
            }

            //is this a major enemy? note: a major enemy should always appear in level 5, and at the exit.
            //GameManager gm = GameManager.instance;
            //gm.dungeonLevel = 5;
            if (majorEnemyCount > 0 || (dungeonLevel == 5 && !forcedMajorEnemy))
            {
                if (Random.value <= 0.3f || (dungeonLevel == 5 && !forcedMajorEnemy))
                {
                    SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                    sr.sprite = enemy.majorEnemySprite;
                    //TODO: have a separate array in Enemy Manager for major enemies and pick one.

                    if (majorEnemyCount > 0) majorEnemyCount--;
                }
            }

            //find a random node to occupy
          
            if ((dungeonLevel == 5 && !forcedMajorEnemy))
            {
                //place a forced major enemy to introduce players to shield tokens.
                forcedMajorEnemy = true;
                enemy.PlaceObject(exit.col, exit.row);
            }
            else
            {
                int randRow;
                int randCol;
                Node node = null;

                do
                {
                    randRow = Random.Range(0, mapHeight);
                    randCol = Random.Range(0, mapWidth);
                    
                    foreach(Node n in nodes)
                    {
                        if (n.row == randRow && n.col == randCol)
                        {
                            node = n;
                            break;
                        }
                    }
                }
                while (mapArray[randCol, randRow] == false || node.isOccupied);
                enemy.PlaceObject(randCol, randRow);
            }

            //set turns. if the enemy is standing over a chest, stairs, or captive, they will not move.
            //exit check
            if (!exit.occupiedByEnemy && enemy.nodeID == exit.nodeID)
            {
                enemy.isStationary = true;
                //enemy becomes semi-transparent so player can see what's being guarded
                SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
                exit.occupiedByEnemy = true;
            }

            //chest check
            foreach(TreasureChest chest in chests)
            {
                if (!chest.occupiedByEnemy && enemy.nodeID == chest.nodeID)
                {
                    enemy.isStationary = true;
                    chest.occupiedByEnemy = true;
                    //enemy becomes semi-transparent so player can see what's being guarded
                    SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
                    break;
                }
            }

            //captive check
            foreach(Captive captive in captiveHeroes)
            {
                if (captive.gameObject.activeSelf && enemy.nodeID == captive.nodeID)
                {
                    enemy.isStationary = true;
                    captive.occupiedByEnemy = true;
                    //enemy becomes semi-transparent so player can see what's being guarded
                    SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
                    break;
                }
            }

            if (!enemy.isStationary)
            {
                int randTurn = Random.Range(enemy.defaultTurnCount, enemy.defaultTurnCount + 2);
                enemy.SetTurnCounter(randTurn);
            }

            if (enemyInstantiated)
            {
                enemy.transform.SetParent(transform);
                enemies.Add(enemy);
            }
        } 
        

    }

}
