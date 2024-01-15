using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private VolumeSignalSO _sfxVolumeSignal;
    [SerializeField] private VolumeSignalSO _musicVolumeSignal;

    [SerializeField] private AudioSettingsSO _audioSettings;

    private Slider musicSlider;
    private Slider sfxSlider;

    private void Awake()
    {
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();

        Debug.Log("changing music volume to " + _audioSettings.MusicVolume);
        Debug.Log("changing sfx volume to " + _audioSettings.SFXVolume);

        musicSlider.value = _audioSettings.MusicVolume;
        sfxSlider.value = _audioSettings.SFXVolume;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("changing music volume to " + volume);
        _audioSettings.MusicVolume = volume;
        _musicVolumeSignal.Trigger(volume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("changing sfx volume to " + volume);
        _audioSettings.SFXVolume = volume;
        _sfxVolumeSignal.Trigger(volume);
    }
}