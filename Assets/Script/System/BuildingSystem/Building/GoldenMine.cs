namespace Framework.BuildProject
{
    public class GoldenMine : UpdateBuilding
    {
        protected override void OnExecute()
        {
            this.GetModel<IResourceDataModel>().AddRes(ResourceType.Gold, WorkerNum * 20);
            this.SendEvent<RefreshResPanel>();
        }
    }
}