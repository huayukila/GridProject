using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBuildingObj
    {
        void Update();
    }

    public class BuildingBase : IController, IBuildingObj
    {
        public string BuildingName;
        public int BuildingMaxHp;
        public int BuildingHp;
        public int BuildingLevel;
        public int m_WorkerNum;
        public int m_MaxWorkerNum;
        public BuildingType BuildingType;
        public Vector2Int GridXY;

        public Dir Dir;

        //public int m_MaxLevel; todo...
        protected GameObject m_GameObj;
        protected List<GridObject> m_BuildingOccupancyGrids { get; set; }

        public virtual void Reset()
        {
            BuildingName = "";
            BuildingHp = 0;
            BuildingMaxHp = 0;
            BuildingLevel = 0;
            BuildingType = BuildingType.Non;
            GridXY = Vector2Int.zero;
            Dir = Dir.Down;

            //todo...
            GameObject.Destroy(m_GameObj);
            m_GameObj = null;

            foreach (var gridObject in m_BuildingOccupancyGrids)
            {
                gridObject.IsEmpty = true;
            }

            m_BuildingOccupancyGrids.Clear();
        }

        //ÇŸÇ©ÇÃåöï®ÉfÅ[É^
        public virtual void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_,
            GameObject obj)
        {
            BuildingName = data_.m_NameString;
            BuildingMaxHp = data_.m_LevelDatasList[0].m_MaxHp;
            BuildingHp = BuildingMaxHp;
            BuildingLevel = data_.m_LevelDatasList[0].m_Level;
            BuildingType = data_.m_BuildingType;
            GridXY = gridXY_;
            Dir = dir_;
            m_GameObj = obj;
            m_BuildingOccupancyGrids = gridObjList_;
        }

        public virtual void Update()
        {
        }

        public GameObject GetGameObj()
        {
            return m_GameObj;
        }

        public void Upgrade()
        {
        }


        public IArchitecture GetArchitecture()
        {
            return BuildProject.Interface;
        }
    }
}