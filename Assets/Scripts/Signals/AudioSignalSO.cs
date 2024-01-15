using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Signals/Audio Signal")]
public class AudioSignalSO : SerializableScriptableObject
{
    public UnityAction<AudioClip, Vector3, float> OnTriggered;

    public void Trigger(AudioClip audioClip, Vector3 position, float volume)
    {
        OnTriggered?.Invoke(audioClip, position, volume);
    }
}