using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds;
    private void Awake()
    {
        ZPlayerPrefs.Initialize("9pe4GExr", "D84Dk344_dfs");
        if (!ZPlayerPrefs.HasKey("sound"))
        {
            ZPlayerPrefs.SetInt("sound", 1);
            ZPlayerPrefs.Save();
        }
    }
    public void PlaySound(int num)
    {
        if (ZPlayerPrefs.GetInt("sound") == 1)
        {
            AudioSource AS = GameObject.FindGameObjectWithTag("Sounds").AddComponent<AudioSource>();
            AS.clip = sounds[num];
            AS.Play();
        }   
    }
   
    private void Update()
    {
       
    }
}
