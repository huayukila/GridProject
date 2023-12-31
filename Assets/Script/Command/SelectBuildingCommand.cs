namespace Framework.BuildProject
{
    public class SelectBuildingCommand<T> : AbstractCommand where T : BuildingBase
    {
        private BuildingData buildingData;

        protected override void OnExecute()
        {
            buildingData = this.GetModel<IBuilDataModel>().GetBuildingConfig(typeof(T).Name);

            if (this.GetModel<IResourceDataModel>().IsResEnough(buildingData.m_LevelDatasList[0].m_CostList))
                this.GetSystem<IGridBuildSystem>().SelectBuilding<T>(buildingData);
        }
    }
}