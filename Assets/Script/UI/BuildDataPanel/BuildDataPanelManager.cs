using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.BuildProject
{
    public class BuildDataPanelManager : BuildController
    {
        public BuildDataPanelBase factoryPanel;
        public BuildDataPanelBase housePanel;
        public BuildDataPanelBase centerCorePanel;
        BuildDataPanelBase m_FocusPanelBase;

        public void OpenPanel(GameObject gameObject)
        {
            if (m_FocusPanelBase != null && m_FocusPanelBase.gameObject.activeSelf)
            {
                m_FocusPanelBase.CloseLabe();
                m_FocusPanelBase = null;
            }

            BuildingType tempType = this.GetModel<IBuildingObjModel>().GetBuildData(gameObject.GetInstanceID())
                .BuildingType;
            switch (tempType)
            {
                case BuildingType.CannonTower:
                case BuildingType.MagicTower:
                case BuildingType.BallistaTower:
                case BuildingType.Sawmill:
                case BuildingType.StoneMine:
                case BuildingType.GoldenMine:
                    m_FocusPanelBase = factoryPanel;
                    break;
                case BuildingType.Core:
                    m_FocusPanelBase = centerCorePanel;
                    break;
                case BuildingType.House:
                    m_FocusPanelBase = housePanel;
                    break;
            }

            m_FocusPanelBase.gameObject.SetActive(true);
            m_FocusPanelBase.OpenLabe(gameObject);
        }

        public void ClosePanel()
        {
            if (m_FocusPanelBase == null)
                return;
            m_FocusPanelBase.CloseLabe();
            m_FocusPanelBase = null;
        }
    }
}