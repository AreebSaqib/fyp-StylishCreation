using System.Collections;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class SplashProperties
{
    public Image splashImage;
    //public Button TapToContinue;
    public Image fillBar;
    [Range(6, 10)]
    public float waitTime;
    public Scenes nextScene;
}
public class SamanGame_SplashScreen : MonoBehaviour
{
    [FoldoutGroup("Splash Properties")]
    [HideLabel]
    public SplashProperties splashProps;

    void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        AudioListener.volume = 1;
        if (!GameManager.Instance.Initialized)
        {
            SamanGame_SaveLoad.LoadProgress();
            GameManager.Instance.Initialized = true;
        }
        StartCoroutine(LoadNextScene());
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Input.multiTouchEnabled = false;
    }

    IEnumerator LoadNextScene()
    {
        if (splashProps.fillBar) splashProps.fillBar.fillAmount = 0;
        while (splashProps.fillBar.fillAmount < 1)
        {
            splashProps.fillBar.fillAmount += Time.deltaTime / splashProps.waitTime;
            yield return null;
        }
        SceneManager.LoadScene(splashProps.nextScene.ToString());
        yield return null;
    }
    
}
