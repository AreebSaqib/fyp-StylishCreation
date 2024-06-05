using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(GSF_PlayerSelection))]
public class GSF_PlayerSelectionEditor : OdinEditor
{

    string module = "Player Selection";

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
