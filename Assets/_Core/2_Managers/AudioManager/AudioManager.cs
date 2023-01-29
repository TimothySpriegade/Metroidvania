using UnityEngine;
using UnityEngine.Audio;

namespace _Core._2_Managers.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        private float effectVolume;
        private float masterVolume;
        private float musicVolume;

        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = value;
                mixer.SetFloat("MasterVolume", value);
                PlayerPrefs.SetFloat("MasterVolume", value);
            }
        }

        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                musicVolume = value;
                mixer.SetFloat("MusicVolume", value);
                PlayerPrefs.SetFloat("MusicVolume", value);
            }
        }

        public float EffectVolume
        {
            get => effectVolume;
            set
            {
                effectVolume = value;
                mixer.SetFloat("EffectVolume", value);
                PlayerPrefs.SetFloat("EffectVolume", value);
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (PlayerPrefs.HasKey("MasterVolume")) MasterVolume = PlayerPrefs.GetFloat("MasterVolume");

            if (PlayerPrefs.HasKey("MusicVolume")) MusicVolume = PlayerPrefs.GetFloat("MusicVolume");

            if (PlayerPrefs.HasKey("EffectVolume")) EffectVolume = PlayerPrefs.GetFloat("EffectVolume");
        }
    }
}