using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    //vars
    private Resolution[] resolutions;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] Slider masterSlider, musicSlider, effectsSlider;
    [SerializeField] private AudioManager manager;
    


    private void Start()
    {
        //clears DropDown and adds all possible resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        var currentResolutionIndex = 0;
        foreach (Resolution resolution in resolutions)
        {
            var option = resolution.width + "x" + resolution.height;
            options.Add(option);
            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = options.IndexOf(option);
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    
        masterSlider.value = manager.MasterVolume;
        musicSlider.value = manager.MusicVolume;
        effectsSlider.value = manager.EffectVolume;
    }
  
    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        manager.MasterVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        manager.EffectVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        manager.MusicVolume = volume;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
