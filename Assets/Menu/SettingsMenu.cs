using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    //vars
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;

    public AudioMixer mixer;
    public Slider masterSlider, musicSlider, effectsSlider;


    private void Start()
    {
        //clears DropDown and adds all possible resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + "x" + resolution.height;
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

        float vol = 0f;
        mixer.GetFloat("MasterVolume", out vol);
        masterSlider.value = vol;
        mixer.GetFloat("MusicVolume", out vol);
        musicSlider.value = vol;
        mixer.GetFloat("EffectVolume", out vol);
        effectsSlider.value = vol;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resoltion = resolutions[resolutionIndex];
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

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
