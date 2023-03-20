using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.UI;
using System;
using UnityEngine.SocialPlatforms;

public class Playgamesservices : MonoBehaviour
{ 
    public Text debugtext;
    public Text datatocloud;

    public int achCount = 7;
    public string[] achKeys;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        ZPlayerPrefs.Initialize("9pe4GExr", "D84Dk344_dfs");
        Initialize();

        int highscore = 0;

        PlayGamesPlatform.Instance.LoadScores(
            "CgkI78jaocISEAIQAg",
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
        (LeaderboardScoreData data) => {
            highscore = (int)data.PlayerScore.value;
        });

        if (!ZPlayerPrefs.HasKey("highScore"))
        {
            ZPlayerPrefs.SetInt("highScore", highscore);
        }
        if (!ZPlayerPrefs.HasKey("addPlayedGames"))
        {
            ZPlayerPrefs.SetInt("addPlayedGames", 0);
        }
        else if (ZPlayerPrefs.GetInt("addPlayedGames") != 0)
        {
            updateGamesPlayed(true);
        }
        for (int i = 0; i < achCount; i++)
        {
            if (ZPlayerPrefs.GetInt("achievement" + i) == 1)
            {
                Social.ReportProgress(achKeys[i], 100.0f, (bool success) => { });
            }
        }
        ZPlayerPrefs.Save();
        updateHighScore();
    }
    void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false).
            EnableSavedGames().
            Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        signinuserwithplaygames();
    }
    void signinuserwithplaygames()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (success)=>{});
    }
    public void showleaderboard()
    {
        Social.ShowLeaderboardUI();
    }
    public void showacievementui()
    {
        Social.ShowAchievementsUI();
    }
    public void updateHighScore()
    {
        Social.ReportScore(ZPlayerPrefs.GetInt("highScore"), "CgkI78jaocISEAIQAg", (bool success) => { });
    }
    public void updateGamesPlayed(bool mainMenu = false)
    {
        int n = mainMenu ? -1 : 0;
        string[] achivements = { "CgkI78jaocISEAIQDA", "CgkI78jaocISEAIQDQ", "CgkI78jaocISEAIQDg", "CgkI78jaocISEAIQDw", "CgkI78jaocISEAIQEA", "CgkI78jaocISEAIQEQ" };
        foreach (string a in achivements)
        {
            bool exit = false;
            PlayGamesPlatform.Instance.IncrementAchievement(a, ZPlayerPrefs.GetInt("addPlayedGames") + 1 + n, ok =>
            {
                if (!ok)
                {
                    ZPlayerPrefs.SetInt("addPlayedGames", ZPlayerPrefs.GetInt("addPlayedGames") + 1 + n);
                    exit = true;
                }
                else
                {
                    ZPlayerPrefs.SetInt("addPlayedGames", 0);
                }
                ZPlayerPrefs.Save();
            });
            if (exit) break;
        }
    }
    /*
    public void postscoretoleaderboard()
    {
        //Social.ReportScore(int.Parse(leaderboard.text), "CgkI78jaocISEAIQAg", (bool success) => {});
    }

    public void achievementcompleted()
    {
        // Social.ReportProgress("CgkI78jaocISEAIQAw", 100.0f, (bool success) => {});
    }

    //cloud saving
    private bool issaving = false;
    private string SAVE_NAME = "savegames";
    public void opensavetocloud(bool saving)
    {
        if (Social.localUser.authenticated)
        {
             issaving = saving;
             ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution
             (SAVE_NAME, GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork,
             ConflictResolutionStrategy.UseLongestPlaytime, savedgameopen);
        }
    }
    private void savedgameopen(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (issaving)//if is saving is true we are saving our data to cloud
            {
                // debugtext.text = "hello in save2";
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetDataToStoreinCloud());
                SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, saveupdate);
            }
            else//if is saving is false we are opening our saved data from cloud
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, ReadDataFromCloud);
            }
        }
    }
    private void ReadDataFromCloud(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string savedata = System.Text.ASCIIEncoding.ASCII.GetString(data);
            LoadDataFromCloudToOurGame(savedata);
        }
    }
    private void LoadDataFromCloudToOurGame(string savedata)
    {
        string[] data = savedata.Split('|');
        debugtext.text = data[0].ToString();
    }
    private void saveupdate(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        //use this to debug whether the game is uploaded to cloud
        debugtext.text = "successfully add data to cloud";
    }
    private string GetDataToStoreinCloud()//  we seting the value that we are going to store the data in cloud
    { 
        string Data = "";
        //data [0]
        Data += datatocloud.text.ToString();
        Data += "|";
        return Data;
    }
    */
}
