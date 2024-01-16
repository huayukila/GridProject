using UnityEngine;

namespace Framework.BuildProject
{
    interface IBuildingSystem : ISystem
    {
        void AddWorker(GameObject obj_);
        void RemoveWorker(GameObject obj_);
        void RepairAllBuilding();

        void RepairBuilding();
    }

    public class BuildingSystem : AbstractSystem, IBuildingSystem
    {
        private IBuildingObjModel m_BuildingObjModel;
        private IResourceDataModel m_ResourceDataModel;

        protected override void OnInit()
        {
            m_BuildingObjModel = this.GetModel<IBuildingObjModel>();
            m_ResourceDataModel = this.GetModel<IResourceDataModel>();
        }


        public void AddWorker(GameObject obj_)
        {
            BuildingBase buildingBase = m_BuildingObjModel.GetBuildData(obj_.GetInstanceID());
            int resWorker = m_ResourceDataModel.GetRes(ResourceType.Worker);
            if (buildingBase.m_WorkerNum < buildingBase.m_MaxWorkerNum && resWorker > 0)
            {
                buildingBase.m_WorkerNum++;
                m_ResourceDataModel.MinusRes(ResourceType.Worker, 1);
            }
        }

        public void RemoveWorker(GameObject obj_)
        {
            BuildingBase buildingBase = m_BuildingObjModel.GetBuildData(obj_.GetInstanceID());
            if (buildingBase.m_WorkerNum > 0)
            {
                buildingBase.m_WorkerNum--;
                m_ResourceDataModel.AddRes(ResourceType.Worker, 1);
            }
        }

        public void RepairAllBuilding()
        {
        }

        public void RepairBuilding()
        {
        }
    }
}