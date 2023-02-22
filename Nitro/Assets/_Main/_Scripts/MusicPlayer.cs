using System.Collections;
using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] musicTracks;

        bool isFading;
        float fadeTime = 2f;
        AudioSource audioSource;
        int currentTrack = -1;
        float volumeLevel;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            volumeLevel = audioSource.volume;
            currentTrack = GetRandomTrack();
        }

        public void PlayRandomTrack() => PlayMusic(GetRandomTrack());

        public void PlayNextTrack() => PlayMusic(GetNextTrack());

        int GetRandomTrack() => Random.Range(0, musicTracks.Length);

        int GetNextTrack() => (currentTrack + 1) % musicTracks.Length;

        void Update()
        {
            if (musicTracks.Length == 0) return;

            if (!audioSource.isPlaying)
            {
                PlayNextTrack();
            }

            if (!isFading) return;
            audioSource.volume -= volumeLevel * Time.deltaTime / fadeTime;
        }

        void PlayMusic(int trackNumber)
        {
            if (trackNumber == currentTrack) return;

            if (audioSource.isPlaying)
            {
                StartCoroutine(FadeOut(trackNumber));
            }
            else
            {
                currentTrack = trackNumber;
                audioSource.clip = musicTracks[trackNumber];
                audioSource.Play();
            }
        }

        IEnumerator FadeOut(int trackNumber)
        {
            float startVolume = audioSource.volume;

            isFading = true;

            yield return new WaitUntil(() => audioSource.volume < 0.1f);
            isFading = false;

            audioSource.Stop();
            audioSource.volume = startVolume;
            volumeLevel = audioSource.volume;

            currentTrack = trackNumber;
            audioSource.clip = musicTracks[trackNumber];
            audioSource.Play();
        }
    }
}