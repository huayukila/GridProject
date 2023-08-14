#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Framework.BuildProject
{
    [CustomEditor(typeof(BuildingData))]
    public class BuildingConfigEditor : Editor
    {
        SerializedProperty buildingLevelProp;
        SerializedProperty nameString;
        SerializedProperty Height;
        SerializedProperty Width;
        SerializedProperty Prefab;
        SerializedProperty Visual;
        SerializedProperty NeedResource;
        private void OnEnable()
        {
            buildingLevelProp = serializedObject.FindProperty("LevelDatasList");
            nameString = serializedObject.FindProperty("nameString");
            Height=serializedObject.FindProperty("height");
            Width = serializedObject.FindProperty("width");
            Prefab = serializedObject.FindProperty("prefab");
            Visual = serializedObject.FindProperty("visual");
            NeedResource = serializedObject.FindProperty("NeedResource");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(nameString);
            EditorGUILayout.PropertyField(Prefab);
            EditorGUILayout.PropertyField(Visual);
            EditorGUILayout.PropertyField(Height);
            EditorGUILayout.PropertyField (Width);
            EditorGUILayout.PropertyField (NeedResource);

            EditorGUILayout.PropertyField(buildingLevelProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}