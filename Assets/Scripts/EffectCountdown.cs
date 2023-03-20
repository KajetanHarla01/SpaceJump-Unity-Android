using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectCountdown : MonoBehaviour
{
    public float time = 30f;
    private float curr_time;
    public Sprite item;
    public itemClass itemClass;
    // Start is called before the first frame update
    void Start()
    {
        curr_time = time;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        GetComponentsInChildren<Image>()[1].fillAmount = (curr_time - time) / curr_time;
        GetComponentsInChildren<Image>()[2].sprite = item;
    }
}
