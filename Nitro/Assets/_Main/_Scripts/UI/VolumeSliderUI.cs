using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Synith
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSliderUI : MonoBehaviour
    {
        public event Action<float> OnVolumeChanged;

        [SerializeField] TextMeshProUGUI currentVolumeLeveltext;
        Slider slider;


        const string MUSIC_VOLUME_PREF = "Music_Volume";

        void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener((value) => { ChangeVolume(value); });
        }
        void Start()
        {
            if (!PlayerPrefs.HasKey(MUSIC_VOLUME_PREF)) return;

            float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PREF);
            ChangeVolume(volume);
            slider.value = volume;
        }

        void ChangeVolume(float value)
        {
            float volumePercent = value * 100;

            PlayerPrefs.SetFloat(MUSIC_VOLUME_PREF, value);
            OnVolumeChanged?.Invoke(value);
            UpdateText(value);
        }

        void UpdateText(float value)
        {
            if (currentVolumeLeveltext == null) return;
            currentVolumeLeveltext.SetText($"{value * 100:00}");
        }

    }
}