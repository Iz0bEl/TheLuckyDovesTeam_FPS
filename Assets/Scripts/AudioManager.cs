using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip defaultSong;

    private AudioSource song1, song2;
    private bool isPLayingSong1;

    public static AudioManager instance;

    [Header("------Audio Stuff------")]
    [SerializeField] AudioMixer Mixer;
    [SerializeField] Slider MusicVolSlider;
    [SerializeField] Slider SFXVolSlider;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        song1 = gameObject.AddComponent<AudioSource>();
        song2 = gameObject.AddComponent<AudioSource>();
        isPLayingSong1 = true;

        SwapMusic(defaultSong);


        if (PlayerPrefs.HasKey("ExposeMusic"))
        {
            MusicVolSlider.value = PlayerPrefs.GetFloat("ExposeMusic");
            SetMixerValue("ExposeMusic", MusicVolSlider.value);
        }

        if (PlayerPrefs.HasKey("ExposeSFX"))
        {
            SFXVolSlider.value = PlayerPrefs.GetFloat("ExposeSFX");
            SetMixerValue("ExposeSFX", SFXVolSlider.value);
        }
    }

    public void MusicVolumeSlider()
    {
        SetMixerValue("ExposeMusic", MusicVolSlider.value);
        PlayerPrefs.SetFloat("ExposeMusic", MusicVolSlider.value);

    }

    public void SFXVolumeSlider()
    {
        SetMixerValue("ExposeSFX", SFXVolSlider.value);
        PlayerPrefs.SetFloat("ExposeSFX", SFXVolSlider.value);
    }

    void SetMixerValue(string key, float val)
    {
        Mixer.SetFloat(key, Mathf.Log10(val) * 20);
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

        if (isPLayingSong1)
        {
            song2.clip = newSong;
            song2.Play();

            while (timePassed < fadeTime)
            {
                song2.volume = Mathf.Lerp(0, 1, timePassed / fadeTime);
                song1.volume = Mathf.Lerp(1, 0, timePassed / fadeTime);
                timePassed += Time.deltaTime;

                yield return null;
            }

            song1.Stop();
        }
        else
        {
            song1.clip = newSong;
            song1.Play();

            while (timePassed < fadeTime)
            {
                song1.volume = Mathf.Lerp(0, 1, timePassed / fadeTime);
                song2.volume = Mathf.Lerp(1, 0, timePassed / fadeTime);
                timePassed += Time.deltaTime;

                yield return null;
            }

            song2.Stop();
        }
    }
}
