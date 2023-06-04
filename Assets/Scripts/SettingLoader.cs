using UnityEngine;

public class SettingLoader: MonoBehaviour {
    [SerializeField] AudioSource music;

    void Awake() {
        music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
    }
    
    private void Start() {
        // Quality
        if (PlayerPrefs.HasKey("QualityLevel")) {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
        }

        // Resolution
        if (PlayerPrefs.HasKey("ResolutionLevel")) {
            Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("ResolutionLevel")].width, Screen.resolutions[PlayerPrefs.GetInt("ResolutionLevel")].height, Screen.fullScreen);
        }

        // Music Volume
        music.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Master Volume
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }
}
