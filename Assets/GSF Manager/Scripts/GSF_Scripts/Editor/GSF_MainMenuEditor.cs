using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(SamanGame_MainMenu))]
public class GSF_MainMenuEditor : OdinEditor {

    string module = "Main Menu";

    void Awake() {
        GSF_Editor.GetLogo();
    }

    public override void OnInspectorGUI() {

        GSF_Editor.DefineGUIStyle(module);
        EditorGUILayout.BeginVertical("box");
        DrawDefaultInspector();
        EditorGUILayout.EndHorizontal();
    }
}

public class ResetSaveData{
	[MenuItem("Window/GSF - Gamerz Studio Framework/Reset Save Data %#r")]
	private static void ResetSave (){				
		Reset ();
	}

	[MenuItem("Window/GSF - Gamerz Studio Framework/Open Save File %#o")]
	private static void OpenSave (){
		Application.OpenURL (Application.persistentDataPath);
	}

	public static void Reset(){
        SamanGame_SaveLoad.DeleteProgress();
		EditorUtility.DisplayDialog("GSF  - Gamerz Studio Framework",
			"Save data reset successfull !", 
			"Ok");
	}
}

public class UGUI
{
    #region AnchorsToCorners
    [MenuItem("uGUI/Anchors to Corners %[")]
    static void AnchorsToCorners()
    {
        RectTransform t = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;

        if (t == null || pt == null) return;

        Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                            t.anchorMin.y + t.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                            t.anchorMax.y + t.offsetMax.y / pt.rect.height);

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }
    #endregion

    #region CornersToAnchors
    [MenuItem("uGUI/Corners to Anchors %]")]
    static void CornersToAnchors()
    {
        RectTransform t = Selection.activeTransform as RectTransform;

        if (t == null) return;

        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }
    #endregion
}
