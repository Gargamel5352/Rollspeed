using UnityEngine.SceneManagement;
using UnityEngine;

class LastScene: MonoBehaviour {
    public string sceneName;
    void Awake() {
        SceneManager.GetSceneByName("level1");
        SceneManager.sceneLoaded += OnSceneChange;
    }

    void OnSceneChange(Scene scene, LoadSceneMode _) {
        if (scene.name.StartsWith("level")) {
            sceneName = scene.name;
        }
    }
}
