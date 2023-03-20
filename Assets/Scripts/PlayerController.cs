using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody2D _rigit;
    private Animator _anim;
    public GameController gameController;
    public bool grounded = true;
    public Camera _cam;
    public float Speed = 1f;
    public float JumpHeight = 5f;
    private float jumpTime = 0f;
    public Sprite[] items;
    public GameObject Effect;
    private float changeTime = 0.5f;
    private bool extraLifeEnded = false;
    private float extraLifeEndedTime = 0.5f;
    public List<EffectCountdown> activeItemsList;
    public float[] itemsCoutdown;
    public float[] currentItemCoundown;
    private float SpeedChangeVal = 1f;
    public GameObject effectPanel;
    public float shakeAmount = 0.4f;
    public Sprite GroundSprite;
    private bool setGroundSprite = false;
    private int currSpeedLevel = 0;
    private float bombTime = 0;
    private bool gameOver = false;
    void AddItem(Item item) 
    {
        bool onList = false;
        EffectCountdown countdown = null;
        foreach (EffectCountdown effect in activeItemsList) 
        {
            if (effect.itemClass == item.item)
            {
                countdown = effect;
                onList = true;
                break;
            }
        }
        if (!onList) 
        {
            GameObject effect = Instantiate(Effect, new Vector3(380, 820 - 220 * activeItemsList.Count, 0), Quaternion.identity);
            activeItemsList.Add(effect.GetComponent<EffectCountdown>());
            countdown = effect.GetComponent<EffectCountdown>();
            countdown.itemClass = item.item;
            countdown.item = items[(int)item.item];
            effect.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        countdown.time = itemsCoutdown[(int)countdown.itemClass];
        currentItemCoundown[(int)countdown.itemClass] = itemsCoutdown[(int)countdown.itemClass];
    }
    void RemoveItem(int item) 
    {
        for (int i = 0; i < activeItemsList.Count; i++)
        {
            if ((int)activeItemsList[i].itemClass == item) 
            {
                Destroy(activeItemsList[i].gameObject);
                activeItemsList.Remove(activeItemsList[i]);               
                currentItemCoundown[item] = 0;
                for (int j = i; j < activeItemsList.Count; j++) 
                {
                    activeItemsList[j].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(activeItemsList[j].gameObject.GetComponent<RectTransform>().anchoredPosition.x, activeItemsList[j].gameObject.GetComponent<RectTransform>().anchoredPosition.y + 220);
                }
            }
        }
    }
    void Jump()
    {
        if (grounded && jumpTime <= 0 && Time.timeScale != 0)
        {
            _rigit.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            jumpTime = 0.3f;
            grounded = false;
        }
    }
    void Walk()
    {
        _transform.localPosition += new Vector3(SpeedChangeVal * Speed * Time.deltaTime, 0, 0);
        Vector3 screenPos = _cam.WorldToScreenPoint(_transform.position);
        if (Speed < 0)
        {
            _transform.localRotation = new Quaternion(0, 180f, 0, 0);
        }
        else
        {
            _transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigit = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < currentItemCoundown.Length; i++) 
        {
            if (currentItemCoundown[i] > 0)
            {
                currentItemCoundown[i] -= Time.deltaTime;
                if (currentItemCoundown[i] <= 0) 
                {
                    RemoveItem(i);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _rigit.velocity = Vector3.zero;
            _rigit.angularVelocity = 0;
            _rigit.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        }
        if (extraLifeEnded)
        {
            extraLifeEndedTime -= Time.deltaTime;
            if (extraLifeEndedTime <= 0)
            {
                extraLifeEndedTime = 0.5f;
                currentItemCoundown[0] = 0;
                extraLifeEnded = false;
            }
        }

        if (currentItemCoundown[2] > 0)
        {
            JumpHeight = 10f;
        }
        else
        {
            JumpHeight = 5f;
        }
        if (currentItemCoundown[1] > 0)
        {
            _rigit.velocity = Vector3.zero;
            _rigit.angularVelocity = 0;
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * 2.5f);
            Physics2D.IgnoreLayerCollision(8, 9);
            _rigit.gravityScale = 0f;
        }
        else
        {
            _rigit.gravityScale = 1f;
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }
        if (currentItemCoundown[4] > 0)
        {
            effectPanel.SetActive(true);
            effectPanel.GetComponent<Image>().color = new Color32(0x29, 0xFF, 0x00, 100);
            _cam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            effectPanel.SetActive(false);
        }
        if (currentItemCoundown[5] > 0)
        {
            foreach (GameObject platform in GameObject.FindGameObjectsWithTag("ground"))
            {
                if (platform.name != "StartGround")
                    platform.GetComponent<SpriteRenderer>().sprite = null;
            }
            effectPanel.SetActive(true);
            effectPanel.GetComponent<Image>().color = new Color32(255, 255, 255, 140);
            setGroundSprite = true;
        }
        else
        {
            if (setGroundSprite)
            {
                foreach (GameObject platform in GameObject.FindGameObjectsWithTag("ground"))
                {
                    if (platform.name != "StartGround")
                        platform.GetComponent<SpriteRenderer>().sprite = GroundSprite;
                }
                effectPanel.SetActive(false);
                setGroundSprite = false;
            }
        }

        jumpTime -= Time.deltaTime;
        Walk();
        if (Input.touchCount > 0 && Time.timeScale != 0)
        {
            int mov = ZPlayerPrefs.GetInt("reverse")==0 ? 1 : -1;
            foreach (Touch touch in Input.touches)
            {
                if (mov * touch.position.x < mov * Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (MainMenu.tutorial && mov * gameController.tutStep == 1)
                        {
                            gameController.tutStep = 2;
                        }
                        Jump();
                    }
                }
                else if (mov * touch.position.x > mov * Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (MainMenu.tutorial && gameController.tutStep == 0)
                        {
                            gameController.tutStep = 1;
                        }
                        Speed *= -1;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            if (MainMenu.tutorial && gameController.tutStep == 1)
            {
                gameController.tutStep = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            currentItemCoundown[3] = 10f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentItemCoundown[4] = 15f;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentItemCoundown[2] = 10f;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentItemCoundown[5] = 8f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentItemCoundown[1] = 20f;
        }
        if (grounded)
        {
            _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Walk");
        }
        else
        {
            _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Jump");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Speed *= -1;
            if (MainMenu.tutorial && gameController.tutStep == 0)
            {
                gameController.tutStep = 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "destroyer")
        {
            if (currentItemCoundown[0] <= 0 && !extraLifeEnded)
            {
                foreach (EffectCountdown effect in activeItemsList) 
                {
                    Destroy(effect.gameObject);
                }
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("item"))
                {
                    Destroy(item);
                }
                effectPanel.SetActive(false);
                gameController.createItem = false;
                if (!gameOver)
                {
                    gameController.GameOver();
                    gameOver = true;
                }  
            }
            else
            {
                _rigit.velocity = Vector3.zero;
                _rigit.angularVelocity = 0;
                _rigit.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                RemoveItem(0);
                extraLifeEnded = true;
            }
        }
        if (collision.gameObject.tag == "ground")
        {
            float verticalVelocity = _rigit.velocity.y;
            if (verticalVelocity < 0)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else
            {
                collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        if (collision.gameObject.tag == "item")
        {
            GameObject.Find("Sounds").GetComponent<SoundManager>().PlaySound(1);
            AddItem(collision.gameObject.GetComponentInParent<Item>());
            Destroy(collision.transform.parent.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if (gameController.score < collision.gameObject.GetComponent<GroundController>().groundNumber)
            {
                gameController.score = collision.gameObject.GetComponent<GroundController>().groundNumber;
                if (gameController.score / 50 > currSpeedLevel)
                {
                    if (gameController.CameraSpeed < 0.88f)
                    {
                        gameController.CameraSpeed += 0.007f;
                    }
                    if (Mathf.Abs(Speed) < 3.1) 
                    {
                        if (Speed > 0)
                            Speed += 0.02f;
                        else
                            Speed -= 0.02f;
                    }
                    currSpeedLevel++;
                }
            }
            bombTime = 0;
        }
        if (collision.gameObject.tag == "border")
        {
            Speed *= -1;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" && currentItemCoundown[3] > 0 && grounded)
        {
            collision.gameObject.GetComponent<GroundController>().destroy = true;
            bombTime += Time.deltaTime;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" && currentItemCoundown[3] > 0 && bombTime < 0.008f)
        {
            collision.gameObject.GetComponent<GroundController>().destroy = false;
        }
    }
}
