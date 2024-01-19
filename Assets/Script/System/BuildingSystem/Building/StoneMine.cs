namespace Framework.BuildProject
{
    public class StoneMine : UpdateBuilding
    {
        protected override void OnExecute()
        {
            this.GetModel<IResourceDataModel>().AddRes(ResourceType.Stone, m_WorkerNum * 30);
        }
    }
}