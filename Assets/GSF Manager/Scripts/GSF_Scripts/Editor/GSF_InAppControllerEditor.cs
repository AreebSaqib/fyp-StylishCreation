using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(GSF_InAppController))]
public class GSF_InAppControllerEditor : OdinEditor{

    string module = "InApp Controller";

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