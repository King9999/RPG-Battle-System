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
    public MysteryNode mysteryPrefab;
    public Stairs exit;
    public Player player;
    public List<MapEnemy> enemies;
    public List<MapEnemy> graveyard;    //defeated map enemies go in here
    public List<TreasureChest> chests;
    public int chestCount {get; set;} //used by mystery node map object to check if chests can be restocked.
    public List<MysteryNode> mysteryNodes;
    public CameraFollow cameraFollow;   //used to keep camera focused on player
    [SerializeField]float heroAppearanceChance;        //the odds that a captive hero appears in a dungeon. Player should have at least 2 heroes by the time they reach level 5.

    public bool[,] mapArray;
    public int mapWidth {get; set;}     //columns
    public int mapHeight {get; set;}    //rows
    int minMapSize {get;} = 4;          //applies to both width and height
    float xOffset, yOffset = 3;
    const float offset = 2;
    public int nodeCount {get; set;}
    public int minNodeCount {get;} = 10;
    int totalNodes;                 //number of nodes in dungeon.
    int nodeID;
    public int dungeonLevel {get; set;}

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
        /*System.Random rnd = new System.Random();
        int p = rnd.Next(); 
        Random.InitState(p);     //seed 1982010089 is for testing   
        Debug.Log("Seed: " + p);
        

        File.WriteAllText(@"C:\_Projects\RPG Battle System\Logs\seeds.txt", p.ToString());*/

        //create the map
        //GenerateDungeon(nodeCount);  
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
            captiveHero.ShowObject(false);
        }   
    }

    //if updateDungeonLevel is false, that means player could not complete dungeon (e.g. inaccessible stairs) and must reset it.
    public void GenerateDungeon(int nodeCount, bool updateDungeonLevel = true)
    {
        GameManager gm = GameManager.instance;
        dungeonLevel = updateDungeonLevel == true ? dungeonLevel + 1 : dungeonLevel;
        //dungeonLevel = 5;
        DungeonUI ui = DungeonUI.instance;
        ui.dungeonLevelUI.text = "Level " + dungeonLevel + "F";

        /***the size of the map increases in the following ways:
        -map width increases by 1 every 5 levels
        -map height increases by 1 every 10 levels 
        -node count increases by 1 every 2 levels. */
        if (updateDungeonLevel)
        {
            mapWidth = dungeonLevel % 5 == 0 ? mapWidth + 1 : mapWidth;
            mapHeight = dungeonLevel % 10 == 0 ? mapHeight + 1 : mapHeight;
            gm.nodeCount = dungeonLevel % 2 == 0 ? gm.nodeCount + 1 : gm.nodeCount; //updating node count for next time a dungeon is generated.
        }

        Debug.Log("Map Width: " + mapWidth + " Map Height: " + mapHeight + " Node Count: " + gm.nodeCount);
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
                        node.nodeID = nodeID++;
                    }
                    else
                    {
                        node = nodes[currentNode];
                        node.ResetNode();
                    }
                    
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
        if (!updateDungeonLevel)
            GenerateMapObjects(dungeonLevelReset: true);    //hero appearance chance is reset to 0
        else
            GenerateMapObjects();

        //validate the dungeon, adding paths where necessary to reach the exit.
        ValidateDungeon();
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
            Debug.Log("Bad dungeon. Seed for this dungeon is " + GameManager.instance.seed);
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

    public void GenerateMapObjects(bool dungeonLevelReset = false)
    {
        /* When generating objects, I must take care to reuse existing objects instead of instantiating new ones whenever new dungeons are generated. */      
        enemies = new List<MapEnemy>();

        //if (chests.Count <= 0 || chests == null)
            //chests = new List<TreasureChest>();     //if there were any existing chests, they will be re-used so we don't want to clear the list.
        
        /****Player & Party setup****/
        DungeonUI ui = DungeonUI.instance;
        HeroManager hm = HeroManager.instance;
        if (player == null)
        {
            TitleManager tm = TitleManager.instance;
            hm.AddHero(tm.selectedHeroData);
            player = Instantiate(playerPrefab);
            player.SetSprite(tm.selectedHeroSprite);        //This should be whichever Hero the player picked at the beginning
            player.transform.SetParent(transform);
            cameraFollow.objectTransform = player.transform;
            
            ui.partyDisplay[0].hero = hm.heroes[0];
            ui.partyDisplay[0].SetSprite(player.mapSprite);
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

        /****Create chests. It's possible for a dungeon to have no chests.****/
        chestCount = Random.Range(0, nodes.Count / 4);
        //int chestCount = 1;
        int tableLevel;
        if (dungeonLevel <= 5)
            tableLevel = 0;
        else if (dungeonLevel <= 10)
            tableLevel = 1;
        else
            tableLevel = 2;

        GenerateChests(chestCount, tableLevel);
        

        /****Mystery Nodes. Uses almost same code as treasure chests****/
        int mysteryNodeCount = Random.Range(0, nodes.Count / minNodeCount + 1);
        //int mysteryNodeCount = 1;

        if (mysteryNodeCount <= 0)
        {
            //deactivate mystery nodes since none will appear in this dungeon
            foreach(MysteryNode node in mysteryNodes)
            {
                node.ShowObject(false);
                node.nodeID = -1;
            }
        }

        for (int i = 0; i < mysteryNodeCount; i++)
        {
            MysteryNode mysteryNode;
            bool nodeInstantiated = false;

            if (i >= mysteryNodes.Count)
            {
                mysteryNode = Instantiate(mysteryPrefab);
                nodeInstantiated = true;
            }
            else
            {
                mysteryNode = mysteryNodes[i];
                mysteryNode.ShowObject(true);
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
                 
            mysteryNode.PlaceObject(randCol, randRow);
            
            if (nodeInstantiated)
            {
                mysteryNode.transform.SetParent(transform);
                mysteryNodes.Add(mysteryNode);
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
            //heroAppearanceChance = 1;
            if ((!dungeonLevelReset && Random.value <= heroAppearanceChance) || dungeonLevel == 5)
            {
                int randCaptive = Random.Range(0, captiveHeroes.Count);
                Captive captive = captiveHeroes[randCaptive];
                //Captive captive = captiveHeroes[0];
                captive.ShowObject(true);
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
                //raise appearance chance for next time. If dungeon was reset, reset appearance to 0.
                if (dungeonLevelReset)
                    heroAppearanceChance = 0;
                else
                    heroAppearanceChance += 0.1f;
            }
        }

        /****create enemy. The number of enemies is (total nodes / 4)****/
        int enemyCount = nodes.Count / 4;
        GenerateEnemies(enemyCount);
        
    }

    public void GenerateEnemies(int enemyCount, bool generateMajorEnemy = false)
    {
        //int enemyCount = nodes.Count / 4;
        int majorEnemyCount = enemyCount < 4 ? 0 : enemyCount / 4;
        bool forcedMajorEnemy = false;
        
        for (int i = 0; i < enemyCount; i++)
        {
            //check graveyard for any enemies to re-use
            MapEnemy enemy;
            bool enemyInstantiated = false;
            if (graveyard.Count > 0)
            {
                enemy = graveyard[graveyard.Count - 1]; //I add from the end so I don't have to mess with i, and all map enemies are the same
                enemy.ResetEnemy();
                enemies.Add(enemy);
                graveyard.Remove(enemy);

                //reset alpha in case it was changed previously.
                enemy.SetAlpha(1);
            }
            else
            {
                enemy = Instantiate(enemyPrefab);
                enemyInstantiated = true;
            }

            //is this a major enemy? note: a major enemy should always appear in level 5, and at the exit.
            float majorEnemyRoll = Random.value;
            Debug.Log("Major enemy spawn chance: " + majorEnemyRoll);
            if (generateMajorEnemy || (majorEnemyCount > 0 && majorEnemyRoll <= 0.4f) || (dungeonLevel == 5 && !forcedMajorEnemy))
            {
                //float majorEnemyRoll = Random.value;
                //Debug.Log("Major enemy spawn chance: " + majorEnemyRoll);
                //if (majorEnemyRoll <= 0.3f || generateMajorEnemy || (dungeonLevel == 5 && !forcedMajorEnemy))
                //{
                    enemy.SetSprite(enemy.majorEnemySprite);
                    enemy.isMajorEnemy = true;
                    //TODO: have a separate array in Enemy Manager for major enemies and pick one.

                    if (majorEnemyCount > 0) majorEnemyCount--;
                //}
            }

            //find a random node to occupy
            if ((dungeonLevel == 5 && enemy.isMajorEnemy) /*|| generateMajorEnemy*/)
            {
                //place a forced major enemy to introduce players to shield tokens.
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

            //generate encounters
            int tableLevel;
            if (dungeonLevel <= 5)
                tableLevel = 0;
            else if (dungeonLevel <= 10)
                tableLevel = 1;
            else if (dungeonLevel <= 15)
                tableLevel = 2;
            else if (dungeonLevel <= 20)
                tableLevel = 3;
            else
                tableLevel = 4;
            
            if (enemy.isMajorEnemy)
            {
                if ((dungeonLevel == 5 && !forcedMajorEnemy) /*|| generateMajorEnemy*/)
                {
                    forcedMajorEnemy = true;
                    generateMajorEnemy = false;
                    enemy.AddFixedEncounter((int)EnemyManager.EnemyName.Golem);
                }
                else
                {
                    //pick encounter based on table level.
                    if (tableLevel == 1)
                        enemy.AddFixedEncounter((int)EnemyManager.EnemyName.Lich);
                    else if (tableLevel == 2)
                        enemy.AddFixedEncounter((int)EnemyManager.EnemyName.Titan);
                }
            }
            else
            {
                //enemy.AddFixedEncounter((int)EnemyManager.EnemyName.Golem);
                enemy.AddEncounter(tableLevel);
            }

            //set turns. if the enemy is standing over a chest, stairs, or captive, they will not move.
            //exit check
            if (!exit.occupiedByEnemy && enemy.nodeID == exit.nodeID)
            {
                enemy.isStationary = true;
                //enemy becomes semi-transparent so player can see what's being guarded
                enemy.SetAlpha(0.6f);
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
                    enemy.SetAlpha(0.6f);
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
                    enemy.SetAlpha(0.6f);
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

    public void GenerateChests(int chestCount, int tableLevel, bool mysteryNodeInEffect = false)
    {
        if (chestCount <= 0)
        {
            //deactivate the chests since none will appear in this dungeon
            foreach(TreasureChest chest in chests)
            {
                chest.ShowObject(false);
                chest.nodeID = -1;
            }
        }

        for (int i = 0; i < chestCount; i++)
        {
            TreasureChest chest;
            bool chestInstantiated = false;

            //more chests are instantiated if there aren't enough
            if (i >= chests.Count)
            {
                chest = Instantiate(chestPrefab);
                chestInstantiated = true;
            }
            else
            {
                chest = chests[i];
                chest.ShowObject(true);
            }

            if (!mysteryNodeInEffect)
            {
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
                chest.PlaceObject(randCol, randRow);
            }
            
            //generate item
            if (mysteryNodeInEffect)
            {
                if (chest.heldItem == null)
                    if (tableLevel >= 4)    //can't go any higher
                        chest.GenerateLoot(tableLevel);
                    else
                        chest.GenerateLoot(tableLevel + 1);
            }
            else
                chest.GenerateLoot(tableLevel);

            if (chestInstantiated)
            {
                chest.transform.SetParent(transform);
                chests.Add(chest);
            }

        }
    }

}
