using UnityEngine.SceneManagement;
using UnityEngine;

public class GSF_ConsentScript : MonoBehaviour
{
	public string PrivacyPolicyLink;
	public string TermConditionsLink;
	public string SplashScreen = "SplashScreen";

	public GameObject dialog;
	public void Start()
	{
		if (!PlayerPrefs.HasKey("IsFirstTime"))
		{
			PlayerPrefs.SetInt("IsFirstTime", 0);
		}
		if (PlayerPrefs.GetInt("IsFirstTime") == 0)
		{
			dialog.SetActive(true);
		}
		else
		{
			No();
		}
	}

	public void openPrivacyPolicy()
	{
		Application.OpenURL(PrivacyPolicyLink);
	}

	public void openTermPolicy()
	{
		Application.OpenURL(TermConditionsLink);
	}

	public void Yes()
	{

		PlayerPrefs.SetInt("IsFirstTime", 1);
		PlayerPrefs.Save();
		SceneManager.LoadScene(SplashScreen);
		dialog.SetActive(false);
	}

	public void No()
	{
		dialog.SetActive(false);
		SceneManager.LoadScene(SplashScreen);
	}
}