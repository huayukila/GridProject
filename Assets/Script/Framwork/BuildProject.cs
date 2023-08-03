namespace Framework.BuildProject
{
    public class BuildProject : Architecture<BuildProject>
    {
        protected override void Init()
        {
            RegisterModel<IBuilDataModel>(new BuildDataModel());
            RegisterModel<IResourceDataModel>(new ResourceDataModel());
            RegisterModel<IBuildingModel>(new BuildingModel());


            RegisterSystem<IGridBuildSystem>(new GridBuildSystem());
        }
    }
}
