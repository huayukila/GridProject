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
            buttonList[0].onClick.AddListener(() => SelectBuilding("House"));
            buttonList[1].onClick.AddListener(() => SelectBuilding("Sawmill"));
            buttonList[2].onClick.AddListener(() => SelectBuilding("StoneMine"));
            buttonList[3].onClick.AddListener(() => SelectBuilding("GoldenMine"));
            buttonList[4].onClick.AddListener(() => SelectBuilding("BallistaTower"));
            buttonList[5].onClick.AddListener(() => SelectBuilding("MagicTower"));
            buttonList[6].onClick.AddListener(() => SelectBuilding("CannonTower"));
        }

        private void OnDestroy()
        {
            foreach (Button button in buttonList)
            {
                button.onClick.RemoveAllListeners();
            }

            buttonList.Clear();
        }

        void SelectBuilding(string name_)
        {
            this.GetModel<IPlayerDataModel>().playerState = PlayerState.Build;
            BuildingData buildingData =
                this.GetModel<IBuilDataModel>().GetBuildingConfig(name_);

            if (this.GetModel<IResourceDataModel>().IsResEnough(buildingData.LevelDatasList[0].CostList))
                this.GetSystem<IGridBuildSystem>().SelectBuilding(buildingData);
        }
    }
}