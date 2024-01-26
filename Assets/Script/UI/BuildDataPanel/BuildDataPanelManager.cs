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
        private Canvas m_ParentCanvas;

        private void Start()
        {
            m_ParentCanvas = transform.parent.GetComponent<Canvas>();
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

            MovePanel(Camera.main.WorldToScreenPoint(gameObject.transform.position));
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

        void MovePanel(Vector2 pos_)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ParentCanvas.GetComponent<RectTransform>(),
                pos_, m_ParentCanvas.worldCamera, out Vector2 localPoint);

            m_FocusPanelBase.GetComponent<RectTransform>().anchoredPosition =
                AdjustPositionToBeWithinScreen(localPoint, m_FocusPanelBase.GetComponent<RectTransform>());
        }


        Vector2 AdjustPositionToBeWithinScreen(Vector2 position, RectTransform panel)
        {
            Vector3[] canvasCorners = new Vector3[4];
            m_ParentCanvas.GetComponent<RectTransform>().GetWorldCorners(canvasCorners);

            float canvasWidth = canvasCorners[2].x - canvasCorners[0].x;
            float canvasHeight = canvasCorners[2].y - canvasCorners[0].y;

            Vector2 panelSize = panel.sizeDelta;

            if (position.x + panelSize.x > canvasWidth)
            {
                position.x = canvasWidth - panelSize.x;
            }

            if (position.y + panelSize.y > canvasHeight)
            {
                position.y = canvasHeight - panelSize.y;
            }

            return position;
        }
    }
}