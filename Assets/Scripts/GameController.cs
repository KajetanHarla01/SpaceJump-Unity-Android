using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;

public class GameController : MonoBehaviour
{
    public static int floor = 1;
    public GameObject[] Borders;
    public GameObject Ground;
    public GameObject Player;
    public Transform CameraObj;
    public Transform Creator;
    public GameObject GameOverObject;
    public GameObject Item;
    public Text GameOverText;
    public Text ScoreText;
    public int score = 0;
    public float CameraSpeed = 1f;
    public bool createItem = true;
    float y = 1.4f;
    float prev = 10;
    bool add = true;
    private float itemTime;
    public SpriteRenderer nightSky;
    public Gradient colorGradient;
    private float fixedScore;
    private float currScore = 0;
    public GameObject moon;
    public GameObject mars;
    public GameObject jupiter;
    public GameObject saturn;
    public GameObject uranus;
    public GameObject neptun;
    private bool cmMove = false;
    private float widthFactor = 1080.0f/Screen.width;
    public int tutStep = 0;
    public GameObject tutLeftPanel;
    public GameObject tutRightPanel;
    public GameObject tutInfoPanel;
    public GameObject PausePanel;
    public int[] achievementsLevels;
    public string[] achievementsKeys;
    public int achievementUnlocked;

    float AddGround (ref float y, ref float prev){
        GroundController GC = GameObject.Instantiate(Ground, new Vector2(0, y), Quaternion.identity).GetComponent<GroundController>();
        GC.groundSize = 0.16f - (fixedScore / 40000);
        float rand = 0;
        int i = 0;
        do
        {
            i++;
            rand = Random.Range(Borders[0].transform.position.x, Borders[1].transform.position.x);
            GC.transform.SetPositionAndRotation(new Vector2(rand, y), Quaternion.identity);
        } while (Mathf.Abs(prev - rand) < 0.5f && prev != 10 || Borders[0].transform.position.x < GC.checkers[0].transform.position.x || Borders[1].transform.position.x > GC.checkers[1].transform.position.x);
        GC.groundNumber = floor;
        floor++;
        prev = rand;
        y += 0.9f;
        return rand;
    }
    public void ConfirmTut()
    {
        tutStep = 3;
    }
    // Start is called before the first frame update
    void Start()
    {
        ZPlayerPrefs.Initialize("9pe4GExr", "D84Dk344_dfs");
        while (y < 15) {
            AddGround(ref y, ref prev);
        }
        Creator = GameObject.Find("Creator").GetComponent<Transform>();
        itemTime = Random.Range(25, 40);
    }

