using UnityEngine;
namespace Framework.BuildProject
{
    public class RemoveWorker : AbstractCommand
    {
        GameObject buildingObj;
        Buildingobj obj;
        protected override void OnExecute()
        {
            obj = this.GetModel<IBuildingModel>().GetBuildData(buildingObj);
            if (obj.WorkerNum > 0)
            {
                this.GetModel<IBuildingModel>().RemoveWorker(buildingObj);
                this.GetModel<IResourceDataModel>().RiseResources(ResourceType.Worker, 1);
                this.SendEvent<RefreshResPanel>();
            }
        }
        public RemoveWorker(GameObject buildObj)
        {
            buildingObj = buildObj;
        }
    }
}