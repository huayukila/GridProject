using UnityEngine;
namespace Framework.BuildProject
{
    public class SelectHouse : AbstractCommand
    {
        BuildingData buildingData;
        protected override void OnExecute()
        {
            buildingData = this.GetModel<IBuilDataModel>().GetBuildingConfig("House");
            if (this.GetModel<IResourceDataModel>().IsResEnough(buildingData.LevelDatasList[0].costList))
            {
                this.GetSystem<IGridBuildSystem>().ChangeToBuildState();
                this.GetSystem<IGridBuildSystem>().SelectBuilding(buildingData);
            }
        }
    }
}