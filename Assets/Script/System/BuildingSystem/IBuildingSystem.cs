namespace Framework.BuildProject
{
    interface IBuildingSystem : ISystem
    {
        void AddWorker(int buildingID);
        void RemoveWorker(int buildingID);
        bool BuildingGetDmg(int buildingID, int damage);
    }

    public class BuildingSystem : AbstractSystem, IBuildingSystem
    {
        protected override void OnInit()
        {
        }


        public void AddWorker(int buildingID)
        {
            BuildingBase @base = this.GetModel<IBuildingObjModel>().GetBuildData(buildingID);
            int resWorker=this.GetModel<IResourceDataModel>().GetRes(ResourceType.Worker);
            if (@base.m_WorkerNum < @base.m_MaxWorkerNum &&  resWorker> 0)
            {
                @base.m_WorkerNum++;
                this.GetModel<IResourceDataModel>().MinusRes(ResourceType.Worker, 1);
            }
        }

        public void RemoveWorker(int buildingID)
        {
            BuildingBase @base = this.GetModel<IBuildingObjModel>().GetBuildData(buildingID);
            if (@base.m_WorkerNum > 0)
            {
                @base.m_WorkerNum--;
                this.GetModel<IResourceDataModel>().AddRes(ResourceType.Worker, 1);
            }
        }

        public bool BuildingGetDmg(int buildingID, int damage)
        {
            return false;
        }
    }
}