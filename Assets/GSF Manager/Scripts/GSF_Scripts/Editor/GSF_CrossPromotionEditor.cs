using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(GSF_CrossPromotion))]
public class GSF_CrossPromotionEditor : OdinEditor
{

	string module = "Cross Promotion";

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
