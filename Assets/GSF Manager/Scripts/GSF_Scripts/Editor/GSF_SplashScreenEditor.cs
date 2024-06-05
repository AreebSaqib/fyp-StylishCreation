using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(SamanGame_SplashScreen))]
public class GSF_SplashScreenEditor : OdinEditor
{

    string module = "Splash Screen";

    void Awake()
    {
        GSF_Editor.GetLogo();
    }

    public override void OnInspectorGUI()
    {

        GSF_Editor.DefineGUIStyle(module);

        EditorGUILayout.BeginVertical("box");
        DrawDefaultInspector();
        EditorGUILayout.EndHorizontal();
    }
}
