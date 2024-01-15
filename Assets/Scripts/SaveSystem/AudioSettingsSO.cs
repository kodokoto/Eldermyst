using UnityEngine;


[CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/AudioSettings")]
public class AudioSettingsSO : ScriptableObject
{


    [SerializeField] private VolumeSignalSO _sfxVolumeSignal;
    [SerializeField] private VolumeSignalSO _musicVolumeSignal;
    public float SFXVolume { get; set; } = 0.5f;
    public float MusicVolume { get; set; } = 0.5f;

    private void OnEnable()
    {
        _sfxVolumeSignal.OnTriggered += SetSFXVolume;
        _musicVolumeSignal.OnTriggered += SetMusicVolume;
    }

    private void OnDisable()
    {
        _sfxVolumeSignal.OnTriggered -= SetSFXVolume;
        _musicVolumeSignal.OnTriggered -= SetMusicVolume;
    }

    private void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
    }

    private void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
    }
}