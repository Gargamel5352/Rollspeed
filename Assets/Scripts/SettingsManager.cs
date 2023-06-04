using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public Dropdown QualityDropdown;
    public Dropdown ResolutionsDropdown;
    public Slider MusicSlider;
    public Slider MasterSlider;
    [SerializeField] AudioSource music;

    void Awake() {
        music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
    }
    void Start() {
        // Quality Dropdown
        QualityDropdown.ClearOptions();
        QualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        QualityDropdown.value = Mathf.Clamp(QualitySettings.GetQualityLevel(), 0, QualityDropdown.options.Count - 1);
        QualityDropdown.RefreshShownValue();
        QualityDropdown.onValueChanged.AddListener(OnQualityChange);
        
        // Resolution Dropdown
        ResolutionsDropdown.ClearOptions();
        List<string> resolutions = new();
        int currentResInd = 0;
        for (int i = 0; i < Screen.resolutions.Length; i++) {
            resolutions.Add($"{Screen.resolutions[i].width}x{Screen.resolutions[i].height}");
            if (Screen.resolutions[i].width == Screen.currentResolution.width && Screen.resolutions[i].height == Screen.currentResolution.height) {
                currentResInd = i;
            }
        }
        ResolutionsDropdown.AddOptions(resolutions);
        ResolutionsDropdown.value = currentResInd;
        ResolutionsDropdown.RefreshShownValue();
        ResolutionsDropdown.onValueChanged.AddListener(OnResolutionChange);

        // Music Volume
        MusicSlider.value = music.volume;
        MusicSlider.onValueChanged.AddListener(OnMusicVolumeChange);


        // Master Volume
        MasterSlider.value = AudioListener.volume;
        MasterSlider.onValueChanged.AddListener(OnMasterVolumeChange);
    }

    public void OnQualityChange(int qualityLevel) {
        QualitySettings.SetQualityLevel(Mathf.Clamp(qualityLevel, 0, QualityDropdown.options.Count - 1));
        PlayerPrefs.SetInt("QualityLevel", QualitySettings.GetQualityLevel());
    }
    public void OnResolutionChange(int resolutionLevel) {
        Screen.SetResolution(Screen.resolutions[resolutionLevel].width, Screen.resolutions[resolutionLevel].height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionLevel", resolutionLevel);
    }
    public void OnMusicVolumeChange(float volume) {
        music.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void OnMasterVolumeChange(float volume) {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
