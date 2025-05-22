using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseScreen;
    public GameObject gameWinScreen;
    bool gameisOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gameLoseScreen.SetActive(false);
        FindObjectOfType<Player>().OnPlayerSeen += OnGameOver;
        FindObjectOfType<Player>().OnPlayerPast += OnGamePast;
    }
    

    void OnGameOver()
    {
        GameOver(gameLoseScreen);
    }

    void OnGamePast()
    {
        GameOver(gameWinScreen);
    }

    void GameOver(GameObject gameOverScreen)
    {
        gameOverScreen.SetActive(true);
        gameisOver = true;
        FindObjectOfType<Player>().OnPlayerSeen -= OnGameOver;
        FindObjectOfType<Player>().OnPlayerPast -= OnGamePast;
    }
}
