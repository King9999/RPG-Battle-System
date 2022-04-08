using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CombatSystem combatSystem;

    public enum GameState { Normal, Combat}
    public GameState gameState;                 //used by input manager to perform different actions with the same button press.

    public static GameManager instance;

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
