namespace Framework.BuildProject
{
    public class Factory : BuildingObj
    {
        protected override void OnUpdate()
        {
            this.GetModel<IResourceDataModel>().AddRes(ResourceType.Stone,20);
            
        }
    }
}