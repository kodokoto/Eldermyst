using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSignalSO _sfxAudioSignal = default;
    [SerializeField] private AudioSignalSO _musicAudioSignal = default;

    private AudioSource _musicSource;

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
    }

    private void OnDisable()
    {
        _sfxAudioSignal.OnTriggered -= PlaySFX;
        _musicAudioSignal.OnTriggered -= PlayMusic;
    }

    private void PlaySFX(AudioClip audioClip, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlayMusic(AudioClip audioClip, Vector3 position, float volume)
    {
        _musicSource.clip = audioClip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }
}