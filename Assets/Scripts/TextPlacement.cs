using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPlacement : MonoBehaviour
{
    public GameObject[] texts;
    public float[] position;
    public RectTransform ParentRect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float height_ = ParentRect.sizeDelta.y;
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(texts[i].GetComponent<RectTransform>().position.x, height_/2*position[i]);
        }
    }
}
