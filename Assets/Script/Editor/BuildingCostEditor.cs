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
            BuildingLevelTable = serializedObject.FindProperty("m_LevelDatasList");
            BuildingType = serializedObject.FindProperty("m_BuildingType");
            NameString = serializedObject.FindProperty("m_NameString");
            Height=serializedObject.FindProperty("m_Height");
            Width = serializedObject.FindProperty("m_Width");
            Prefab = serializedObject.FindProperty("m_Prefab");
            Visual = serializedObject.FindProperty("m_Visual");
            NeedResource = serializedObject.FindProperty("m_NeedResource");
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