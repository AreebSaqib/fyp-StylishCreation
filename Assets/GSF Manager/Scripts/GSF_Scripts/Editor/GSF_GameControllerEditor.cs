using UnityEditor;
using Sirenix.OdinInspector.Editor;
[CustomEditor(typeof(GSF_GameController))]
public class GSF_GameControllerEditor : OdinEditor
{

    string module = "Game Play";

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
