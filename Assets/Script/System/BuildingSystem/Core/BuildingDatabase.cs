using UnityEngine;

namespace Framework.BuildProject
{
    [CreateAssetMenu(menuName = "DataBase/BuildingDatabase",fileName = "BuildingDatabase")]
    public class BuildingDatabase : ScriptableObject
    {
        public BuildingData[] BuildingDatas;
    }
}