using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(GSF_OfferWall))]
public class GSF_OfferWallEditor : OdinEditor {

	string module = "Offer Wall";

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
