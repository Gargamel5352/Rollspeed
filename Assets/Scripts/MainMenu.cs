using UnityEngine;

public class MainMenu : MonoBehaviour {
    void Awake() {
        Manager.Instance.playTime = 0f;
    }
    public static void exitGame() {
        Application.Quit();
    }
}
