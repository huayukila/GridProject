using UnityEngine;

namespace Framework.BuildProject
{
    public class PlayerController : BuildController
    {
        IGridBuildSystem gridBuildSystem;

        public BuildPanel buildPanel;
        // Start is called before the first frame update
        private void Awake()
        {
            gridBuildSystem = this.GetSystem<IGridBuildSystem>();
            gridBuildSystem.CreatGrid(10, 10, 10f);
        }
        void Start()
        {
            buildPanel.CloseLabe();
        }
        // Update is called once per frame
        void Update()
        {
            switch (gridBuildSystem.state.Value)
            {
                case State.Normal:
                    if (Input.GetMouseButton(0))
                    {
                        if (!UtilsClass.Instance.IsMouseOverUI())
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;
                            Physics.Raycast(ray, out hit);
                            if (hit.transform.tag == "Building")
                            {
                                buildPanel.OpenLabe(hit.transform.gameObject);
                            }
                        }
                    }
                    if (Input.GetMouseButton(1))
                    {
                        buildPanel.CloseLabe();
                    }
                    break;
                case State.Build:
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
                        gridBuildSystem.ChangeToNormalState();
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        gridBuildSystem.BuildingRota();
                    }
                    break;
                default:
                    break;
            }
        }
        void OnDestroy()
        {
            gridBuildSystem.state.Unregister(e => { });
        }
    }
}