using UnityEngine;

namespace Framework.BuildProject
{
    public class PlayerController : BuildController
    {
        IGridBuildSystem m_GridBuildSystem;
        private IPlayerDataModel m_PlayerDataModel;

        public BuildDataPanelManager m_BuildDataPanelManager;


        private void Start()
        {
            m_GridBuildSystem = this.GetSystem<IGridBuildSystem>();
            m_PlayerDataModel = this.GetModel<IPlayerDataModel>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (m_PlayerDataModel.playerState)
            {
                case PlayerState.Normal:
                    if (Input.GetMouseButton(0))
                    {
                        if (!Utils.IsMouseOverUI())
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out var hit, 1000,
                                    LayerMask.GetMask(Global.TARGET_STRING_BUILDING))
                               )
                            {
                                m_BuildDataPanelManager.OpenPanel(hit.transform.parent.gameObject);
                            }
                        }
                    }

                    if (Input.GetMouseButton(1))
                    {
                        m_BuildDataPanelManager.ClosePanel();
                    }

                    break;
                case PlayerState.Build:
                    m_GridBuildSystem.VisualBuildingFollowMouse();
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!Utils.IsMouseOverUI())
                        {
                            m_GridBuildSystem.SetBuilding();
                        }
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        m_GridBuildSystem.CancelSelect();
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        m_GridBuildSystem.BuildingRota();
                    }

                    break;
            }
        }
    }
}