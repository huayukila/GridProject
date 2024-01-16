using System.Collections.Generic;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildMenu : BuildController
    {
        public List<Button> buttonList;
        // Start is called before the first frame update
        void Start()
        {
            buttonList[0].onClick.AddListener(() => SelectBuilding(BuildingType.House));
            buttonList[1].onClick.AddListener(() => SelectBuilding(BuildingType.Factory));
            buttonList[2].onClick.AddListener(() => SelectBuilding(BuildingType.BallistaTower));
        }

        private void OnDestroy()
        {
            foreach (Button button in buttonList)
            {
                button.onClick.RemoveAllListeners();
            }

            buttonList.Clear();
        }

        void SelectBuilding(BuildingType buildingType_)
        {
            this.GetModel<IPlayerDataModel>().playerState = PlayerState.Build;
            BuildingData buildingData =
                this.GetModel<IBuilDataModel>().GetBuildingConfig(buildingType_);

            if (this.GetModel<IResourceDataModel>().IsResEnough(buildingData.m_LevelDatasList[0].m_CostList))
                this.GetSystem<IGridBuildSystem>().SelectBuilding(buildingData);
        }
    }
}