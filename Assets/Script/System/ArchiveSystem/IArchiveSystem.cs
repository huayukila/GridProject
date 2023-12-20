using System;
using System.IO;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IArchiveSystem : ISystem
    {
        void CreatSaveData();
        GameSaveData GetSaveData();
        void SaveGameData();
        void ClearSaveData();
    }

    public class ArchiveSystem : AbstractSystem, IArchiveSystem
    {
        private GameSaveData m_SaveData;

        protected override void OnInit()
        {
            string defaultPath = Application.dataPath;
            string saveDataFolderPath = Path.Combine(defaultPath, "SaveData");
            if (!Directory.Exists(saveDataFolderPath))
            {
                Directory.CreateDirectory(saveDataFolderPath);
            }
        }


        public void CreatSaveData()
        {
        }

        public GameSaveData GetSaveData()
        {
            return default;
        }

        public void SaveGameData()
        {
            m_SaveData = new GameSaveData();
            //todo...
            // m_SaveData.stageLevel=


            BuildingObj[] buildsArray = this.GetModel<IBuildingObjModel>().GetBuildingObjsDataForArchiveSystem();
            ResourceCost[] resourcesArray = this.GetModel<IResourceDataModel>().GetResourceDatasForArchiveSystem();
            
            m_SaveData.resDatas = resourcesArray;
            m_SaveData.buildingSaveDatas = new BuildingSaveData[buildsArray.Length];
            for (int i = 0; i < buildsArray.Length; i++)
            {
                m_SaveData.buildingSaveDatas[i].gridX = buildsArray[i].GridXY.x;
                m_SaveData.buildingSaveDatas[i].gridZ = buildsArray[i].GridXY.x;
                m_SaveData.buildingSaveDatas[i].buildingType = buildsArray[i].m_BuildingType;
                m_SaveData.buildingSaveDatas[i].dir = buildsArray[i].Dir;
                m_SaveData.buildingSaveDatas[i].worker = buildsArray[i].m_WorkerNum;
                m_SaveData.buildingSaveDatas[i].level = buildsArray[i].m_BuildingLevel;
            }
        }

        public void ClearSaveData()
        {
        }

        #region �����p�֐�

        void LoadGameData()
        {
        }

        #endregion
    }

    [Serializable]
    public struct BuildingSaveData
    {
        public int gridX;
        public int gridZ;
        public BuildingType buildingType;
        public Dir dir;
        public int level;
        public int worker;
    }

    [Serializable]
    public struct GameSaveData
    {
        public int stageLevel;
        public ResourceCost[] resDatas;
        public BuildingSaveData[] buildingSaveDatas;
    }
}