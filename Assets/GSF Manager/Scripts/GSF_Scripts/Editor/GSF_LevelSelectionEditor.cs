using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(SamanGame_LevelSelection))]
public class GSF_LevelSelectionEditor : OdinEditor
{

    string module = "Level Selection";

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
