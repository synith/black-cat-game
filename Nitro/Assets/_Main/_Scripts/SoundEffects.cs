using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEffects : MonoBehaviour
    {
        public static SoundEffects Instance { get; private set; }

        AudioSource audioSource;
        public void PlayClip(AudioClip clip) => audioSource.PlayOneShot(clip);
        public void PlayClip(AudioClip clip, float volumeScale) => audioSource.PlayOneShot(clip, volumeScale);
        public void StopClip() => audioSource.Stop();

        void Awake()
        {
            #region Singleton

            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            #endregion
            audioSource = GetComponent<AudioSource>();
        }
        float GetVolume() => audioSource.volume;
        void SetVolume(float volume) => audioSource.volume = volume;
    }
}