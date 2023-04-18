using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public static void deathScreen()
    {
        SceneManager.LoadScene("death");
    }

    public static void gameScreen()
    {
        SceneManager.LoadScene("game");
    }
}
