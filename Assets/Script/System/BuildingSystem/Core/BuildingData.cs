using System;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.BuildProject
{
    [CreateAssetMenu(menuName = "Buildings",fileName = "BuildingData")]
    public class BuildingData : ScriptableObject
    {
        public string m_NameString;
        public BuildingType m_BuildingType;
        public Transform m_Visual;
        public Transform m_Prefab;
        public int m_Width;
        public int m_Height;
        public ResourceType m_NeedResource;
        [SerializeField] public List<BuildingLevelData> m_LevelDatasList = new List<BuildingLevelData>();
    }

    [Serializable]
    public struct BuildingLevelData
    {
        public int m_Level;
        public int m_MaxWorker;
        public int m_MaxHp;
        public int m_UpdateTime;
        public ResourceCost[] m_CostList;
    }
}