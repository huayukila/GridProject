using UnityEngine;

namespace Framework.BuildProject
{
    public class AddWorker : AbstractCommand
    {
        GameObject buildObj;
        Buildingobj obj;
        int workerRes;
        protected override void OnExecute()
        {
            obj = this.GetModel<IBuildingModel>().GetBuildData(buildObj);
            workerRes = this.GetModel<IResourceDataModel>().GetRes(ResourceType.Worker);
            if ( workerRes> 0&&obj.WorkerNum<obj.MaxWorkerNum)
            {
                this.GetModel<IBuildingModel>().AddWorker(buildObj);
                this.GetModel<IResourceDataModel>().DeductResources(ResourceType.Worker, 1);
                this.SendEvent<RefreshResPanel>();
            }
        }
        public AddWorker(GameObject buildObj)
        {
            this.buildObj = buildObj;
        }
    }
}