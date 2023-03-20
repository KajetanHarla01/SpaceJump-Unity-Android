using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip music;
    static bool play = false;
    public static bool playMusic = true;
    private void Awake()
    {
        ZPlayerPrefs.Initialize("9pe4GExr", "D84Dk344_dfs");
        if (!play)
        {
            playMusic = ZPlayerPrefs.HasKey("music") ? ZPlayerPrefs.GetInt("music") == 1 : true;
            DontDestroyOnLoad(gameObject);
            GetComponent<AudioSource>().clip = music;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
            play = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (!playMusic)
        {
            if (GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }
    }
}
