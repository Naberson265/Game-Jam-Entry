using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider renderDistSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider voiceVolumeSlider;

    public Toggle occlusionCullToggle;
    public Toggle turnCam;
    void Start()
    {
        CheckForDefaults();
    }
    public void SetToDefaults()
    {
        // Sets everything to zero, then uses the below function to set to defaults.
        PlayerPrefs.SetFloat("Sensitivity", 0);
        PlayerPrefs.SetFloat("RenderDist", 0);
        PlayerPrefs.SetInt("OcclusionCulling", 0);
        PlayerPrefs.SetInt("TurnWithCamera", 0);
        PlayerPrefs.SetFloat("MusicVolume", 0);
        PlayerPrefs.SetFloat("SFXVolume", 0);
        PlayerPrefs.SetFloat("VoiceVolume", 0);
        CheckForDefaults();
    }
    public void CheckForDefaults()
    {
        // Sets every setting to their default value, then resets all interactables.
        if (PlayerPrefs.GetFloat("Sensitivity") == 0) PlayerPrefs.SetFloat("Sensitivity", 1);
        if (PlayerPrefs.GetFloat("RenderDist") == 0) PlayerPrefs.SetFloat("RenderDist", 1000);
        if (PlayerPrefs.GetFloat("MusicVolume") == 0) PlayerPrefs.SetFloat("MusicVolume", 0);
        if (PlayerPrefs.GetFloat("SFXVolume") == 0) PlayerPrefs.SetFloat("SFXVolume", 0);
        if (PlayerPrefs.GetFloat("VoiceVolume") == 0) PlayerPrefs.SetFloat("VoiceVolume", 0);
        if (PlayerPrefs.GetInt("OcclusionCulling") == 0) PlayerPrefs.SetInt("OcclusionCulling", 2);
        if (PlayerPrefs.GetInt("TurnWithCamera") == 0) PlayerPrefs.SetInt("TurnWithCamera", 1);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        renderDistSlider.value = PlayerPrefs.GetFloat("RenderDist");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume");
        if (PlayerPrefs.GetInt("OcclusionCulling") == 2) occlusionCullToggle.isOn = true;
        else occlusionCullToggle.isOn = false;
        if (PlayerPrefs.GetInt("TurnWithCamera") == 2) turnCam.isOn = true;
        else turnCam.isOn = false;
    }
    void Update()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.SetFloat("RenderDist", renderDistSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolumeSlider.value);
        // 0 is default so the bools (done with ints) are set to 1/2 instead of 0/1.
        if (occlusionCullToggle.isOn) PlayerPrefs.SetInt("OcclusionCulling", 2);
        else PlayerPrefs.SetInt("OcclusionCulling", 1);
        if (turnCam.isOn) PlayerPrefs.SetInt("TurnWithCamera", 2);
        else PlayerPrefs.SetInt("TurnWithCamera", 1);
    }
}
