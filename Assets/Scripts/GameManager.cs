using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static int coinCount = 0;

    public static void AddCoin()
    {
        coinCount++;
        
    }

    public static void ResetCoin()
    {
        coinCount = 0;
    }
    
}
