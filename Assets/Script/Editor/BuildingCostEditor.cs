#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Framework.BuildProject
{
    [CustomEditor(typeof(BuildingData))]
    public class BuildingConfigEditor : Editor
    {
        SerializedProperty BuildingLevelTable;
        SerializedProperty NameString;
        SerializedProperty Height;
        SerializedProperty Width;
        SerializedProperty Prefab;
        SerializedProperty Visual;
        SerializedProperty BuildingType;
        SerializedProperty NeedResource;
        private void OnEnable()
        {
            BuildingLevelTable = serializedObject.FindProperty("LevelDatasList");
            BuildingType = serializedObject.FindProperty("BuildingType");
            NameString = serializedObject.FindProperty("NameString");
            Height=serializedObject.FindProperty("Height");
            Width = serializedObject.FindProperty("Width");
            Prefab = serializedObject.FindProperty("Prefab");
            Visual = serializedObject.FindProperty("Visual");
            NeedResource = serializedObject.FindProperty("NeedResource");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(NameString);
            EditorGUILayout.PropertyField(BuildingType);
            EditorGUILayout.PropertyField(Prefab);
            EditorGUILayout.PropertyField(Visual);
            EditorGUILayout.PropertyField(Height);
            EditorGUILayout.PropertyField (Width);
            EditorGUILayout.PropertyField (NeedResource);
            EditorGUILayout.PropertyField(BuildingLevelTable);
            serializedObject.ApplyModifiedProperties();
        }
    }
}