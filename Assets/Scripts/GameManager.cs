using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This game is a concept for a battle system. It's not intended to be a fully-featured game, as I just wanted to show off the idea.
    The dungeon consists of randomly generated rooms (called "nodes"). The player must explore each dungeon and find the missing heroes.*/
public class GameManager : MonoBehaviour
{

    public enum GameState { Normal, Combat, ShowCombatRewards, GameOver }
    public GameState gameState;                 //used by input manager to perform different actions with the same button press.
    public Dungeon dungeon;
    public Camera camera;                       

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

        switch(gameState)
        {
            case GameState.Normal:
                dungeon.gameObject.SetActive(true);
                combatSystem.gameObject.SetActive(false);
                ui.gameObject.SetActive(false);
                dungeonUI.gameObject.SetActive(true);
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
        camera.GetComponent<CameraFollow>().enabled = toggle;
        camera.transform.position = new Vector3(0, 0, camera.transform.position.z);
    }

    public void GameOver()
    {
        Debug.Log("Game is over");

        //TODO: Show a game over scene
        combatSystem.gameObject.SetActive(false);
    }
}
