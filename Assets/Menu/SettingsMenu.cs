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

        var volume = 0f;
        mixer.GetFloat("MasterVolume", out volume);
        masterSlider.value = volume;
        mixer.GetFloat("MusicVolume", out volume);
        musicSlider.value = volume;
        mixer.GetFloat("EffectVolume", out volume);
        effectsSlider.value = volume;
    }
    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        Debug.Log(volume);
        mixer.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }
    public void SetSFXVolume(float volume)
    {
        Debug.Log(volume);
        mixer.SetFloat("EffectVolume", effectsSlider.value);
        PlayerPrefs.SetFloat("EffectVolume", effectsSlider.value);
    }
    public void SetMusicVolume(float volume)
    {
        Debug.Log(volume);
        mixer.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