    // Update is called once per frame
    void Update()
    {
        if (MainMenu.tutorial)
        {
            if (tutStep == 0)
            {
                tutLeftPanel.SetActive(false);
                tutRightPanel.SetActive(true);
                tutInfoPanel.SetActive(false);
            }
            else if (tutStep == 1)
            {
                tutLeftPanel.SetActive(true);
                tutRightPanel.SetActive(false);
                tutInfoPanel.SetActive(false);
            }
            else if (tutStep == 2)
            {
                tutLeftPanel.SetActive(false);
                tutRightPanel.SetActive(false);
                tutInfoPanel.SetActive(true);
            }
            else
            {
                tutLeftPanel.SetActive(false);
                tutRightPanel.SetActive(false);
                tutInfoPanel.SetActive(false);
                ZPlayerPrefs.SetInt("tut", 1);
                MainMenu.tutorial = false;
            }
        }
        if (fixedScore >= currScore)
        {
            currScore = fixedScore;
        }
        if (Player)
            fixedScore = ((Player.transform.position.y - 1.4f) / 0.9f) + 1;
        if (fixedScore >= 100 && fixedScore <= 400)
        {
            CameraObj.gameObject.GetComponentInChildren<Camera>().backgroundColor = colorGradient.Evaluate((fixedScore - 100) / 300);
            if (fixedScore >= 280) 
            {
                nightSky.color = new Color(nightSky.color.r, nightSky.color.g, nightSky.color.b, ((float)(fixedScore - 280) / 120));
            }
        }
        if (fixedScore >= 500) 
        {
            moon.transform.localPosition = new Vector3(0.14f, 7.5f - ((float)(currScore - 500) / ((float)250 / 15)), 10);
        }
        if (fixedScore >= 950)
        {
            mars.transform.localPosition = new Vector3(0, 7.8f - ((float)(currScore - 950) / (340 / 15.6f)), 10);
        }
        if (fixedScore >= 1500)
        {
            jupiter.transform.localPosition = new Vector3(0, 7.8f - ((float)(currScore - 1500) / (420 / 15.6f)), 10);
        }
        if (fixedScore >= 2200)
        {
            saturn.transform.localPosition = new Vector3(0, 7.8f - ((float)(currScore - 2200) / (400 / 15.6f)), 10);
        }
        if (fixedScore >= 2850)
        {
            uranus.transform.localPosition = new Vector3(0, 7.8f - ((float)(currScore - 2850) / (370 / 15.6f)), 10);
        }
        if (fixedScore >= 3450)
        {
            neptun.transform.localPosition = new Vector3(0, 7.8f - ((float)(currScore - 3450) / (370 / 15.6f)), 10);
        }

        if (fixedScore >= achievementsLevels[achievementUnlocked])
        {
            ZPlayerPrefs.SetInt("achievement"+achievementUnlocked, 1);
            Social.ReportProgress(achievementsKeys[achievementUnlocked], 100.0f, (bool success) => {});
            achievementUnlocked++;
        }

        if (Player && Player.transform.position.y > 5.21f)
            itemTime -= Time.deltaTime;
        if (itemTime <= 0 && createItem)
        {
            int count = 1;
            float prev = 10f;
            int rand = Random.Range(1, 101);
            if (rand > 90)
            {
                count = 3;
            }
            else if (rand > 55) 
            {
                count = 2;
            }
            for (int i = 0; i < count; i++) 
            {
                Item it = Instantiate(Item, new Vector2(0, 0), Quaternion.identity).GetComponent<Item>();
                float _rand = 0;
                do
                {
                    _rand = Random.Range(Borders[0].transform.position.x, Borders[1].transform.position.x);
                    it.transform.SetPositionAndRotation(new Vector2(_rand, CameraObj.transform.position.y + 5.4f + Random.Range(0f, 1f)), Quaternion.identity);
                } while (Mathf.Abs(prev - _rand) < 1f || Borders[0].transform.position.x-0.823f < it.checkers[0].transform.position.x || Borders[1].transform.position.x+0.823f > it.checkers[1].transform.position.x);
                it.speed = 1.5f + (score / 700.0f) + Random.Range(-0.2f, 0.2f);
                prev = _rand;
            }
            itemTime = Random.Range(20, 27);
        }
        ScoreText.text = score.ToString();
        if (Player && (Player.transform.position.y > 5.21f || cmMove))
        {
            if (Player.transform.position.y > CameraObj.transform.position.y)
            {
                CameraObj.position = new Vector3(CameraObj.transform.position.x, Player.transform.position.y, CameraObj.transform.position.z);
            }
            else
            {
                CameraObj.position += new Vector3(0, Time.deltaTime * CameraSpeed, 0);
                cmMove = true;
            }
        }
        if (y <= Creator.transform.position.y)
        {
            if (add)
            {
                AddGround(ref y, ref prev);
                add = false;
            }
        }
        else {
            add = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOverObject.activeSelf)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        ZPlayerPrefs.Save();
    }
    public void GameOver()
    {
        GameObject.Find("Sounds").GetComponent<SoundManager>().PlaySound(2);
        GameOverObject.SetActive(true);
        Destroy(Player);
        if (score > ZPlayerPrefs.GetInt("highScore"))
        {
            ZPlayerPrefs.SetInt("highScore", score);
        }
        int bestScore = ZPlayerPrefs.GetInt("highScore");
        GameOverText.text = "Wynik: " + score + "\n" + "Najwyższy wynik: " + bestScore;
        GameObject.FindGameObjectWithTag("playStoreServices").GetComponent<Playgamesservices>().updateHighScore();
        GameObject.FindGameObjectWithTag("playStoreServices").GetComponent<Playgamesservices>().updateGamesPlayed();
        foreach (GameObject game in GameObject.FindGameObjectsWithTag("ground"))
        {
            Destroy(game);
        }
    }
    public void Restart()
    {
        floor = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadScene(int scene)
    {
        floor = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }
}