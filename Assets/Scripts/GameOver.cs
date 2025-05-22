using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour
{
    
    public Text coinText;
    
    // Start is called before the first frame update
    void Start()
    {
        coinText.text = "Coins collected: " + GameManager.coinCount;

    }
    

    public void RestartGame()
    {
        GameManager.ResetCoin();
        SceneManager.LoadScene("Map2");
    }

    public void MainMenu()
    {
        GameManager.ResetCoin();
        SceneManager.LoadScene("MainMenu");
    }

    
}
