using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerGO : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AudioSource AS in gameObject.GetComponents<AudioSource>())
        {
            if (!AS.isPlaying)
            {
                Destroy(AS);
            }
        }
    }
}
