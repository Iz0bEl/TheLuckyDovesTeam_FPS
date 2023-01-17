using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerTemp2 : MonoBehaviour
{
    private static readonly string FirstPlay = "FisrtPlay";
    private static readonly string backPref = "backPref";
    private static readonly string soundPref = "soundPref";
    private int FirstPlayint;
    public Slider background_Slider, soundE_Slider;
    private float background_Float, soundE_Float;
    public AudioSource BackGroundAudio;
    public AudioSource[] SoundEffectsAudio;

    void Start()
    {
        FirstPlayint = PlayerPrefs.GetInt(FirstPlay);
        if (FirstPlayint == 0) 
        {
            background_Float = 1f;
            soundE_Float = 1f;
            background_Slider.value = background_Float;
            soundE_Slider.value = soundE_Float;

            //Playerprefs allow you to save values that last through plays

            PlayerPrefs.SetFloat(backPref, background_Float);
            PlayerPrefs.SetFloat(soundPref, soundE_Float);

            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
           background_Float =  PlayerPrefs.GetFloat(backPref);
           background_Slider.value = background_Float;

            soundE_Float = PlayerPrefs.GetFloat(soundPref);
            soundE_Slider.value = soundE_Float;
        }
    }
    public void Save()
    {
        PlayerPrefs.SetFloat(backPref, background_Slider.value);
        PlayerPrefs.SetFloat(soundPref, soundE_Slider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Save();
        }
    }

    public void UpdateSound()
    {
        BackGroundAudio.volume = background_Slider.value;

        //means we can have any value for sound effects
        for (int i = 0; i < SoundEffectsAudio.Length; i++)
        {
            SoundEffectsAudio[i].volume = soundE_Slider.value;
        }
    }


}
