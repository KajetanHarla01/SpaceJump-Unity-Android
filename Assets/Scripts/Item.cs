using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemClass { secoundLife = 0,  fly = 1, highJump = 2, bomb = 3, poison = 4, groundResizing = 5 };

public class Item : MonoBehaviour
{
    public float speed = 1.5f;
    public itemClass item;
    public Sprite[] items;
    public GameObject[] checkers;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(1, 101);
        if (rand < 10)
        {
            item = itemClass.secoundLife;
            gameObject.GetComponent<SpriteRenderer>().sprite = items[0];
            gameObject.GetComponentsInChildren<EdgeCollider2D>()[0].enabled = true;
        }
        else if (rand < 30)
        {
            item = itemClass.fly;
            gameObject.GetComponent<SpriteRenderer>().sprite = items[1];
            gameObject.GetComponentsInChildren<EdgeCollider2D>()[1].enabled = true;
        }
        else if (rand < 50)
        {
            item = itemClass.highJump;
            gameObject.GetComponent<SpriteRenderer>().sprite = items[2];
            gameObject.GetComponentsInChildren<EdgeCollider2D>()[2].enabled = true;
        }
        else if (rand < 70)
        {
            item = itemClass.bomb;
            gameObject.GetComponent<SpriteRenderer>().sprite = items[3];
            gameObject.GetComponentsInChildren<EdgeCollider2D>()[3].enabled = true;
        }
        else if (rand < 90)
        {
            item = itemClass.poison;
            gameObject.GetComponent<SpriteRenderer>().sprite = items[4];
            gameObject.GetComponentsInChildren<EdgeCollider2D>()[4].enabled = true;
        }
        else
        {
            item = itemClass.groundResizing;
            gameObject.GetComponent<SpriteRenderer>().sprite = items[5];
            gameObject.GetComponentsInChildren<EdgeCollider2D>()[5].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}
