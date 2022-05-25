using UnityEngine;
using System.IO;


/* This game is a concept for a battle system. It's not intended to be a fully-featured game, as I just wanted to show off the idea.
    The dungeon consists of randomly generated rooms (called "nodes"). The player must explore each dungeon and find the missing heroes.*/
public class GameManager : MonoBehaviour
{

    public enum GameState { Normal, Combat, ShowCombatRewards, CombatEnded, CombatEndedPlayerRanAway, GenerateNewDungeon, GameOver }  //CombatEnded is used to deal with map enemies after combat is finished.
    public GameState gameState;                 //used by input manager to perform different actions with the same button press.
    public Dungeon dungeon;
    public Camera gameCamera;
    public int seed {get; set;}                 //used to generate random content.
    public int nodeCount {get; set;}                                   

    //singletons
    public static GameManager instance;
    CombatSystem combatSystem;
    HeroManager hm;
    UI ui;
    DungeonUI dungeonUI;
    BonusSystem bs;


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
        //get seed
        System.Random rnd = new System.Random();
        seed = rnd.Next(); 
        Random.InitState(seed);     //seed 1982010089 & 1471483880 are for testing
                                    //1943297621 is bad seed, 2nd level is broken   
        Debug.Log("Seed: " + seed);
        
        File.WriteAllText(@"C:\_Projects\RPG Battle System\Logs\seeds.txt", seed.ToString());


        nodeCount = dungeon.minNodeCount;
        SetState(GameState.GenerateNewDungeon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(GameState state)
    {
        gameState = state;
        combatSystem = CombatSystem.instance;
        ui = UI.instance;
        dungeonUI = DungeonUI.instance;
        bs = BonusSystem.instance;
        hm = HeroManager.instance;

        switch(gameState)
        {
            case GameState.Normal:
            case GameState.CombatEnded:
            case GameState.CombatEndedPlayerRanAway:
                //update dungeon party UI so that current HP, MP and XP are reflected
                dungeon.gameObject.SetActive(true);
                dungeonUI.gameObject.SetActive(true);
                for (int i = 0; i < hm.heroes.Count; i++)
                {
                    dungeonUI.partyDisplay[i].UpdateUI();
                }

                combatSystem.gameObject.SetActive(false);
                ui.gameObject.SetActive(false);
                bs.gameObject.SetActive(false);
                break;

            case GameState.Combat:
                dungeon.gameObject.SetActive(false);
                combatSystem.gameObject.SetActive(true);
                ui.gameObject.SetActive(true);
                dungeonUI.gameObject.SetActive(false);
                bs.gameObject.SetActive(true);
                break;

            case GameState.GenerateNewDungeon:
                dungeon.GenerateDungeon(nodeCount);
                SetState(GameState.Normal);
                break;
            
        }
    }

    public void SetCameraFollow(bool toggle)
    {
        gameCamera.GetComponent<CameraFollow>().enabled = toggle;
        gameCamera.transform.position = new Vector3(0, 0, gameCamera.transform.position.z);
    }

    public void GameOver()
    {
        Debug.Log("Game is over");

        //TODO: Show a game over scene
        combatSystem.gameObject.SetActive(false);
    }
}
