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
            buttonList[0].onClick.AddListener(this.SendCommand<SelectBuildingCommand<House>>);
            buttonList[1].onClick.AddListener(this.SendCommand<SelectBuildingCommand<Factory>>);
            buttonList[2].onClick.AddListener(this.SendCommand<SelectBuildingCommand<DefendTower>>);
        }

        private void OnDestroy()
        {
            foreach (Button button in buttonList)
            {
                button.onClick.RemoveAllListeners();
            }
            buttonList.Clear();
        }
    }
}