using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.BuildProject
{
    public interface IGridBuildSystem : ISystem
    {
        void CancelSelect();
        void SetBuilding();
        void DestroyBuilding(GameObject gameObj);
        void BuildingRota();
        void SelectBuilding(BuildingData buildingData);
        void VisualBuildingFollowMouse();

        /// <summary>
        /// 建物構築
        /// </summary>
        /// <param name="buildingType_"></param>
        /// <param name="gridPosition_"></param>
        /// <param name="dir_"></param>
        void CreatBuilding(BuildingType buildingType_, Vector2Int gridPosition_, Dir dir_);

        void CreatGrid(int gridWidth_, int gridHeight_, float cellSize_);
        void Deinit();
    }

    public class GridBuildSystem : AbstractSystem, IGridBuildSystem
    {
        //グリッドマップ対象
        GridUtils<GridObject> m_Grid;

        //建物データ
        BuildingData m_BuildingData;

        Transform m_VisualBuilding;
        Dir m_Dir = Dir.Down;

        private BuildingType m_buildingType;

        private IPlayerDataModel m_playerDataModel;

        protected override void OnInit()
        {
            m_playerDataModel = this.GetModel<IPlayerDataModel>();
        }

        public void CreatGrid(int gridWidth, int gridHeight, float cellSize)
        {
            m_Grid = new GridUtils<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero,
                (GridUtils<GridObject> g, int x, int z) => new GridObject(g, x, z)
            );
        }

        public void Deinit()
        {
            //グリッドマップ対象
            m_Grid = null;

            //建物データ
            m_BuildingData = null;

            m_VisualBuilding = null;
            m_Dir = Dir.Down;
            m_buildingType = BuildingType.Non;
        }

        public void CancelSelect()
        {
            m_VisualBuilding.DOKill();
            Object.Destroy(m_VisualBuilding.gameObject);
            m_BuildingData = null;
            m_buildingType = BuildingType.Non;
            m_playerDataModel.playerState = PlayerState.Normal;
        }

        public void SetBuilding()
        {
            if (m_BuildingData == null ||
                !this.GetModel<IResourceDataModel>().IsResEnough(m_BuildingData.m_LevelDatasList[0].m_CostList)) return;

            //マウスの世界座標を獲得
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition, "Ground");
            //マウスの座標をグリッド座標に変換
            m_Grid.GetXY3D(mousePosition, out int x, out int z);

            if (CheckInMap(x, z))
            {
                if (m_BuildingData.m_BuildingType == BuildingType.House)
                {
                    this.GetModel<IResourceDataModel>().MaxWorkerNum +=
                        m_BuildingData.m_LevelDatasList[0].m_MaxWorker;
                    this.GetModel<IResourceDataModel>().AddRes(ResourceType.Worker,
                        m_BuildingData.m_LevelDatasList[0].m_MaxWorker);
                }

                CreatBuilding(m_BuildingData, m_buildingType, new Vector2Int(x, z), m_Dir);
                //建物のデータから建造コーストによって、modelに指定された資源を減少する
                this.GetModel<IResourceDataModel>()
                    .MinusRes(m_BuildingData.m_LevelDatasList[0].m_CostList);
                this.SendEvent<RefreshResPanel>();
            }
            else
            {
                Debug.Log("ここは築けません");
            }
        }

        public void DestroyBuilding(GameObject gameObj)
        {
            IResourceDataModel resDataModel = this.GetModel<IResourceDataModel>();
            IBuildingObjModel buildObjModel = this.GetModel<IBuildingObjModel>();

            BuildingBase buildingBase = buildObjModel.GetBuildData(gameObj.GetInstanceID());

            if (buildingBase.BuildingType == BuildingType.House)
            {
                int removeWorkerNums = buildingBase.m_MaxWorkerNum;
                resDataModel.MaxWorkerNum -= removeWorkerNums;
                if (!resDataModel.IsResEnough(new ResourceCost()
                        { resType = ResourceType.Worker, Cost = removeWorkerNums }))
                {
                    removeWorkerNums -= resDataModel.GetRes(ResourceType.Worker);
                    resDataModel.MinusRes(ResourceType.Worker, resDataModel.GetRes(ResourceType.Worker));
                    List<BuildingBase> tempList = buildObjModel.GetBuildDataList(BuildingType.Factory);
                    foreach (var buildingObj in tempList)
                    {
                        if (buildingObj.m_WorkerNum <= 0)
                        {
                            continue;
                        }

                        if (buildingObj.m_WorkerNum >= removeWorkerNums)
                        {
                            buildingObj.m_WorkerNum -= removeWorkerNums;
                            removeWorkerNums = 0;
                        }
                        else
                        {
                            removeWorkerNums -= buildingObj.m_WorkerNum;
                            buildingObj.m_WorkerNum = 0;
                        }

                        if (removeWorkerNums == 0)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    resDataModel.MinusRes(ResourceType.Worker, buildingBase.m_MaxWorkerNum);
                }
            }
            else
            {
                if (buildingBase.m_WorkerNum != 0)
                {
                    resDataModel.AddRes(ResourceType.Worker, buildingBase.m_WorkerNum);
                }
            }

            buildObjModel.UnregisterBuild(gameObj.GetInstanceID());
            GameObject.Destroy(gameObj);
        }

        public void SelectBuilding(BuildingData buildingData)
        {
            m_playerDataModel.playerState = PlayerState.Build;

            m_BuildingData = buildingData;

            m_Dir = Dir.Down;
            //マウス座標
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition, "Ground");
            //グリッド座標に変換
            m_Grid.GetXY3D(mousePosition, out int x, out int z);
            //ロール基点獲得
            Vector2Int rotationOffset = GetRotationOffset(m_Dir);

            Vector3 buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(x, z) +
                                                  new Vector3(rotationOffset.x, 0.6f, rotationOffset.y) *
                                                  m_Grid.GetCellSize;
            //ゴースト建物生成
            if (m_VisualBuilding == null)
            {
                m_VisualBuilding = Object.Instantiate(
                    buildingData.m_Visual,
                    buildingObjectWorldPosition,
                    Quaternion.Euler(0, GetRotationAngle(m_Dir), 0));
            }
        }

        public void BuildingRota()
        {
            m_Dir = GetNextDir(m_Dir);
        }

        public void VisualBuildingFollowMouse()
        {
            //マウスの世界座標
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition, "Ground");
            //グリッド座標に変換
            m_Grid.GetXY3D(mousePosition, out int x, out int z);
            //ロール基点偏移
            Vector2Int rotationOffset = GetRotationOffset(m_Dir);


            Vector3 buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(x, z) +
                                                  new Vector3(rotationOffset.x, 0.3f, rotationOffset.y) *
                                                  m_Grid.GetCellSize;
            if (x >= 0 && z >= 0 && x < m_Grid.GetGridSize().x && z < m_Grid.GetGridSize().y)
            {
                m_VisualBuilding.DORotate(new Vector3(0, GetRotationAngle(m_Dir), 0), 0.5f);
                m_VisualBuilding.DOMove(buildingObjectWorldPosition, 0.5f);
            }
        }


        public void CreatBuilding(BuildingType buildingType_, Vector2Int gridPosition_, Dir dir_)
        {
            BuildingData buildingData = this.GetModel<IBuilDataModel>().GetBuildingConfig(buildingType_);
            if (buildingData == null)
                return;
            CreatBuilding(buildingData, m_buildingType, gridPosition_, dir_);
        }

        #region 内部用関数

        int GetRotationAngle(Dir dir)
        {
            switch (dir)
            {
                default:
                case Dir.Down: return 0;
                case Dir.Left: return 90;
                case Dir.Up: return 180;
                case Dir.Right: return 270;
            }
        }

        //ロール時の基準点偏移
        Vector2Int GetRotationOffset(Dir dir)
        {
            switch (dir)
            {
                default:
                case Dir.Down: return new Vector2Int(0, 0);
                case Dir.Left: return new Vector2Int(0, m_BuildingData.m_Width);
                case Dir.Up: return new Vector2Int(m_BuildingData.m_Width, m_BuildingData.m_Height);
                case Dir.Right: return new Vector2Int(m_BuildingData.m_Height, 0);
            }
        }

        //建物の方向によって、更新後の建物のグリッドリストを獲得
        List<Vector2Int> GetGridPositionList(BuildingData data_,　Vector2Int offset, Dir dir)
        {
            List<Vector2Int> gridPositionList = new();
            switch (dir)
            {
                default:
                case Dir.Down:
                case Dir.Up:
                    for (int x = 0; x < data_.m_Width; x++)
                    {
                        for (int y = 0; y < data_.m_Height; y++)
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y));
                        }
                    }

                    break;
                case Dir.Left:
                case Dir.Right:
                    for (int x = 0; x < m_BuildingData.m_Height; x++)
                    {
                        for (int y = 0; y < m_BuildingData.m_Width; y++)
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y));
                        }
                    }

                    break;
            }

            return gridPositionList;
        }

        Dir GetNextDir(Dir dir)
        {
            switch (dir)
            {
                default:
                case Dir.Down:
                    return Dir.Left;
                case Dir.Left:
                    return Dir.Up;
                case Dir.Up:
                    return Dir.Right;
                case Dir.Right:
                    return Dir.Down;
            }
        }

        bool CheckInMap(int x, int y)
        {
            //クリックした場所はマップの中ではないか？
            if (!(x >= 0 && y >= 0 && x <= m_Grid.GetGridSize().x && y <= m_Grid.GetGridSize().y))
                return false;

            //建物の占有グリッドリスト
            List<Vector2Int> gridPositionList = GetGridPositionList(m_BuildingData, new Vector2Int(x, y), m_Dir);

            //建物の毎グリッドはマップの中にあるのか
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!(gridPosition.x >= m_Grid.GetGridSize().x) &&
                    !(gridPosition.y >= m_Grid.GetGridSize().y)) continue;
                return false;
            }


            //グリッドの中にも建物がありました
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                GridObject gObj = m_Grid.GetGridObjectByXY(gridPosition.x, gridPosition.y);
                if (gObj.IsEmpty && gObj.TerrainData.resType == m_BuildingData.m_NeedResource) continue;
                return false;
            }

            return true;
        }


        /// <summary>
        /// runtime建物構築
        /// </summary>
        /// <param name="buildingData_"></param>
        /// <param name="buildingType_"></param>
        /// <param name="gridPosition_"></param>
        /// <param name="dir_"></param>
        void CreatBuilding(BuildingData buildingData_, BuildingType buildingType_, Vector2Int gridPosition_, Dir dir_)
        {
            //基準点偏移
            Vector2Int rotationOffset = GetRotationOffset(dir_);
            //正しいの世界座標を獲得
            Vector3 buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(gridPosition_.x, gridPosition_.y) +
                                                  new Vector3(rotationOffset.x, 0, rotationOffset.y) *
                                                  m_Grid.GetCellSize;


            List<Vector2Int> gridPositionList = GetGridPositionList(buildingData_,
                new Vector2Int(gridPosition_.x, gridPosition_.y), dir_);

            List<GridObject> tempGridObjList = new List<GridObject>();
            foreach (Vector2Int girdPosition in gridPositionList)
            {
                GridObject gridObj = m_Grid.GetGridObjectByXY(girdPosition.x, girdPosition.y);
                gridObj.IsEmpty = false;
                tempGridObjList.Add(gridObj);
            }

            //建物実体生成
            Transform tempBuilding = GameObject.Instantiate(buildingData_.m_Prefab,
                buildingObjectWorldPosition,
                Quaternion.Euler(0, GetRotationAngle(dir_), 0));
            BuildingBase buildingBase = tempBuilding.GetComponent<BuildingBase>();
            buildingBase.Init(buildingData_, tempGridObjList, gridPosition_, dir_);
            this.GetModel<IBuildingObjModel>().RegisterBuild(tempBuilding.gameObject.GetInstanceID(), buildingBase);
        }

        #endregion
    }
}