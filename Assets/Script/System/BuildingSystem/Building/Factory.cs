namespace Framework.BuildProject
{
    public class Factory : UpdateBuilding
    {
        protected override void OnUpdate()
        {
            this.GetModel<IResourceDataModel>().AddRes(ResourceType.Stone,20);
        }
    }
}