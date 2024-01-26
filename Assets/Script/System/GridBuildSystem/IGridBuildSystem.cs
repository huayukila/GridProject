using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        ///     �����\�z
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="gridPosition_"></param>
        /// <param name="dir_"></param>
        void CreateBuilding(string name_, Vector2Int gridPosition_, Dir dir_);

        void CreateGrid(int gridWidth_, int gridHeight_, float cellSize_);
        void Deinit();
    }

    public class GridBuildSystem : AbstractSystem, IGridBuildSystem
    {
        //�����f�[�^
        private BuildingData m_BuildingData;

        private BuildingType m_buildingType;

        private Dir m_Dir = Dir.Down;

        //�O���b�h�}�b�v�Ώ�
        private GridUtils<GridObject> m_Grid;

        private IPlayerDataModel m_playerDataModel;

        private Transform m_VisualBuilding;

        public void CreateGrid(int gridWidth, int gridHeight, float cellSize)
        {
            m_Grid = new GridUtils<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero,
                (g, x, z) => new GridObject(x, z)
            );
        }

        public void Deinit()
        {
            //�O���b�h�}�b�v�Ώ�
            m_Grid = null;

            //�����f�[�^
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
            if (m_BuildingData == null || !this.GetModel<IResourceDataModel>()
                    .IsResEnough(m_BuildingData.LevelDatasList[0].CostList))
                return;

            // �}�E�X�̐��E���W���擾
            var mousePosition = Utils.GetMouseWorldPosition3D(Input.mousePosition, "Ground");
            // �}�E�X���W���O���b�h���W�ɕϊ�
            m_Grid.GetXY3D(mousePosition, out var x, out var z);

            if (CheckInMap(x, z))
            {
                if (m_BuildingData.BuildingType == BuildingType.House)
                {
                    var resourceModel = this.GetModel<IResourceDataModel>();
                    resourceModel.MaxWorkerNum += m_BuildingData.LevelDatasList[0].MaxWorker;
                    resourceModel.AddRes(ResourceType.Worker, m_BuildingData.LevelDatasList[0].MaxWorker);
                }

                CreatBuilding(m_BuildingData, m_buildingType, new Vector2Int(x, z), m_Dir);
                // �����f�[�^���猚�݃R�X�g�ɉ����ă��f���Ɏw�肳�ꂽ���\�[�X������
                this.GetModel<IResourceDataModel>().MinusRes(m_BuildingData.LevelDatasList[0].CostList);
                this.SendEvent<RefreshResPanel>();
            }
        }

        public void DestroyBuilding(GameObject gameObj)
        {
            // ���\�[�X�ƌ����I�u�W�F�N�g���f�����擾
            var resDataModel = this.GetModel<IResourceDataModel>();
            var buildObjModel = this.GetModel<IBuildingObjModel>();

            var buildingBase = buildObjModel.GetBuildData(gameObj.GetInstanceID());

            if (buildingBase.BuildingType == BuildingType.House)
            {
                var removeWorkerNums = buildingBase.MaxWorkerNum;
                resDataModel.MaxWorkerNum -= removeWorkerNums;

                if (!resDataModel.IsResEnough(new ResourceCost
                        { resType = ResourceType.Worker, Cost = removeWorkerNums }))
                    RemoveWorkers(resDataModel, buildObjModel, removeWorkerNums);
                else
                    resDataModel.MinusRes(ResourceType.Worker, removeWorkerNums);
            }
            else if (buildingBase.WorkerNum != 0)
            {
                resDataModel.AddRes(ResourceType.Worker, buildingBase.WorkerNum);
            }

            buildObjModel.UnregisterBuild(gameObj.GetInstanceID());
            this.SendEvent<RefreshResPanel>();
            Object.Destroy(gameObj);
        }

        public void SelectBuilding(BuildingData buildingData)
        {
            if (m_VisualBuilding!=null)
                return;

            m_playerDataModel.playerState = PlayerState.Build;
            m_BuildingData = buildingData;
            m_Dir = Dir.Down;

            // �}�E�X���W���擾
            var mousePosition = Utils.GetMouseWorldPosition3D(Input.mousePosition, "Ground");
            // �}�E�X���W���O���b�h���W�ɕϊ�
            m_Grid.GetXY3D(mousePosition, out var x, out var z);

            // ��]���̊�_�I�t�Z�b�g���擾
            var rotationOffset = GetRotationOffset(m_Dir);

            // �����������̃��[���h���W���v�Z
            var buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(x, z) +
                                              new Vector3(rotationOffset.x, 0.6f, rotationOffset.y) *
                                              m_Grid.GetCellSize;

            // �S�[�X�g�����̐���
            if (m_VisualBuilding == null)
            {
                m_VisualBuilding = Object.Instantiate(
                    buildingData.Visual,
                    buildingObjectWorldPosition,
                    Quaternion.Euler(0, GetRotationAngle(m_Dir), 0));
            }
            else
            {
                // �����̃S�[�X�g�����̈ʒu���X�V
                m_VisualBuilding.position = buildingObjectWorldPosition;
                m_VisualBuilding.rotation = Quaternion.Euler(0, GetRotationAngle(m_Dir), 0);
            }
        }


        public void BuildingRota()
        {
            m_Dir = GetNextDir(m_Dir);
        }

        public void VisualBuildingFollowMouse()
        {
            // �}�E�X�̐��E���W���擾
            var mousePosition = Utils.GetMouseWorldPosition3D(Input.mousePosition, "Ground");
            // �}�E�X���W���O���b�h���W�ɕϊ�
            m_Grid.GetXY3D(mousePosition, out var x, out var z);

            // ��]��_�̃I�t�Z�b�g���擾
            var rotationOffset = GetRotationOffset(m_Dir);

            // �����̐��������[���h���W���v�Z
            var buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(x, z) +
                                              new Vector3(rotationOffset.x, 0.3f, rotationOffset.y) *
                                              m_Grid.GetCellSize;

            if (x >= 0 && z >= 0 && x < m_Grid.GetGridSize().x && z < m_Grid.GetGridSize().y &&
                m_VisualBuilding != null)
            {
                m_VisualBuilding.DORotate(new Vector3(0, GetRotationAngle(m_Dir), 0), 0.5f);
                m_VisualBuilding.DOMove(buildingObjectWorldPosition, 0.5f);
            }
        }


        public void CreateBuilding(string name_, Vector2Int gridPosition_, Dir dir_)
        {
            var buildingData = this.GetModel<IBuilDataModel>().GetBuildingConfig(name_);
            if (buildingData == null)
                return;

            CreatBuilding(buildingData, m_buildingType, gridPosition_, dir_);
        }

        protected override void OnInit()
        {
            m_playerDataModel = this.GetModel<IPlayerDataModel>();
        }


        #region �����p�֐�

        private int GetRotationAngle(Dir dir)
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
        private Vector2Int GetRotationOffset(Dir dir)
        {
            switch (dir)
            {
                default:
                case Dir.Down: return new Vector2Int(0, 0);
                case Dir.Left: return new Vector2Int(0, m_BuildingData.Width);
                case Dir.Up: return new Vector2Int(m_BuildingData.Width, m_BuildingData.Height);
                case Dir.Right: return new Vector2Int(m_BuildingData.Height, 0);
            }
        }

        //�����̕����ɂ���āA�X�V��̌����̃O���b�h���X�g���l��
        private List<Vector2Int> GetGridPositionList(BuildingData data_,�@Vector2Int offset, Dir dir)
        {
            List<Vector2Int> gridPositionList = new();
            switch (dir)
            {
                default:
                case Dir.Down:
                case Dir.Up:
                    for (var x = 0; x < data_.Width; x++)
                    for (var y = 0; y < data_.Height; y++)
                        gridPositionList.Add(offset + new Vector2Int(x, y));

                    break;
                case Dir.Left:
                case Dir.Right:
                    for (var x = 0; x < m_BuildingData.Height; x++)
                    for (var y = 0; y < m_BuildingData.Width; y++)
                        gridPositionList.Add(offset + new Vector2Int(x, y));

                    break;
            }

            return gridPositionList;
        }

        private Dir GetNextDir(Dir dir)
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

        private bool CheckInMap(int x, int y)
        {
            //�N���b�N�����ꏊ�̓}�b�v�̒��ł͂Ȃ����H
            if (!(x >= 0 && y >= 0 && x <= m_Grid.GetGridSize().x && y <= m_Grid.GetGridSize().y))
                return false;

            //�����̐�L�O���b�h���X�g
            var gridPositionList = GetGridPositionList(m_BuildingData, new Vector2Int(x, y), m_Dir);

            //�����̖��O���b�h�̓}�b�v�̒��ɂ���̂�
            foreach (var gridPosition in gridPositionList)
            {
                if (!(gridPosition.x >= m_Grid.GetGridSize().x) &&
                    !(gridPosition.y >= m_Grid.GetGridSize().y)) continue;
                return false;
            }


            //�O���b�h�̒��ɂ�����������܂���
            foreach (var gridPosition in gridPositionList)
            {
                var gObj = m_Grid.GetGridObjectByXY(gridPosition.x, gridPosition.y);
                if (gObj.IsEmpty && gObj.TerrainData.resType == m_BuildingData.NeedResource) continue;
                return false;
            }

            return true;
        }


        /// <summary>
        ///     runtime�����\�z
        /// </summary>
        /// <param name="buildingData_"></param>
        /// <param name="buildingType_"></param>
        /// <param name="gridPosition_"></param>
        /// <param name="dir_"></param>
        private void CreatBuilding(BuildingData buildingData_, BuildingType buildingType_, Vector2Int gridPosition_,
            Dir dir_)
        {
            // ��_�̃I�t�Z�b�g���擾
            var rotationOffset = GetRotationOffset(dir_);
            // ���������[���h���W���擾
            var buildingObjectWorldPosition = m_Grid.GetWorldPosition3D(gridPosition_.x, gridPosition_.y) +
                                              new Vector3(rotationOffset.x, 0, rotationOffset.y) *
                                              m_Grid.GetCellSize;

            var gridPositionList = GetGridPositionList(buildingData_, gridPosition_, dir_);

            // �ꎞ�I��GridObject���X�g���쐬
            var tempGridObjList = new List<GridObject>(gridPositionList.Count);
            foreach (var gridPosition in gridPositionList)
            {
                var gridObj = m_Grid.GetGridObjectByXY(gridPosition.x, gridPosition.y);
                gridObj.IsEmpty = false;
                tempGridObjList.Add(gridObj);
            }

            // �����̎��̂𐶐�
            var tempBuilding = Object.Instantiate(buildingData_.Prefab,
                buildingObjectWorldPosition,
                Quaternion.Euler(0, GetRotationAngle(dir_), 0));
            var buildingBase = tempBuilding.GetComponent<BuildingBase>();
            buildingBase.Init(buildingData_, tempGridObjList, gridPosition_, dir_);
            this.GetModel<IBuildingObjModel>().RegisterBuild(tempBuilding.gameObject.GetInstanceID(), buildingBase);
        }


        private void RemoveWorkers(IResourceDataModel resDataModel, IBuildingObjModel buildObjModel,
            int removeWorkerNums)
        {
            var tempNums = resDataModel.GetRes(ResourceType.Worker);
            removeWorkerNums -= tempNums;
            resDataModel.MinusRes(ResourceType.Worker, tempNums);

            if (removeWorkerNums == 0)
                return;
            var tempList = buildObjModel.GetBuildDataList(BuildingType.Factory);
            foreach (var buildingObj in tempList)
            {
                if (buildingObj.WorkerNum <= 0) continue;

                var deduction = Mathf.Min(buildingObj.WorkerNum, removeWorkerNums);
                buildingObj.WorkerNum -= deduction;
                removeWorkerNums -= deduction;

                if (removeWorkerNums == 0) break;
            }
        }

        #endregion
    }
}