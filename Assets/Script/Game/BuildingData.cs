using System;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.BuildProject
{
    [CreateAssetMenu(fileName = "BuildingData")]
    public class BuildingData : ScriptableObject
    {
        public string nameString;
        public Transform visual;
        public Transform prefab;
        public int width;
        public int height;
        public ResourceType NeedResource;

        [SerializeField]
        public List< BuildingLevelData> LevelDatasList=new List<BuildingLevelData>();
    }
}