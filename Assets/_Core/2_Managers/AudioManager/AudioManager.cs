using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private float masterVolume;
    public float MasterVolume
    {
        get => masterVolume;
        set {
            masterVolume = value;  
            mixer.SetFloat("MasterVolume", value);
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
    }
    private float musicVolume;
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
    private float effectVolume;
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
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            EffectVolume = PlayerPrefs.GetFloat("EffectVolume");
        }
    }

    

  
}
