using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour {
    public string sceneName;
    public void OnCollisionEnter2D(Collision2D collision) {
        if (sceneName == null) {
            Debug.LogWarning($"[{gameObject.name}] Scene \"{sceneName}\" does not exist!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public static void death() => SceneManager.LoadScene("death");
    // public static void gaming() => SceneManager.LoadScene("game");
    public static void gaming() => SceneManager.LoadScene("level2");
    public static void main() => SceneManager.LoadScene("mainmenu");

    public static void changeScene(string name) => SceneManager.LoadScene(name);
}
