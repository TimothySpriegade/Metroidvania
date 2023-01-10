using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        // Components
        [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
        [SerializeField] private Slider masterSlider, musicSlider, effectsSlider;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Toggle fullscreenToggle;

        // Variables
        private List<Resolution> addedResolutions;


        private void Start()
        {
            //clears DropDown and adds all possible resolutions
            var resolutions = Screen.resolutions;
            addedResolutions = new List<Resolution>();
            var options = new List<string>();

            var currentResolutionIndex = 0;

            resolutionDropdown.ClearOptions();

            foreach (var resolution in resolutions)
            {
                var option = resolution.width + "x" + resolution.height;
                if (!options.Contains(option))
                {
                    options.Add(option);
                    addedResolutions.Add(resolution);
                    if (resolution.width == Screen.currentResolution.width &&
                        resolution.height == Screen.currentResolution.height)
                    {
                        currentResolutionIndex = options.IndexOf(option);
                    }
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            // Initialize volume sliders
            masterSlider.value = audioManager.MasterVolume;
            musicSlider.value = audioManager.MusicVolume;
            effectsSlider.value = audioManager.EffectVolume;

            // Initialize fullScreen toggle
            fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
        }

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        public void SetResolution(int resolutionIndex)
        {
            var resolution = addedResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetMasterVolume(float volume)
        {
            audioManager.MasterVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            audioManager.EffectVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            audioManager.MusicVolume = volume;
        }

        public void ToggleFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}