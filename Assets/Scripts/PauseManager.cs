using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    public static bool isPaused = false;
    public GameObject pauseUI;
    public GameObject settingsUI;
    public GameObject mainUI;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
        
    }

    public void Resume() {
        changeMenu(0);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause() {
        changeMenu(1);
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void changeMenu(int menuID) {
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        Time.timeScale = 0f;

        // None = 0
        // Pause = 1
        // Settings = 2
        if (menuID == 0) {
            mainUI.SetActive(true);
            pauseUI.SetActive(false);
            settingsUI.SetActive(false);
        } else if (menuID == 1) {
            mainUI.SetActive(false);
            pauseUI.SetActive(true);
            settingsUI.SetActive(false);
        } else if (menuID == 2) {
            mainUI.SetActive(false);
            pauseUI.SetActive(false);
            settingsUI.SetActive(true);
        }
    }

    public void changeScene(string scene) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void quitGame() {
        Application.Quit();
    }
}
