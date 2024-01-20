using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    [CreateAssetMenu(menuName = "Buildings",fileName = "BuildingData")]
    public class BuildingData : ScriptableObject
    {
        public string NameString;
        public BuildingType BuildingType;
        public Transform Visual;
        public Transform Prefab;
        public int Width;
        public int Height;
        public ResourceType NeedResource;
        [SerializeField] public List<BuildingLevelData> LevelDatasList = new List<BuildingLevelData>();
    }

    [Serializable]
    public struct BuildingLevelData
    {
        public int Level;
        public int MaxWorker;
        public int MaxHp;
        public int UpdateTime;
        public ResourceCost[] CostList;
    }
}