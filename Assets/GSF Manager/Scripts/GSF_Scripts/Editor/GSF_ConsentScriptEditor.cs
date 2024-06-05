using UnityEditor;
using Sirenix.OdinInspector.Editor;
[CustomEditor(typeof(GSF_ConsentScript))]
public class GSF_ConsentScriptEditor : OdinEditor
{

    string module = "Consent";

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
