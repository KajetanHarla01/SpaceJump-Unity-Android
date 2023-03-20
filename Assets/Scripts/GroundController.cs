using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private BoxCollider2D _collider;
    public GameObject Player;
    public GameObject[] checkers;
    public Transform Camera;
    public int groundNumber;
    public bool destroy = false;
    public float groundSize;
    private float destroyTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        Player = GameObject.Find("Player");
        Camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player && Player.GetComponentsInChildren<Transform>()[1].transform.position.y < gameObject.transform.position.y)
        {
            _collider.isTrigger = true;
        }
        else
        {
            _collider.isTrigger = false;
        }
        if (gameObject.transform.position.y < Camera.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
        if (destroy)
        {
            destroyTime -= Time.deltaTime;
            if (destroyTime < 0)
            {
                Destroy(gameObject);
            }
        }
        if (gameObject.name != "StartGround")
            gameObject.transform.localScale = new Vector3(groundSize, gameObject.transform.localScale.y);
    }
}
