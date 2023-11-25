using UnityEngine;

namespace Framework.BuildProject
{
    public class BuildDataPanelManager : BuildController
    {
        public BuildDataBasePanel FactoryPanel;
        public BuildDataBasePanel HousePanel;

        BuildDataBasePanel m_FocusPanel;

        public void OpenPanel(GameObject gameObject)
        {
            if (m_FocusPanel != null && m_FocusPanel.gameObject.activeSelf)
            {
                m_FocusPanel.CloseLabe();
                m_FocusPanel = null;
            }

            BuildingType tempType = this.GetModel<IBuildingObjModel>().GetBuildData(gameObject.GetInstanceID())
                .m_BuildingType;
            switch (tempType)
            {
                case BuildingType.Factory:
                    m_FocusPanel = FactoryPanel;
                    m_FocusPanel.gameObject.SetActive(true);
                    m_FocusPanel.OpenLabe(gameObject);
                    break;
                case BuildingType.House:
                    m_FocusPanel = HousePanel;
                    m_FocusPanel.gameObject.SetActive(true);
                    m_FocusPanel.OpenLabe(gameObject);
                    break;
            }
        }

        public void ClosePanel()
        {
            if (m_FocusPanel == null)
                return;
            m_FocusPanel.CloseLabe();
            m_FocusPanel = null;
        }
    }
}