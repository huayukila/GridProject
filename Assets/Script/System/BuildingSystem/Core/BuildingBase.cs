using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class BuildingBase : BuildController
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
        protected List<GridObject> m_BuildingOccupancyGrids { get; set; }

        public virtual void Reset()
        {
            SetInitData(this.GetModel<IBuilDataModel>().GetBuildingConfig(BuildingType));
            GridXY = Vector2Int.zero;
            Dir = Dir.Down;

            foreach (var gridObject in m_BuildingOccupancyGrids)
            {
                gridObject.IsEmpty = true;
            }

            m_BuildingOccupancyGrids.Clear();
        }

        //ÇŸÇ©ÇÃåöï®ÉfÅ[É^
        public virtual void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            SetInitData(data_);
            GridXY = gridXY_;
            Dir = dir_;
            m_BuildingOccupancyGrids = gridObjList_;
        }
        void SetInitData(BuildingData data_)
        {
            BuildingName = data_.m_NameString;
            BuildingMaxHp = data_.m_LevelDatasList[0].m_MaxHp;
            BuildingHp = BuildingMaxHp;
            BuildingLevel = data_.m_LevelDatasList[0].m_Level;
            BuildingType = data_.m_BuildingType;
        }
    }
}