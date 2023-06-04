using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnScene : MonoBehaviour {
    [SerializeField] public string[] scenes;
    void Awake() {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    void OnSceneChange(Scene scene, LoadSceneMode _) {
        if (scenes.Contains(scene.name)) {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneChange;
        }
    }
}
