using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneVoid(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
