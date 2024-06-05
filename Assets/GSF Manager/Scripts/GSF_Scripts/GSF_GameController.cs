using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

#region GamePlay_Sounds
[System.Serializable]
public class GamePlay_Sounds
{
    public AudioSource bgMusic;
    public AudioSource defaultBtnSFX;
    public AudioSource winSFX;
    public AudioSource loseSFX;
    public AudioSource gameCompleteSFX;
    public AudioSource countDownSFX;
}
#endregion

#region Gameplay_UI
[System.Serializable]
public class Gameplay_UI
{
    public GameObject pausePanel, levelCompletePanel, gameCompletePanel, levelFailPanel, loadingPanel, instructionPanel;
    public GameObject pauseBtn;
    public Text gameLevel, levelTime, totalCoins, totalKills, startInstruction, loadProgressText;
    public Text levelCompleteCoins, levelFailedCoins, gameCompleteCoins;
    public Image loadingBarFill;
}
#endregion

#region Gameplay_Level_Props
[System.Serializable]
public class Gameplay_Level_Props
{
    public GameObject gameLevel;
    public Transform spawnPoint;
    [Range(0, 50000)]
    public int winReward;
    [Range(0, 50000)]
    public int loseReward;
    public bool isKillingIncluded;
    [ShowIf("isKillingIncluded")]
    [Range(0, 100)]
    public int targetsToKill;
    [ShowIf("isKillingIncluded")]
    [Range(0, 50000)]
    public int perKillReward;
    public bool isObjecttiveIncluded;
    [ShowIf("isObjecttiveIncluded")]
    [Range(0, 100)]
    public int totalObjectives;
    [ShowIf("isObjecttiveIncluded")]
    [Range(0, 50000)]
    public int perObjectiveReward;
    public bool isTimeBased;
    [ShowIf("isTimeBased")]
    [Range(0, 500)]
    public int timeInSeconds;
    [ShowIf("isTimeBased")]
    [Range(0, 500)]
    public int timeReward;
    [ShowIf("isTimeBased")]
    public bool failOnTimeUp;
    public bool showInstruction;
    [ShowIf("showInstruction")]
    [Title("Level Instruction", bold: false)]
    [HideLabel]
    [MultiLineProperty(4)]
    public string levelInstruction;
}
#endregion

#region Gameplay_PlayerProps
[System.Serializable]
public class Gameplay_PlayerProps
{
    public GameObject player;
    public GameObject playerControls;
    public GameObject playerCamera;
    public Texture[] playerTextures;
}
#endregion

public class GSF_GameController : MonoBehaviour
{
    #region Variables
    [FoldoutGroup("GamePlay Sounds")]
    [HideLabel]
    public GamePlay_Sounds soundProps;
    [FoldoutGroup("Gameplay UI")]
    [HideLabel]
    public Gameplay_UI gameplay_UI;
    public Gameplay_PlayerProps[] playerProps;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public Gameplay_Level_Props[] gameplay_Level_Props;

    public bool isDebug;
    [ShowIf("isDebug")]
    public int playLevel, currentPlayer, currentTexture;

    [Header("Scene Selection")]
    public Scenes PreviousScene;
    public Scenes NextScene;

    [Header("Level End Delay")]
    public float GameWinDelay;
    public float GameLooseDelay;

    [Header("Ad Sequence ID")]
    public int SequenceID = 3;

    private int totalMinutes;
    private int totalSeconds;
    private string levelTime, _Minutes, _Seconds, _Zero = "0", _Colon = " : ";
    private bool timerPaused = false;
    private int currentLevel, totalReward = 0;
    #endregion

    #region Awake
    private void Awake()
    {
        GSF_AdsManager.HideBanner();
    }
    #endregion

