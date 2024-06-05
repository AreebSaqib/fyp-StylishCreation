using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(GSF_StoreManager))]
public class GSF_StoreManagerEditor : OdinEditor {

    string module = "Store Manager";

    void Awake() {
        GSF_Editor.GetLogo();
    }

    public override void OnInspectorGUI() {

        GSF_Editor.DefineGUIStyle(module);

        EditorGUILayout.BeginVertical("box");

        DrawDefaultInspector();

        EditorGUILayout.EndHorizontal();
    }

    void Draw(string Object)
    {
        var serializedObject = new SerializedObject(target);
        var property = serializedObject.FindProperty(Object);
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();
    }
}
