using UnityEngine;

namespace Framework.BuildProject
{
    // �����V�X�e���̃C���^�[�t�F�[�X
    interface IBuildingSystem : ISystem
    {
        void AddWorker(GameObject obj_);
        void RemoveWorker(GameObject obj_);
        void RepairAllBuilding();
        void RepairBuilding();
    }

    // �����V�X�e���̎���
    public class BuildingSystem : AbstractSystem, IBuildingSystem
    {
        private IBuildingObjModel m_BuildingObjModel; // �����I�u�W�F�N�g���f��
        private IResourceDataModel m_ResourceDataModel; // �����f�[�^���f��

        // ����������
        protected override void OnInit()
        {
            m_BuildingObjModel = this.GetModel<IBuildingObjModel>();
            m_ResourceDataModel = this.GetModel<IResourceDataModel>();
        }

        // �������ǉ����鏈��
        public void AddWorker(GameObject obj_)
        {
            BuildingBase buildingBase = m_BuildingObjModel.GetBuildData(obj_.GetInstanceID());
            int resWorker = m_ResourceDataModel.GetRes(ResourceType.Worker);
            if (buildingBase.WorkerNum < buildingBase.MaxWorkerNum && resWorker > 0)
            {
                buildingBase.WorkerNum++;
                m_ResourceDataModel.MinusRes(ResourceType.Worker, 1);
            }
        }

        // ��������폜���鏈��
        public void RemoveWorker(GameObject obj_)
        {
            BuildingBase buildingBase = m_BuildingObjModel.GetBuildData(obj_.GetInstanceID());
            if (buildingBase.WorkerNum > 0)
            {
                buildingBase.WorkerNum--;
                m_ResourceDataModel.AddRes(ResourceType.Worker, 1);
            }
        }

        // ���ׂĂ̌������C������i���݋�̃��\�b�h�j
        public void RepairAllBuilding()
        {
            // �����\��̏���
        }

        // ����̌������C������i���݋�̃��\�b�h�j
        public void RepairBuilding()
        {
            // �����\��̏���
        }
    }
}