    #region Start
    void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        GameManager.Instance.totalKills = 0;
        GameManager.Instance.objectivesAchieved = 0;
        GameManager.Instance.stopPlaying = false;
        GameManager.Instance.showStats = false;
        gameplay_UI.pauseBtn.SetActive(false);
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
           SamanGame_SaveLoad.LoadProgress();
        }
        if (isDebug)
        {
            GameManager.Instance.selectedLevel = playLevel - 1;
            GameManager.Instance.selectedPlayer = currentPlayer - 1;
            GameManager.Instance.selectedTexture = currentTexture - 1;
        }
        currentLevel = GameManager.Instance.selectedLevel;
        currentPlayer = GameManager.Instance.selectedPlayer;      
        gameplay_UI.gameLevel.text = (currentLevel + 1).ToString();
        gameplay_UI.totalCoins.text = SaveData.Instance.Coins.ToString();
        if (gameplay_Level_Props.Length > 0)
        {
            totalMinutes = gameplay_Level_Props[currentLevel].timeInSeconds / 60;
            totalSeconds = gameplay_Level_Props[currentLevel].timeInSeconds % 60;
            SetTime();
            if (gameplay_Level_Props[currentLevel].showInstruction)
            {
                StartCoroutine(ShowStartInstruction());
            }
            else if (gameplay_Level_Props[currentLevel].isTimeBased)
            {
                StartCoroutine(TimeManager());
            }
            else
            {
                SetInitialProps();
            }
            if (!gameplay_Level_Props[currentLevel].isTimeBased)
            {
                gameplay_UI.levelTime.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        GamePlaySettings();
        GSF_AdsManager.ShowBanner(SequenceID, "Game Play");
        LogEvents.Instance.Event("gameplay_" + (currentLevel + 1) + "_started", "gameplay_" + (currentLevel + 1) + "_started", "started");
    }
    #endregion

    #region GamePlaySettings
    private void GamePlaySettings()
    {
        if (SaveData.Instance.isRightControls)
        {
            // You can write your logic to activate the controls
        }
        else if (!SaveData.Instance.isRightControls)
        {
            // You can write your logic to activate the controls
        }
        if (!SaveData.Instance.isMusic)
        {
            soundProps.bgMusic.enabled = false;
        }
        if (!SaveData.Instance.isSound)
        {
            soundProps.countDownSFX.enabled = false;
            soundProps.defaultBtnSFX.enabled = false;
            soundProps.gameCompleteSFX.enabled = false;
            soundProps.loseSFX.enabled = false;
            soundProps.winSFX.enabled = false;
        }
    }
    #endregion

    #region ShowStartInstruction
    IEnumerator ShowStartInstruction()
    {
        Time.timeScale = 0;
        gameplay_UI.instructionPanel.SetActive(true);
        gameplay_UI.startInstruction.text = "";
        for (int i = 0; i < gameplay_Level_Props[0].levelInstruction.Length; i++)
        {
            gameplay_UI.startInstruction.text += gameplay_Level_Props[0].levelInstruction[i];
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    #endregion

    #region SetInitialProps
    private void SetInitialProps()
    {
        gameplay_UI.pauseBtn.SetActive(true);
        for (int i=0;i< gameplay_Level_Props.Length; i++)
        {
            if(i == currentLevel)
            {
                gameplay_Level_Props[currentLevel].gameLevel.SetActive(true);
            }
            else
            {
                Destroy(gameplay_Level_Props[i].gameLevel);
            }
        }
        if(playerProps.Length > 0)
        {
            for(int i=0;i< playerProps.Length; i++)
            {
                if( i == currentPlayer)
                {
                    if (playerProps[i].playerCamera)
                    {
                        playerProps[i].playerCamera.SetActive(true);
                        playerProps[i].playerCamera.GetComponentInChildren<Camera>().enabled = true;
                    }
                    if (playerProps[i].playerControls)
                    {
                        playerProps[i].playerControls.SetActive(true);
                    }
                    playerProps[currentPlayer].player.transform.position = gameplay_Level_Props[currentLevel].spawnPoint.position;
                    playerProps[currentPlayer].player.transform.rotation = gameplay_Level_Props[currentLevel].spawnPoint.rotation;
                    playerProps[currentPlayer].player.SetActive(true);
                    if (playerProps[currentPlayer].playerTextures.Length > 0)
                    {
                        SetMaterials();
                    }
                }
                else
                {
                    Destroy(playerProps[i].player);
                }
            }
        }
    }
    #endregion

    #region SetMaterials
    private void SetMaterials()
    {
        Helper.SetTextures(playerProps[currentPlayer].player, playerProps[currentPlayer].playerTextures[GameManager.Instance.selectedTexture]);
    }
    #endregion

    #region StartPlaying
    public void StartPlaying()
    {
        Time.timeScale = 1;
        gameplay_UI.instructionPanel.SetActive(false);
        if (gameplay_Level_Props[currentLevel].isTimeBased)
        {
            StartCoroutine(TimeManager());
        }
    }
    #endregion

    #region TimeManager
    IEnumerator TimeManager()
    {
        while((totalMinutes > 0 || totalSeconds > 0) && !GameManager.Instance.showStats)
        {
            yield return new WaitForSecondsRealtime(1f);
            if (!timerPaused)
            {
                soundProps.countDownSFX.Play();
                totalSeconds--;
                SetTime();
            }    
        }
        if(totalMinutes <=0 && totalSeconds <=0 && gameplay_Level_Props[currentLevel].failOnTimeUp)
        {
            GameManager.Instance.showStats = true;
            LevelFailed();
        }
    }
    private void SetTime()
    {
        if(totalSeconds == 0 && totalMinutes > 0)
        {
            totalMinutes -= 1;
            totalSeconds = 60;
        }
        _Minutes = totalMinutes.ToString();
        _Seconds = totalSeconds.ToString();
        if(totalMinutes < 10)
        {
            _Minutes = _Zero + totalMinutes;
        }
        if (totalSeconds < 10)
        {
            _Seconds = _Zero + totalSeconds;
        }
        else if(totalSeconds == 60)
        {
            _Seconds = _Zero +_Zero;

        }
        levelTime = _Minutes + _Colon + _Seconds;
        gameplay_UI.levelTime.text = levelTime;
    }
    #endregion

    #region CheckLevelStatus
    public void CheckLevelStatus()
    {

    }
    #endregion

    #region LevelComplete
    public void LevelComplete()
    {
        if (playerProps[currentPlayer].playerControls)
        {
            playerProps[currentPlayer].playerControls.SetActive(false);
        }
        SaveData.Instance.Coins += gameplay_Level_Props[currentLevel].winReward;
        totalReward += gameplay_Level_Props[currentLevel].winReward;
        CalculateRewardAmount();
        StartCoroutine(LevelCompleteAction());
    }
    IEnumerator LevelCompleteAction()
    {
        yield return new WaitForSeconds(GameWinDelay);
        soundProps.bgMusic.Stop();
        soundProps.winSFX.Play();
        if (currentLevel == gameplay_Level_Props.Length - 1)
        {
            gameplay_UI.gameCompletePanel.SetActive(true);
            LogEvents.Instance.Event("gameplay_completed", "gameplay_completed", "started");
        }
        else
        {
            if(GameManager.Instance.selectedLevel + 1 == SaveData.Instance.LevelsUnlocked)
            {
                SaveData.Instance.LevelsUnlocked++;
            }
            gameplay_UI.levelCompletePanel.SetActive(true);
            LogEvents.Instance.Event("gameplay_" + (currentLevel + 1) + "_completed", "gameplay_" + (currentLevel + 1) + "_completed", "started");
        }
        gameplay_UI.totalCoins.text = SaveData.Instance.Coins.ToString();
       SamanGame_SaveLoad.SaveProgress();
        Time.timeScale = 0;
        ShowAds(SequenceID);
    }
    #endregion

    #region LevelFailed
    public void LevelFailed()
    {
        CalculateRewardAmount();
        SaveData.Instance.Coins += gameplay_Level_Props[currentLevel].loseReward;
        // You can comment if you don't wanna give any of these rewards
        gameplay_Level_Props[currentLevel].timeReward = 0;
        gameplay_Level_Props[currentLevel].perKillReward = 0;
        gameplay_Level_Props[currentLevel].perObjectiveReward = 0;
        if (playerProps[currentPlayer].playerControls)
        {
            playerProps[currentPlayer].playerControls.SetActive(false);
        }
        StartCoroutine(LevelFailedAction());
    }
    IEnumerator LevelFailedAction()
    {
        yield return new WaitForSeconds(GameLooseDelay);
        soundProps.bgMusic.Stop();
        soundProps.loseSFX.Play();
        gameplay_UI.levelFailPanel.SetActive(true);
        gameplay_UI.totalCoins.text = SaveData.Instance.Coins.ToString();
        Time.timeScale = 0;
        ShowAds(SequenceID);
        LogEvents.Instance.Event("gameplay_" + (currentLevel + 1) + "_failed", "gameplay_" + (currentLevel + 1) + "_failed", "completed");
    }
    #endregion

    #region CalculateRewardAmount
    void CalculateRewardAmount()
    {
        float timeReward = 0, objectiveReward = 0, killReward = 0;
        if (gameplay_Level_Props[currentLevel].isTimeBased && gameplay_Level_Props[currentLevel].timeInSeconds > 0)
        {
            float remainingSec = ((totalMinutes * 60) + totalSeconds - 1);
            float timePercentage = (remainingSec / (gameplay_Level_Props[currentLevel].timeInSeconds)) * 100;
            timeReward = Mathf.Floor((gameplay_Level_Props[currentLevel].timeReward * timePercentage)/ 100);
        }
        if (gameplay_Level_Props[currentLevel].isKillingIncluded)
        {
            killReward = GameManager.Instance.totalKills * gameplay_Level_Props[currentLevel].perKillReward;
        }
        if (gameplay_Level_Props[currentLevel].isObjecttiveIncluded)
        {
            objectiveReward = GameManager.Instance.objectivesAchieved * gameplay_Level_Props[currentLevel].perObjectiveReward;
        }
        SaveData.Instance.Coins += (int)(timeReward + objectiveReward + killReward);
        totalReward += (int)(timeReward + objectiveReward + killReward);
        gameplay_UI.levelCompleteCoins.text = (gameplay_Level_Props[currentLevel].winReward + timeReward + objectiveReward + killReward).ToString();
        gameplay_UI.gameCompleteCoins.text = (gameplay_Level_Props[currentLevel].winReward + timeReward + objectiveReward + killReward).ToString();
       SamanGame_SaveLoad.SaveProgress();
    }
    #endregion

    #region DoubleYourReward
    public void DoubleYourReward()
    {
        SaveData.Instance.Coins += totalReward;
       SamanGame_SaveLoad.SaveProgress();
        gameplay_UI.totalCoins.text = SaveData.Instance.Coins.ToString();
        gameplay_UI.levelCompleteCoins.text = (totalReward * 2).ToString();
    }
    #endregion

    #region ShowAds
    void ShowAds(int id)
    {
        GSF_AdsManager.ShowInterstitial(id, "Game Play - Level : " + (currentLevel + 1));
    }
    #endregion

    #region PauseGame
    public void PauseGame()
    {
        timerPaused = !timerPaused;
        ShowAds(SequenceID);
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        gameplay_UI.pausePanel.SetActive(true);
    }
    #endregion

    #region ResumeGame
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        gameplay_UI.pausePanel.SetActive(false);
        timerPaused = !timerPaused;

    }
    #endregion

    #region RetryLevel
    public void RetryLevel()
    {
        gameplay_UI.loadingPanel.SetActive(true);
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
        LogEvents.Instance.Event("gameplay_to_replay_level", "gameplay_to_replay_level", "completed");
    }
    #endregion

    #region NextLevel
    public void NextLevel()
    {
        if(GameManager.Instance.selectedLevel < gameplay_Level_Props.Length - 1)
        {
            GameManager.Instance.selectedLevel += 1;
        }
        gameplay_UI.loadingPanel.SetActive(true);
        StartCoroutine(LoadScene(NextScene.ToString()));
        LogEvents.Instance.Event("gameplay_to_next_level", "gameplay_to_next_level", "completed");
    }
    #endregion

    #region MainMenu
    public void MainMenu()
    {
        gameplay_UI.loadingPanel.SetActive(true);
        StartCoroutine(LoadScene(PreviousScene.ToString()));
        LogEvents.Instance.Event("gameplay_to_main_menu", "gameplay_to_main_menu", "completed");
    }
    #endregion

    #region LoadScene
    IEnumerator LoadScene(string levelName)
    {
        gameplay_UI.pausePanel.SetActive(false);
        gameplay_UI.levelCompletePanel.SetActive(false);
        gameplay_UI.levelFailPanel.SetActive(false);
        gameplay_UI.gameCompletePanel.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName);
        while (!asyncOperation.isDone)
        {
            gameplay_UI.loadProgressText.text =Mathf.Floor(asyncOperation.progress * 100) + "%";
            gameplay_UI.loadingBarFill.fillAmount = (asyncOperation.progress * 100);
            if (asyncOperation.progress >= 0.9f)
            {
                gameplay_UI.loadProgressText.text = "100%";
                gameplay_UI.loadingBarFill.fillAmount = 1f;    
            }
            yield return null;
        }
    }
    #endregion

}