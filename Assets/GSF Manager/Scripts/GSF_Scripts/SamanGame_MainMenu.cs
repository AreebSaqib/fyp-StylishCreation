using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;

#region MainMenu_UI
[System.Serializable]
public class MainMenu_UI
{
    public Image profileImg, ReplaceableAvatar, fillBar;
    public Text profileName, inputName, totalCoins;
    public GameObject storeBtn;
    [Header("UI Panels")]
    public GameObject profilePanel;
    public GameObject uiPanel;
    public GameObject menuPanel;
    public GameObject ExitDialogue;
    public GameObject loadingPanel;
}
#endregion

#region Settings_Props
[System.Serializable]
public class Settings_Props
{
    public GameObject soundOn, soundOff;
    public GameObject musicOn, musicOff;
    public GameObject vibrationOn, vibrationOff;
    public GameObject leftControlOn, leftControlOff;
    public GameObject rightControlOn, rightControlOff;
}
#endregion

public class SamanGame_MainMenu : MonoBehaviour
{
    #region Variables
    public Sprite[] playerAvatars;
    [FoldoutGroup("Settings Props")]
    [HideLabel]
    public Settings_Props settingsProps;
    [FoldoutGroup("MainMenu UI")]
    [HideLabel]
    public MainMenu_UI menu_UI;
    public Scenes NextScene;
    public float loadTime = 4f;
    #endregion

    #region Start
    void Start ()
    {
        Time.timeScale = 1;
		AudioListener.pause = false;
        AudioListener.volume = 1;
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            SamanGame_SaveLoad.LoadProgress();
        }
        Input.multiTouchEnabled = false;
    }
    #endregion

    #region RemoveAds
    public void RemoveAds ()
    {
        Helper.RemoveAds();
        SamanGame_SaveLoad.SaveProgress();
	}
    #endregion

    #region RestorePurchases
    public void RestorePurchases () {
        GSF_InAppController.Instance.RestoreButtonClick();
	}
    #endregion

    #region ShowRateUs
    public void RateUs ()
    {
        //if (ConsoliAds.Instance != null)
        //{
        //    Application.OpenURL(ConsoliAds.Instance.rateUsURL);
        //}
    }
    #endregion

    #region Exit
    public void Exit () {
		Application.Quit ();
	}
    #endregion

    #region ResetSaveData
    public void ResetSaveData () {
		SamanGame_SaveLoad.DeleteProgress ();
		SamanGame_SaveLoad.LoadProgress ();
	}
    #endregion

    #region MoreFunBtn
    public void MoreFunBtn ()
    {
        Application.OpenURL("http://unity3d.com/");
    }
    #endregion

    public void PrivacyPolicy()
    {
        Application.OpenURL("http://unity3d.com/");
       
    }

    #region UpdateStatus
    public void UpdateStatus()
    {

    }
    #endregion

    #region Play
    public void PlayBtn()
    {
        StartCoroutine(LoadScene());
        //LogEvents.Instance.GA_Event(GA_EventType.Completed, "Mode_1");
    }
    IEnumerator LoadScene()
    {
        menu_UI.uiPanel.SetActive(false);
        menu_UI.fillBar.fillAmount = 0;
        menu_UI.loadingPanel.SetActive(true);
        while (menu_UI.fillBar.fillAmount < 1)
        {
            menu_UI.fillBar.fillAmount += Time.deltaTime / loadTime;
            yield return null;
        }
        SceneManager.LoadScene(NextScene.ToString());
        yield return null;
    }
    #endregion
}