using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This game is a concept for a battle system. It's not intended to be a fully-featured game, as I just wanted to show off the idea.
    The dungeon consists of randomly generated rooms (called "nodes"). The player must explore each dungeon and find the missing heroes.*/
public class GameManager : MonoBehaviour
{

    public enum GameState { Normal, Combat, ShowCombatRewards, CombatEnded, GameOver }  //CombatEnded is used to deal with map enemies after combat is finished.
    public GameState gameState;                 //used by input manager to perform different actions with the same button press.
    public Dungeon dungeon;
    public Camera gameCamera;                       

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
        //combatSystem = CombatSystem.instance;
        //hm = HeroManager.instance;
        //gameState = GameState.Normal;
        //hm.gameObject.SetActive(false);
        //combatSystem.gameObject.SetActive(false);
        SetState(GameState.Normal);
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
