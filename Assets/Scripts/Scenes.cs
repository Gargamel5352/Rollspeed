using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour {
    public string sceneName;
    public void OnCollisionEnter2D(Collision2D _) {
        if (sceneName == null) {
            Debug.LogWarning($"[{gameObject.name}] Scene \"{sceneName}\" does not exist!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public static void Death() => SceneManager.LoadScene("death");
    public static void Gaming() => SceneManager.LoadScene("level1");
    public static void Main() => SceneManager.LoadScene("mainmenu");
    public static void LastLevel() {
        LastScene last = GameObject.FindWithTag("level manager").GetComponent<LastScene>(); // very good code
        // Why does this actually work
        SceneManager.LoadScene("level1");
        SceneManager.LoadScene(last.sceneName);
    }

    public static void ChangeScene(string name) => SceneManager.LoadScene(name);
}
