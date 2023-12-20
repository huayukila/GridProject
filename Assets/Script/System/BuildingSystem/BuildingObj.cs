using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBuildingObj
    {
        void Update();
    }

    public class BuildingObj : IController, IBuildingObj
    {
        public int m_BuildingHp;
        public int m_BuildingLevel;
        public string m_BuildingName;
        public BuildingType m_BuildingType;

        public int m_MaxBuildingHp;
        public int m_MaxWorkerNum;
        public int m_UpdateTime;
        public int m_WorkerNum;

        public Vector2Int GridXY;
        public Dir Dir;

        private float m_DurationTime;
        private GameObject m_GameObj;
        public List<GridObject> m_BuildingOccupancyGrids { get; private set; }
        private bool isWorking => m_WorkerNum > 0;

        public void Update()
        {
            if (!isWorking)
            {
                m_DurationTime = Time.time;
                return;
            }

            if (Time.time - m_DurationTime < m_UpdateTime) return;

            OnUpdate();
            m_DurationTime = Time.time;
        }

        public IArchitecture GetArchitecture()
        {
            return BuildProject.Interface;
        }

        //‚Ù‚©‚ÌŒš•¨ƒf[ƒ^
        public void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_,Dir dir_)
        {
            m_BuildingName = data_.m_NameString;
            m_MaxBuildingHp = data_.m_LevelDatasList[0].m_MaxHp;
            m_BuildingHp = m_MaxBuildingHp;
            m_BuildingLevel = data_.m_LevelDatasList[0].m_Level;
            m_WorkerNum = 0;
            m_MaxWorkerNum = data_.m_LevelDatasList[0].m_MaxWorker;
            m_UpdateTime = data_.m_LevelDatasList[0].m_UpdateTime;
            m_DurationTime = 0;
            m_BuildingType = data_.m_BuildingType;
            m_BuildingOccupancyGrids = gridObjList_;
            m_GameObj = gridObjList_[0].GetTransform().gameObject;

            GridXY = gridXY_;
            Dir = dir_;
        }

        protected virtual void OnUpdate()
        {
        }

        public GameObject GetGameObj()
        {
            return m_GameObj;
        }

        public void Reset()
        {
            m_WorkerNum = 0;
            m_BuildingHp = 0;
            m_BuildingLevel = 0;
            m_MaxWorkerNum = 0;
            m_MaxBuildingHp = 0;
            m_UpdateTime = 0;
            m_DurationTime = 0.0f;
            m_BuildingOccupancyGrids[0].DestoryObject();
            m_BuildingOccupancyGrids.Clear();
            m_BuildingType = BuildingType.Non;
        }
    }
}