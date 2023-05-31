using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {
    public string sceneName;

    public void OnTriggerEnter2D(Collider2D collider) {
        SceneManager.LoadScene(sceneName);
    }
}
