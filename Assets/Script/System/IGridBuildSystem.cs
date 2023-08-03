using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Framework.BuildProject
{
    public enum Dir
    {
        Down,
        Left,
        Up,
        Right
    }
    public enum State
    {
        Normal,
        Build
    }
    public enum ObjectType
    {

    }

    public class Buildingobj
    {
        public int WorkerNum;
        public int BuildingHp;
        public int BuildingLevel;
        public int MaxWorkerNum;
        public int MaxBuildingHp;
        public Buildingobj(int MaxHp,int MaxWorkerNum)
        {
            MaxBuildingHp = MaxHp;
            BuildingHp = MaxBuildingHp;
            WorkerNum = 0;
            this.MaxWorkerNum = MaxWorkerNum;
        }
        //ほかの建物データ
    }
    public interface IGridBuildSystem : ISystem
    {
        public BindableProperty<State> state { get; }
        public void ChangeToBuildState();
        public void ChangeToNormalState();
        public void SetBuilding();
        public void DestroyBuilding();
        public void BuildingRota();
        public void CreatGrid(int gridWidth, int gridHeight, float cellSize);
        public void SelectBuilding(BuildingData building);
        public void VisualBuildingFollowMouse();
    }
    public class GridBuildSystem : AbstractSystem, IGridBuildSystem
    {
        GridUtils<GridObject> grid;
        BuildingData building;
        Transform visualBuilding;
        Dir dir = Dir.Down;

        public BindableProperty<State> state { get; } = new BindableProperty<State>()
        {
            Value = State.Normal
        };

        protected override void OnInit()
        {
            state.Register(e =>
            {
                Debug.Log("モード変更" + state.Value);
            });
        }
        public void CreatGrid(int gridWidth, int gridHeight, float cellSize)
        {
            grid = new GridUtils<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridUtils<GridObject> g, int x, int z) => new GridObject(g, x, z));
        }
        public void SetBuilding()
        {
            if (building != null && this.GetModel<IResourceDataModel>().IsResEnough(building.LevelDatasList[0].costList))
            {
                //マウスの世界座標を獲得
                Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
                //マウスの座標をグリッド座標に変換
                grid.GetXY3D(mousePosition, out int x, out int z);
                Debug.Log(x + "," + z);
                if (x >= 0 && z >= 0 && x <= grid.GetGridSize().x && z <= grid.GetGridSize().y)
                {
                    //建物のグリッド
                    List<Vector2Int> gridPositionList = GetGridPositionList(new Vector2Int(x, z), dir);
                    //マップ中フラグ
                    bool isInGridMap = true;
                    //築けるフラグ
                    bool isCanBuil = true;

                    //建物の毎グリッドはマップの中にあるのか
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        if (gridPosition.x >= grid.GetGridSize().x || gridPosition.y >= grid.GetGridSize().y)
                        {
                            isInGridMap = false;
                            Debug.Log("ここに築くことはできません");
                            break;
                        }
                    }
                    if (isInGridMap)
                    {
                        //グリッドの中にも建物がありました
                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            if (!grid.GetGridObjectByXY(gridPosition.x, gridPosition.y).Canbuild)
                            {
                                Debug.Log("ここはもう何か築かれた");
                                isCanBuil = false;
                                break;
                            }
                        }
                    }
                    if (isInGridMap && isCanBuil)
                    {
                        //基準点偏移
                        Vector2Int rotationOffset = GetRotationOffset(dir);
                        //正しいの世界座標を獲得
                        Vector3 buildingObjectWorldPosition = grid.GetWorldPosition3D(x, z) +
                            new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize;
                        //建物生成
                        Transform buildTransform = UnityEngine.Object.Instantiate(
                            building.prefab,
                            buildingObjectWorldPosition,
                            Quaternion.Euler(0, GetRotationAngle(dir), 0));
                        //建物の実体を相応のグリッドに中に打ち込み
                        foreach (Vector2Int girdPosition in gridPositionList)
                        {
                            grid.GetGridObjectByXY(girdPosition.x, girdPosition.y).SetTransform(buildTransform);
                        }
                        buildTransform.DOScaleY(1, 0.5f);


                        //資源システムから建造コーストによって、指定された資源を減少する
                        this.GetModel<IResourceDataModel>().DeductResources(building.LevelDatasList[0].costList);

                        this.SendEvent<RefreshResPanel>();
                        //テースト用
                        Buildingobj buildingobj = new Buildingobj(building.LevelDatasList[0].MaxHp, building.LevelDatasList[0].MaxWorker) 
                        { BuildingLevel = building.LevelDatasList[0].Level};
                        this.GetModel<IBuildingModel>().RegisterBuild(buildTransform.gameObject, buildingobj);
                    }
                }
                else
                {
                    Debug.Log("ここはマップの中ではない");
                }
            }
        }
        public void DestroyBuilding()
        {
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
            GridObject gridObject = grid.GetGridObjectByWorldPosition(mousePosition);
            gridObject.DestoryObject();
        }
        public void SelectBuilding(BuildingData building)
        {
            this.building = building;
            dir = Dir.Down;
            //マウス座標
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
            //グリッド座標に変換
            grid.GetXY3D(mousePosition, out int x, out int z);
            //ロール基点獲得
            Vector2Int rotationOffset = GetRotationOffset(dir);
            
            Vector3 buildingObjectWorldPosition = grid.GetWorldPosition3D(x, z) +
                new Vector3(rotationOffset.x, 0.6f, rotationOffset.y) * grid.GetCellSize;
            //ゴースト建物生成
            if (visualBuilding == null)
            {
                visualBuilding = UnityEngine.Object.Instantiate(
                building.visual,
                buildingObjectWorldPosition,
                Quaternion.Euler(0, GetRotationAngle(dir), 0));
            }
        }
        public void BuildingRota()
        {
            dir = GetNextDir(dir);
        }
        public void ChangeToBuildState()
        {
            state.Value = State.Build;
        }
        public void ChangeToNormalState()
        {
            visualBuilding.DOKill();
            UnityEngine.Object.Destroy(visualBuilding.gameObject);
            state.Value = State.Normal;
        }
        public void VisualBuildingFollowMouse()
        {
            //マウスの世界座標
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
            //グリッド座標に変換
            grid.GetXY3D(mousePosition, out int x, out int z);
            //ロール基点偏移
            Vector2Int rotationOffset = GetRotationOffset(dir);


            Vector3 buildingObjectWorldPosition = grid.GetWorldPosition3D(x, z) +
                new Vector3(rotationOffset.x, 0.3f, rotationOffset.y) * grid.GetCellSize;
            if (x >= 0 && z >= 0 && x < grid.GetGridSize().x && z < grid.GetGridSize().y)
            {
                visualBuilding.DORotate(new Vector3(0, GetRotationAngle(dir), 0), 0.5f);
                visualBuilding.DOMove(buildingObjectWorldPosition, 0.5f);
            }
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
                case Dir.Left: return new Vector2Int(0, building.width);
                case Dir.Up: return new Vector2Int(building.width, building.height);
                case Dir.Right: return new Vector2Int(building.height, 0);
            }
        }
        //建物の方向によって、更新後の建物のグリッドリストを獲得
        List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
        {
            List<Vector2Int> gridPositionList = new();
            switch (dir)
            {
                default:
                case Dir.Down:
                case Dir.Up:
                    for (int x = 0; x < building.width; x++)
                    {
                        for (int y = 0; y < building.height; y++)
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y));
                        }
                    }
                    break;
                case Dir.Left:
                case Dir.Right:
                    for (int x = 0; x < building.height; x++)
                    {
                        for (int y = 0; y < building.width; y++)
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
        #endregion
    }
}