using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GameAnalyticsSDK;

public class LogEvents : MonoBehaviour
{

	// Use this for initialization
	private static LogEvents instance;

	private LogEvents()
	{
	}

	public static LogEvents Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LogEvents();
			}
			return instance;
		}
	}

	public void EventStarted(string SceneName, string Identifier, string value)
	{
#if !UNITY_EDITOR
		//GameAnalytics.NewDesignEvent(Identifier);
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Identifier);
        //FirebaseAnalytics.Event(SceneName, Identifier, value);
#endif
		//Debug.Log(SceneName + "=====" + Identifier + "======" + value);
	}

	public void EventCompleted(string SceneName, string Identifier, string value)
	{
#if !UNITY_EDITOR
		 //GameAnalytics.NewDesignEvent(Identifier);
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Identifier);
        //FirebaseAnalytics.Event(SceneName, Identifier, value);
#endif
		//Debug.Log(SceneName + "=====" + Identifier + "======" + value);
	}
	public void EventFailed(string SceneName, string Identifier, string value)
	{
#if !UNITY_EDITOR
		 //GameAnalytics.NewDesignEvent(Identifier);
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, Identifier);
        //FirebaseAnalytics.Event(SceneName, Identifier, value);
#endif
		//Debug.Log(SceneName + "=====" + Identifier + "======" + value);
	}
	public void Event(string SceneName, string Identifier, string value)
	{

		//Debug.Log(SceneName + "=====" + Identifier + "======" + value);
	}
}
