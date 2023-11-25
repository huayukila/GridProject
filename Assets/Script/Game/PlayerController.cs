using UnityEngine;

namespace Framework.BuildProject
{
    public class PlayerController : BuildController
    {
        IGridBuildSystem gridBuildSystem;

        public BuildDataPanelManager m_BuildDataPanelManager;

        // Start is called before the first frame update
        private void Awake()
        {
            gridBuildSystem = this.GetSystem<IGridBuildSystem>();
            gridBuildSystem.CreatGrid(10, 10, 10f);
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            switch (gridBuildSystem.m_State.Value)
            {
                case PlayerState.Normal:
                    if (Input.GetMouseButton(0))
                    {
                        if (!UtilsClass.Instance.IsMouseOverUI())
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;
                            Physics.Raycast(ray, out hit);
                            if (hit.transform.tag == "Building")
                            {
                                m_BuildDataPanelManager.OpenPanel(hit.transform.gameObject);
                            }
                        }
                    }

                    if (Input.GetMouseButton(1))
                    {
                        m_BuildDataPanelManager.ClosePanel();
                    }

                    break;
                case PlayerState.Build:
                    gridBuildSystem.VisualBuildingFollowMouse();
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!UtilsClass.Instance.IsMouseOverUI())
                        {
                            gridBuildSystem.SetBuilding();
                        }
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        gridBuildSystem.CancelSelect();
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        gridBuildSystem.BuildingRota();
                    }

                    break;
            }
        }

        void OnDestroy()
        {
            gridBuildSystem.m_State.Unregister(e => { });
        }
    }
}