using UnityEngine;

namespace Framework.BuildProject
{
    public class PlayerController : BuildController
    {
        public BuildDataPanelManager m_BuildDataPanelManager;
        private IGridBuildSystem m_GridBuildSystem;
        private IPlayerDataModel m_PlayerDataModel;

        // スタート時の初期化
        private void Start()
        {
            m_GridBuildSystem = this.GetSystem<IGridBuildSystem>();
            m_PlayerDataModel = this.GetModel<IPlayerDataModel>();
        }

        // 毎フレームの更新
        private void Update()
        {
            switch (m_PlayerDataModel.playerState)
            {
                case PlayerState.Normal:
                    HandleNormalState();
                    break;
                case PlayerState.Build:
                    HandleBuildState();
                    break;
            }
        }

        // ノーマル状態の処理
        private void HandleNormalState()
        {
            if (Input.GetMouseButton(0)) TryOpenBuildPanel();

            if (Input.GetMouseButton(1)) m_BuildDataPanelManager.ClosePanel();
        }

        // ビルド状態の処理
        private void HandleBuildState()
        {
            m_GridBuildSystem.VisualBuildingFollowMouse();

            if (Input.GetMouseButtonDown(0)) TrySetBuilding();

            if (Input.GetMouseButtonDown(1)) m_GridBuildSystem.CancelSelect();

            if (Input.GetKeyDown(KeyCode.R)) m_GridBuildSystem.BuildingRota();
        }

        // 建築パネルを開く試み
        private void TryOpenBuildPanel()
        {
            if (!Utils.IsMouseOverUI())
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask(Global.TARGET_STRING_BUILDING)))
                    m_BuildDataPanelManager.OpenPanel(hit.transform.parent.gameObject);
            }
        }

        // 建築設定を試みる
        private void TrySetBuilding()
        {
            if (!Utils.IsMouseOverUI()) m_GridBuildSystem.SetBuilding();
        }
    }
}