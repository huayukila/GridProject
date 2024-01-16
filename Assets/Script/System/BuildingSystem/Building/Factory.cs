namespace Framework.BuildProject
{
    public class Factory : UpdateBuilding
    {
        protected override void OnExecute()
        {
            this.GetModel<IResourceDataModel>().AddRes(ResourceType.Stone,20);
        }
    }
}