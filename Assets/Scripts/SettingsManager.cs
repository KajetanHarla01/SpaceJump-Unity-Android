using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle reverse;
    public Toggle music;
    public Toggle sound;

    // Start is called before the first frame update
    void Start()
    {
        ZPlayerPrefs.Initialize("9pe4GExr", "D84Dk344_dfs");
        reverse.isOn = ZPlayerPrefs.GetInt("reverse") == 1;
        music.isOn = ZPlayerPrefs.HasKey("music") ? ZPlayerPrefs.GetInt("music") == 1 : true;
        sound.isOn = ZPlayerPrefs.HasKey("sound") ? ZPlayerPrefs.GetInt("sound") == 1 : true;
    }
    // Update is called once per frame
    void Update()
    {
        int res = reverse.isOn ? 1 : 0;
        int mu = music.isOn ? 1 : 0;
        int so = sound.isOn ? 1 : 0;
        ZPlayerPrefs.SetInt("reverse", res);
        ZPlayerPrefs.SetInt("music", mu);
        ZPlayerPrefs.SetInt("sound", so);
        ZPlayerPrefs.Save();
        if (mu == 1)
        {
            MusicManager.playMusic = true;
        }
        else
        {
            MusicManager.playMusic = false;
        }
    }
}
