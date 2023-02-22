using UnityEngine;

namespace Synith
{
    [RequireComponent(typeof(VolumeSliderUI), typeof(MixerVolume))]
    public class VolumeManager : MonoBehaviour
    {
        VolumeSliderUI volumeSliderUI;
        MixerVolume mixerVolume;
        void Awake()
        {
            volumeSliderUI = GetComponent<VolumeSliderUI>();
            mixerVolume = GetComponent<MixerVolume>();
        }

        void Start() => volumeSliderUI.OnVolumeChanged += VolumeSliderUI_OnVolumeChanged;
        void OnDestroy() => volumeSliderUI.OnVolumeChanged -= VolumeSliderUI_OnVolumeChanged;
        void VolumeSliderUI_OnVolumeChanged(float sliderValue) => mixerVolume.ChangeVolume(sliderValue);
    }
}
