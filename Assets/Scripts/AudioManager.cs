using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip defaultSong;

    private AudioSource song1, song2;
    private bool isPLayingSong1;

    public static AudioManager instance;

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
