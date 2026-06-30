using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Icons")]
    [SerializeField] public Image BGMIcon;
    [SerializeField] public Image sfxIcon;

    [SerializeField] public Sprite soundOn;
    [SerializeField] public Sprite soundOff;

    public bool BGMMuted;
    public bool sfxMuted;

    public void Start()
    {
        // Load setting sebelumnya
        BGMMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        ApplyBGM();
        ApplySFX();
    }

    public void ToggleBGM()
    {
        BGMMuted = !BGMMuted;

        ApplyBGM();

        PlayerPrefs.SetInt("BGMMuted", BGMMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;

        ApplySFX();

        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ApplyBGM()
    {
        if (BGMMuted)
        {
            audioMixer.SetFloat("BGMVolume", -80f);
            BGMIcon.sprite = soundOff;
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", 0f);
            BGMIcon.sprite = soundOn;
        }
    }

    public void ApplySFX()
    {
        if (sfxMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
            sfxIcon.sprite = soundOff;
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", 0f);
            sfxIcon.sprite = soundOn;
        }
    }
}