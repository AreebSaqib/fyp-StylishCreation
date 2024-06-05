using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;

public class SamanGame_LevelSelection : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject loadingPanel;
    public GameObject AvtarPanel;
    public GameObject StartPanel;
    public Sprite[] opponentAvatars;
    public Sprite[] PlayerAvatars;
    public GameObject[] Modes;
    public Image fillBar;
    public MRS_Manager[] modesInfo;
    public AudioSource selectionSFX;
    public Image PlayerAvtar;
    public Image OpponentAvtar;
    private int selectedIndex;
    private float timeInterval = 0.2f;
    private float previousTime;
    private bool btnClicked;
    private List<MRS_Manager> mRS_Managers = new List<MRS_Manager>();
    private List<Sprite> avatarList = new List<Sprite>();
    private int startIndex, LoadModeindx;
    private float findDelay,modefindDelay;
    private float nextDealy,modenextDealy;
    public AudioSource opponentSfx;

    public enum LoadLevel
    {
        SamanGame_Mode1, SamanGame_Mode2, SamanGame_Mode3, SamanGame_Mode4, SamanGame_Mode5,
    }
    private LoadLevel loadLevel;

    #region Start
    private void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        AudioListener.volume = 1;
        if (!GameManager.Instance.Initialized)
        {
            SamanGame_SaveLoad.LoadProgress();
            GameManager.Instance.Initialized = true;
        }
        for (int i = 0; i < modesInfo.Length; i++)
        {
            if (modesInfo[i])
                mRS_Managers.Add(modesInfo[i]);
        }
        //GameImages.Instance.PlayerImage.sprite = PlayerAvtar.sprite;
        if(PlayerPrefs.GetInt("LoadMode")<=0)
        {
            PlayerPrefs.SetInt("LoadMode", 0);
        }
        else if(PlayerPrefs.GetInt("LoadMode")>4)
        {
            PlayerPrefs.DeleteKey("LoadMode");
        }
        
    }
    #endregion

    #region RightSeletionBtn
    public void RightSeletionBtn()
    {
        timeInterval = Mathf.Abs(previousTime - Time.time);
        if (!btnClicked)
        {
            btnClicked = true;
            timeInterval = 0.2f;
            previousTime = Time.time;
        }
        mRS_Managers[selectedIndex].MoveTo(new Vector3(-1300, -200, 0), timeInterval, true, false);
        //ModeLabels[selectedIndex].transform.DOMove(new Vector3(-1300, 227, 0), timeInterval);
        selectedIndex++;
        if (selectedIndex >= modesInfo.Length)
        {
            selectedIndex = 0;
        }
        mRS_Managers[selectedIndex].transform.localPosition = new Vector3(1300, -200, 0);
        mRS_Managers[selectedIndex].MoveTo(new Vector3(0, -200, 0), timeInterval, true, false);
        //ModeLabels[selectedIndex].transform.DOMove(new Vector3(0, 227, 0), timeInterval);
        GameManager.Instance.selectedPlayer = selectedIndex;
        if (timeInterval >= 0.2f)
        {
            timeInterval = 0.2f;
            btnClicked = false;
        }
    }
    #endregion


    #region LeftSelectionBtn
    public void LeftSelectionBtn()
    {
        timeInterval = Mathf.Abs(previousTime - Time.time);
        if (!btnClicked)
        {
            btnClicked = true;
            timeInterval = 0.2f;
            previousTime = Time.time;
        }
        mRS_Managers[selectedIndex].MoveTo(new Vector3(1300, -200, 0), timeInterval, true, false);
        //ModeLabels[selectedIndex].transform.DOMove(new Vector3(1300, 227, 0), timeInterval);
        if (selectedIndex == 1)
        {
            selectedIndex = 0;
        }
        else
        {
            selectedIndex--;
            if (selectedIndex <= 0)
            {
                selectedIndex = modesInfo.Length - 1;
            }
        }
        mRS_Managers[selectedIndex].transform.localPosition = new Vector3(-1300, -200, 0);
        mRS_Managers[selectedIndex].MoveTo(new Vector3(0, -200, 0), timeInterval, true, false);
        //ModeLabels[selectedIndex].transform.DOMove(new Vector3(0, 227, 0), timeInterval);
        GameManager.Instance.selectedPlayer = selectedIndex;
        if (timeInterval >= 0.2f)
        {
            timeInterval = 0.2f;
            btnClicked = false;
        }
    }
    #endregion

    #region SelectItem
    public IEnumerator SelectItem()
    {
        yield return new WaitForSeconds(1f);
        uiPanel.SetActive(true);
        loadingPanel.SetActive(true);
        loadLevel = (LoadLevel)LoadModeindx;
        StartCoroutine(Loading(loadLevel.ToString(), 4f));
    }
    #endregion

    #region Back
    public void BackFun()
    {
        loadingPanel.SetActive(true);
        fillBar.fillAmount = 0;
        fillBar.DOFillAmount(1, 3).SetEase(Ease.Linear).OnComplete(delegate
        {
            SceneManager.LoadScene(1);
        });
    }
    #endregion

    #region Menu
    public void Menu()
    {
        StartCoroutine(Loading(Scenes.MyMainMenu.ToString(), 3));
    }
    #endregion

    #region Loading
    IEnumerator Loading(string sceneName, float delay)
    {
        uiPanel.SetActive(false);
        loadingPanel.SetActive(true);
        fillBar.fillAmount = 0;
        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime / delay;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
        yield return null;
    }
    #endregion
    #region FindingOpponent
    IEnumerator SamanUniformGame_FindingOpponent()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < opponentAvatars.Length; i++)
        {
            if (opponentAvatars[i])
                avatarList.Add(opponentAvatars[i]);
        }
        startIndex = Random.Range(1, avatarList.Count);
        if (opponentSfx) opponentSfx.Play();
        while (modefindDelay < 4)
        {
            if (OpponentAvtar && modenextDealy < modefindDelay)
            {
                OpponentAvtar.sprite = avatarList[startIndex];
                PlayerPrefs.SetInt("Opponent", startIndex);
                startIndex++;
                if (startIndex >= avatarList.Count)
                    startIndex = 1;
                modenextDealy += 0.2f;
            }
            yield return null;
            modefindDelay += Time.deltaTime;
        }
        if (opponentSfx) opponentSfx.Stop();
        StartCoroutine(SamanUniCornGame_FindingMode());
        yield return null;
    }
    #endregion

    #region AvtarSelection
    public void AvtarSelection(int i)
    {
        PlayerAvtar.sprite = PlayerAvatars[i];
        PlayerPrefs.SetInt("Player", i);
    }
    #endregion

    #region AvtarSelected
    public  void AvtarSelected()
    {
        
        AvtarPanel.gameObject.SetActive(false);
        StartPanel.gameObject.SetActive(true);
        StartCoroutine(SamanUniformGame_FindingOpponent());
    }
    #endregion
    #region Mode
    IEnumerator SamanUniCornGame_FindingMode()
    {
        yield return new WaitForSeconds(1.5f);
        if (opponentSfx) opponentSfx.Play();
        startIndex = Random.Range(0, Modes.Length - 1);
        while (findDelay < 4)
        {
            if (startIndex<=Modes.Length-1  &&  nextDealy<findDelay)
            {
                for (int i = 0; i <= Modes.Length - 1; i++)
                    Modes[i].gameObject.SetActive(false);
                Modes[startIndex].gameObject.SetActive(true);
                startIndex++;
                if (startIndex >Modes.Length-1)
                    startIndex = 0;
                nextDealy += 0.2f;
            }
            yield return null;
            findDelay += Time.deltaTime;
        }
        if (opponentSfx) opponentSfx.Stop();
        int indx= PlayerPrefs.GetInt("LoadMode");
        for(int j=0;j < Modes.Length;j++)
            Modes[j].gameObject.SetActive(false); 
        Modes[indx].gameObject.SetActive(true);
        LoadModeindx = indx;
        indx++;
        PlayerPrefs.SetInt("LoadMode", indx);
        print(LoadModeindx);
        StartCoroutine(SelectItem());
        yield return null;
    }
    #endregion

}
