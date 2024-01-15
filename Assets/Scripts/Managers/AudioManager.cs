using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSignalSO _sfxAudioSignal = default;
    [SerializeField] private AudioSignalSO _musicAudioSignal = default;

    [SerializeField] private VolumeSignalSO _sfxVolumeSignal = default;
    [SerializeField] private VolumeSignalSO _musicVolumeSignal = default;
    
    private AudioSource _musicSource;
    private float _sfxVolume = 1f;

    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
        if (_musicSource == null)
        {
            Debug.LogError("No AudioSource found on AudioManager");
        }
        _musicSource.loop = true;
    }

    private void OnEnable()
    {
        _sfxAudioSignal.OnTriggered += PlaySFX;
        _musicAudioSignal.OnTriggered += PlayMusic;
        _sfxVolumeSignal.OnTriggered += SetSFXVolume;
        _musicVolumeSignal.OnTriggered += SetMusicVolume;
    }

    private void OnDisable()
    {
        _sfxAudioSignal.OnTriggered -= PlaySFX;
        _musicAudioSignal.OnTriggered -= PlayMusic;
        _sfxVolumeSignal.OnTriggered -= SetSFXVolume;
        _musicVolumeSignal.OnTriggered -= SetMusicVolume;
    }

    private void PlaySFX(AudioClip audioClip, Vector3 position, float volume)
    {
        Debug.Log("Playing SFX at " + position + " with volume " + volume);
        AudioSource.PlayClipAtPoint(audioClip, position, volume * _sfxVolume);
    }

    private void PlayMusic(AudioClip audioClip, Vector3 position, float volume)
    {
        _musicSource.clip = audioClip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }

    private void SetMusicVolume(float volume)
    {
        Debug.Log("Received music volume " + volume + " from signal");
        _musicSource.volume = volume;
    }

    private void SetSFXVolume(float volume)
    {
        Debug.Log("Received sfx volume " + volume + " from signal");
        _sfxVolume = volume;
    }
}