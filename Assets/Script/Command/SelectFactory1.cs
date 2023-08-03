namespace Framework.BuildProject
{
    public class SelectFactory1 : AbstractCommand
    {
        BuildingData buildingData;
        protected override void OnExecute()
        {
            buildingData = this.GetModel<IBuilDataModel>().GetBuildingConfig("Factory1");
            if (this.GetModel<IResourceDataModel>().IsResEnough(buildingData.LevelDatasList[0].costList))
            {
                this.GetSystem<IGridBuildSystem>().ChangeToBuildState();
                this.GetSystem<IGridBuildSystem>().SelectBuilding(buildingData);
            }
        }
    }
}