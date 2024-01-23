using UnityEngine;

namespace Framework.BuildProject
{
    public class BuildDataPanelManager : BuildController
    {
        public BuildDataPanelBase factoryPanel;
        public BuildDataPanelBase housePanel;
        public BuildDataPanelBase centerCorePanel;
        private GameObject m_CurrentBuilding;
        private BuildDataPanelBase m_FocusPanelBase;

        private void Start()
        {
            this.RegisterEvent<BuildingHasBeenDestroyEvent>(e =>
            {
                if (m_FocusPanelBase == null)
                    return;
                if (e.id == m_CurrentBuilding.GetInstanceID())
                {
                    ClosePanel();
                    m_CurrentBuilding = null;
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);
        }

        public void OpenPanel(GameObject gameObject)
        {
            m_CurrentBuilding = gameObject;
            if (m_FocusPanelBase != null && m_FocusPanelBase.gameObject.activeSelf)
            {
                m_FocusPanelBase.CloseLabe();
                m_FocusPanelBase = null;
            }

            var tempType = this.GetModel<IBuildingObjModel>().GetBuildData(gameObject.GetInstanceID())
                .BuildingType;
            switch (tempType)
            {
                case BuildingType.Factory:
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