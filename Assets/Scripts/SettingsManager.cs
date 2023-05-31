using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public Camera PlayerCamera;
    public Dropdown QualityDropdown;
    public Dropdown ResolutionsDropdown;

    void Start() {
        if (PlayerPrefs.HasKey("QualityLevel")) {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
        }
        if (PlayerPrefs.HasKey("ResolutionLevel")) {
            Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("ResolutionLevel")].width, Screen.resolutions[PlayerPrefs.GetInt("ResolutionLevel")].height, Screen.fullScreen);
        } 
        ResolutionsDropdown.ClearOptions();
        List<string> resolutions = new List<string>();
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
        QualityDropdown.ClearOptions();
        QualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        QualityDropdown.value = Mathf.Clamp(QualitySettings.GetQualityLevel(), 0, QualityDropdown.options.Count - 1);
        QualityDropdown.RefreshShownValue();
    }

    public void onQualityChange(int qualityLevel) {
        QualitySettings.SetQualityLevel(Mathf.Clamp(qualityLevel, 0, QualityDropdown.options.Count - 1));
        PlayerPrefs.SetInt("QualityLevel", QualitySettings.GetQualityLevel());
    }

    public void onResolutionChange(int resolutionLevel) {
        Screen.SetResolution(Screen.resolutions[resolutionLevel].width, Screen.resolutions[resolutionLevel].height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionLevel", resolutionLevel);
    }
}
