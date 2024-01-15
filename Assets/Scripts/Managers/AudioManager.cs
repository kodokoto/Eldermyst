using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSignalSO _sfxAudioSignal = default;
    [SerializeField] private AudioSignalSO _musicAudioSignal = default;

    [SerializeField] private AudioSource _musicSource = default;

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
        _musicSource.loop = true;
        _musicSource.Play();
    }
}