using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.BuildProject
{
    public interface IGridBuildSystem : ISystem
    {
        BindableProperty<PlayerState> m_State { get; }
        void CancelSelect();
        void SetBuilding();
        void DestroyBuilding(GameObject gameObj);
        void BuildingRota();
        void SelectBuilding<T>(BuildingData buildingData) where T : BuildingObj;
        void VisualBuildingFollowMouse();

        bool CreatMapByArchive();
    }

    public class GridBuildSystem : AbstractSystem, IGridBuildSystem
    {
        //�O���b�h�}�b�v�Ώ�
        GridUtils<GridObject> m_Grid;

        //�����f�[�^
        BuildingData m_BuildingData;

        Transform m_VisualBuilding;
        Dir m_Dir = Dir.Down;

        private Type m_BuildingObjType;

        public BindableProperty<PlayerState> m_State { get; } = new BindableProperty<PlayerState>()
        {
            Value = PlayerState.Normal
        };

        protected override void OnInit()
        {
            m_State.Register(e => { Debug.Log("���[�h�ύX" + m_State.Value); });
            CreatGrid(Global.GRID_SIZE_WIDTH, Global.GRID_SIZE_HEIGHT, Global.GRID_SIZE_CELL);
        }


        public void CancelSelect()
        {
            m_VisualBuilding.DOKill();
            Object.Destroy(m_VisualBuilding.gameObject);
            m_BuildingData = null;
            m_BuildingObjType = null;
            m_State.Value = PlayerState.Normal;
        }

        public void SetBuilding()
        {
            if (m_BuildingData == null ||
                !this.GetModel<IResourceDataModel>().IsResEnough(m_BuildingData.m_LevelDatasList[0].m_CostList)) return;

            //�}�E�X�̐��E���W���l��
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
            //�}�E�X�̍��W���O���b�h���W�ɕϊ�
            m_Grid.GetXY3D(mousePosition, out int x, out int z);

            if (CheckInMap(x, z))
            {
                //�����̐�L�O���b�h���X�g
                List<Vector2Int> gridPositionList = GetGridPositionList(new Vector2Int(x, z), m_Dir);
                //�}�b�v���t���O
                bool isInGridMap = true;
                //�z����t���O
                bool isCanBuild = true;

                //�����̖��O���b�h�̓}�b�v�̒��ɂ���̂�
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    if (!(gridPosition.x >= m_Grid.GetGridSize().x) &&
                        !(gridPosition.y >= m_Grid.GetGridSize().y)) continue;
                    isInGridMap = false;
                    Debug.Log("�����ɒz�����Ƃ͂ł��܂���");
                    break;
                }

                if (isInGridMap)
                {
                    //�O���b�h�̒��ɂ�����������܂���
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        GridObject gObj = m_Grid.GetGridObjectByXY(gridPosition.x, gridPosition.y);
                        if (gObj.IsEmpty && gObj.TerrainData.resType == m_BuildingData.m_NeedResource) continue;
                        Debug.Log("�����ɒz�����Ƃ͂ł��܂���");
                        isCanBuild = false;
                        break;
                    }
                }

                if (!isCanBuild || !isInGridMap) return;

                if (m_BuildingData.m_BuildingType == BuildingType.House)
                {
                    this.GetModel<IResourceDataModel>().MaxWorkerNum +=
                        m_BuildingData.m_LevelDatasList[0].m_MaxWorker;
                    this.GetModel<IResourceDataModel>().AddRes(ResourceType.Worker,
                        m_BuildingData.m_LevelDatasList[0].m_MaxWorker);
                }

                //��_�Έ�
                Vector2Int rotationOffset = GetRotationOffset(m_Dir);
                //�������̐��E���W���l��
                Vector3 buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(x, z) +
                                                      new Vector3(rotationOffset.x, 0, rotationOffset.y) *
                                                      m_Grid.GetCellSize;
                //��������
                Transform buildTransform = Object.Instantiate(
                    m_BuildingData.m_Prefab,
                    buildingObjectWorldPosition,
                    Quaternion.Euler(0, GetRotationAngle(m_Dir), 0));


                //runtime building obj�\�z
                {
                    var obj = Activator.CreateInstance(m_BuildingObjType);
                    BuildingObj buildingObj = obj as BuildingObj;
                    //�����̎��̂𑊉��̃O���b�h�ɒ��ɑł�����
                    List<GridObject> tempGridObjList = new List<GridObject>();
                    foreach (Vector2Int girdPosition in gridPositionList)
                    {
                        GridObject gridObj = m_Grid.GetGridObjectByXY(girdPosition.x, girdPosition.y);
                        gridObj.SetTransform(buildTransform);
                        tempGridObjList.Add(gridObj);
                    }

                    buildingObj.Init(m_BuildingData, tempGridObjList, new Vector2Int(x, z),m_Dir);
                    this.GetModel<IBuildingObjModel>()
                        .RegisterBuild(buildTransform.gameObject.GetInstanceID(), buildingObj);
                }

                buildTransform.DOScaleY(1, 0.5f);

                //�����̃f�[�^���猚���R�[�X�g�ɂ���āAmodel�Ɏw�肳�ꂽ��������������
                this.GetModel<IResourceDataModel>()
                    .MinusRes(m_BuildingData.m_LevelDatasList[0].m_CostList);
                this.SendEvent<RefreshResPanel>();
            }
            else
            {
                Debug.Log("�����̓}�b�v�̒��ł͂Ȃ�");
            }
        }

        public void DestroyBuilding(GameObject gameObj)
        {
            IResourceDataModel resDataModel = this.GetModel<IResourceDataModel>();
            IBuildingObjModel buildObjModel = this.GetModel<IBuildingObjModel>();

            BuildingObj obj = buildObjModel.GetBuildData(gameObj.GetHashCode());

            if (obj.m_BuildingType == BuildingType.House)
            {
                int removeWorkerNums = obj.m_MaxWorkerNum;
                resDataModel.MaxWorkerNum -= removeWorkerNums;
                if (!resDataModel.IsResEnough(new ResourceCost()
                        { resType = ResourceType.Worker, Cost = removeWorkerNums }))
                {
                    removeWorkerNums -= resDataModel.GetRes(ResourceType.Worker);
                    resDataModel.MinusRes(ResourceType.Worker, resDataModel.GetRes(ResourceType.Worker));
                    List<BuildingObj> tempList = buildObjModel.GetBuildDataList(BuildingType.Factory);
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
                    resDataModel.MinusRes(ResourceType.Worker, obj.m_MaxWorkerNum);
                }
            }
            else
            {
                if (obj.m_WorkerNum != 0)
                {
                    resDataModel.AddRes(ResourceType.Worker, obj.m_WorkerNum);
                }
            }

            buildObjModel.UnregisterBuild(gameObj.GetInstanceID());
        }

        public void SelectBuilding<T>(BuildingData buildingData) where T : BuildingObj
        {
            m_State.Value = PlayerState.Build;

            m_BuildingData = buildingData;

            m_BuildingObjType = typeof(T);

            m_Dir = Dir.Down;
            //�}�E�X���W
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
            //�O���b�h���W�ɕϊ�
            m_Grid.GetXY3D(mousePosition, out int x, out int z);
            //���[����_�l��
            Vector2Int rotationOffset = GetRotationOffset(m_Dir);

            Vector3 buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(x, z) +
                                                  new Vector3(rotationOffset.x, 0.6f, rotationOffset.y) *
                                                  m_Grid.GetCellSize;
            //�S�[�X�g��������
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
            //�}�E�X�̐��E���W
            Vector3 mousePosition = UtilsClass.Instance.GetMouseWorldPosition3D(Input.mousePosition);
            //�O���b�h���W�ɕϊ�
            m_Grid.GetXY3D(mousePosition, out int x, out int z);
            //���[����_�Έ�
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

        public bool CreatMapByArchive()
        {
            return false;
        }

        #region �����p�֐�

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

        //���[�����̊�_�Έ�
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

        //�����̕����ɂ���āA�X�V��̌����̃O���b�h���X�g���l��
        List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
        {
            List<Vector2Int> gridPositionList = new();
            switch (dir)
            {
                default:
                case Dir.Down:
                case Dir.Up:
                    for (int x = 0; x < m_BuildingData.m_Width; x++)
                    {
                        for (int y = 0; y < m_BuildingData.m_Height; y++)
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
            return x >= 0 && y >= 0 && x <= m_Grid.GetGridSize().x && y <= m_Grid.GetGridSize().y;
        }

        void CreatGrid(int gridWidth, int gridHeight, float cellSize)
        {
            m_Grid = new GridUtils<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero,
                (GridUtils<GridObject> g, int x, int z) => new GridObject(g, x, z)
            );
            //�n�`�ݒ�t�@�C������O���b�h�}�b�v��ݒ肷��
            //...
            // m_Grid.GetGridObjectByXY(1, 1).TerrainData.resType = ResourceType.Gold;
            GameObject.Instantiate(this.GetModel<IBuilDataModel>().GetBuildingConfig("CentreCore").m_Prefab,
                m_Grid.GetWorldPosition3D(5, 5), Quaternion.Euler(0, GetRotationAngle(Dir.Down), 0));
        }

        void CreatBuilding()
        {
        }

        #endregion
    }
}