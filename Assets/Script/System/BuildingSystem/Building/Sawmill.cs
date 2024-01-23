namespace Framework.BuildProject
{
    public class Sawmill : UpdateBuilding
    {
        protected override void OnExecute()
        {
            this.GetModel<IResourceDataModel>().AddRes(ResourceType.Wood, WorkerNum * 40);
            this.SendEvent<RefreshResPanel>();
        }
    }
}