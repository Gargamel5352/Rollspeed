using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public static void death()
    {
        SceneManager.LoadScene("death");
    }

    public static void gaming()
    {
        SceneManager.LoadScene("game");
    }
}
