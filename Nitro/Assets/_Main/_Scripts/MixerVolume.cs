using UnityEngine;
using UnityEngine.Audio;

namespace Synith
{
    public class MixerVolume : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;

        const string MASTER_AUDIO_GROUP_NAME = "MasterVolume";

        public void ChangeVolume(float value)
        {
            if (audioMixer == null) return;
            audioMixer?.SetFloat(MASTER_AUDIO_GROUP_NAME, Mathf.Log10(value) * 20);
        }
    }
}