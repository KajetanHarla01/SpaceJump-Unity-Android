using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool tutorial = false;
    // Update is called once per frame
    private void Start()
    {
        if (!ZPlayerPrefs.HasKey("tut") || ZPlayerPrefs.GetInt("tut") == 0)
        {       
            tutorial = true;
        }
        GameObject.FindGameObjectWithTag("playStoreServices").GetComponent<Playgamesservices>().updateHighScore();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ZPlayerPrefs.DeleteAll();
            ZPlayerPrefs.Save();
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
