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


    public AudioClip defaultSong;

    [SerializeField] public AudioClip song1, song2;
    private bool isPLayingSong1;

    public AudioClip newTrack;

    public static SoundManagerTemp2 instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {

        isPLayingSong1 = true;

        PlayMusic(defaultSong);

        //SwapMusic(defaultSong);

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
            background_Float = PlayerPrefs.GetFloat(backPref);
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

    public void SwapMusic(AudioClip newSong)
    {
        StopAllCoroutines();

        StartCoroutine(fadeMusic(newSong));

        isPLayingSong1 = !isPLayingSong1;
    }

    public void ReturnToDefault()
    {
        SwapMusic(defaultSong);
    }

    private IEnumerator fadeMusic(AudioClip newSong)
    {

        float fadeTime = 1.25f;
        float timePassed = 0;

        //if (isPLayingSong1)
        //{
        //    while (timePassed < fadeTime)
        //    {
        //        BackGroundAudio.volume = Mathf.Lerp(0, 1, timePassed / fadeTime);
        //        BackGroundAudio.volume = Mathf.Lerp(1, 0, timePassed / fadeTime);
        //        timePassed += Time.deltaTime;

        //        yield return null;
        //    }

        //    BackGroundAudio.clip = newSong;
        //    BackGroundAudio.Stop();

        //    BackGroundAudio.volume = background_Float;

        //    //isPLayingSong1 = !isPLayingSong1;
        //}


        while (timePassed < fadeTime)
        {
            BackGroundAudio.volume = Mathf.Lerp(0, background_Float, timePassed / fadeTime);
            BackGroundAudio.volume = Mathf.Lerp(background_Float, 0, timePassed / fadeTime);
            timePassed += Time.deltaTime;

            yield return null;
        }

        BackGroundAudio.Stop();

        BackGroundAudio.clip = newSong;
        BackGroundAudio.Play();
        timePassed= 0;

        while (timePassed < fadeTime)
        {
            BackGroundAudio.volume = Mathf.Lerp(background_Float, 0, timePassed / fadeTime);
            BackGroundAudio.volume = Mathf.Lerp(0, background_Float, timePassed / fadeTime);
            timePassed += Time.deltaTime;

            yield return null;
        }
        //BackGroundAudio.volume = background_Float;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwapMusic(newTrack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReturnToDefault();
        }
    }

    public void PlayMusic(AudioClip song)
    {
        BackGroundAudio.clip = song;

        BackGroundAudio.Play(0);

    }
}


