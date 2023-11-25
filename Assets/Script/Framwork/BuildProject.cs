namespace Framework.BuildProject
{
    public class BuildProject : Architecture<BuildProject>
    {
        protected override void Init()
        {
            RegisterModel<IBuilDataModel>(new BuildDataModel());
            RegisterModel<IResourceDataModel>(new ResourceDataModel());
            RegisterModel<IBuildingObjModel>(new BuildingObjModel());
            RegisterSystem<IGridBuildSystem>(new GridBuildSystem());
            RegisterSystem<IBuildingSystem>(new BuildingSystem());
        }
    }
}
