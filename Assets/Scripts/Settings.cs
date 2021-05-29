using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{

    public AudioMixer musicAudioMixer;
    public AudioMixer effectsAudioMixer;

    public Slider musicSlider;
    public Slider effectSlider;


    public void SetVolumeMusic(float volume)
    {
        musicSlider.value = volume;
        musicAudioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetVolumeEffects(float volume)
    {
        effectSlider.value = volume;
        effectsAudioMixer.SetFloat("EffectVolume", volume);
    }



    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}
