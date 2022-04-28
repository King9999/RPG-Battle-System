using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState { Normal, Combat, ShowCombatRewards, GameOver }
    public GameState gameState;                 //used by input manager to perform different actions with the same button press.

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
        //hm.gameObject.SetActive(false);
        combatSystem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("Game is over");

        //TODO: Show a game over scene
        combatSystem.gameObject.SetActive(false);
    }
}
