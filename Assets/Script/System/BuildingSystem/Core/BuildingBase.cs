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
        public int WorkerNum;
        public int MaxWorkerNum;
        public BuildingType BuildingType;

        public Vector2Int GridXY;
        public Dir Dir;

        protected List<GridObject> BuildingOccupancyGrids { get; set; }

        // �����̃f�[�^������
        public virtual void Reset()
        {
            SetInitData(this.GetModel<IBuilDataModel>().GetBuildingConfig(BuildingName));
            GridXY = Vector2Int.zero;
            Dir = Dir.Down;

            foreach (var gridObject in BuildingOccupancyGrids)
            {
                gridObject.IsEmpty = true;
            }

            BuildingOccupancyGrids.Clear();
        }

        // �ʂ̌����f�[�^�̏�����
        public virtual void Init(BuildingData data_, List<GridObject> gridObjList_, Vector2Int gridXY_, Dir dir_)
        {
            SetInitData(data_);
            GridXY = gridXY_;
            Dir = dir_;
            BuildingOccupancyGrids = gridObjList_;
        }

        // �����f�[�^�ݒ�
        void SetInitData(BuildingData data_)
        {
            BuildingName = data_.NameString;
            BuildingMaxHp = data_.LevelDatasList[0].MaxHp;
            BuildingHp = BuildingMaxHp;
            BuildingLevel = data_.LevelDatasList[0].Level;
            BuildingType = data_.BuildingType;
        }
    }
}