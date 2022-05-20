using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This game is a concept for a battle system. It's not intended to be a fully-featured game, as I just wanted to show off the idea.
    The dungeon consists of randomly generated rooms (called "nodes"). The player must explore each dungeon and find the missing heroes.*/
public class GameManager : MonoBehaviour
{

    public enum GameState { Normal, Combat, ShowCombatRewards, GameOver }
    public GameState gameState;                 //used by input manager to perform different actions with the same button press.
    public int dungeonLevel {get; set;}         //how far into the game the player is. Dungeon complexity increases after certain thresholds.

    //singletons
    public static GameManager instance;
    CombatSystem combatSystem;
    HeroManager hm;


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
        combatSystem = CombatSystem.instance;
        hm = HeroManager.instance;
        gameState = GameState.Normal;
        //hm.gameObject.SetActive(false);
        //combatSystem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(GameState state)
    {
        gameState = state;

        switch(gameState)
        {
            
        }
    }

    public void GameOver()
    {
        Debug.Log("Game is over");

        //TODO: Show a game over scene
        combatSystem.gameObject.SetActive(false);
    }
}
