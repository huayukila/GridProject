using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class HouseDataPanel : BuildDataPanelBase
    {
        public Text WorkText;

        protected override void ShowData()
        {
            NameText.text = $"{m_BuildBase.BuildingName}";
            WorkText.text = $"{m_BuildBase.MaxWorkerNum}";
            HpText.text = $"{m_BuildBase.BuildingHp}/{m_BuildBase.BuildingMaxHp}";
        }
    }
}