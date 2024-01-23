using UnityEngine;

namespace Framework.BuildProject
{
    public class PlayerController : BuildController
    {
        public BuildDataPanelManager m_BuildDataPanelManager;
        private IGridBuildSystem m_GridBuildSystem;
        private IPlayerDataModel m_PlayerDataModel;

        // �X�^�[�g���̏�����
        private void Start()
        {
            m_GridBuildSystem = this.GetSystem<IGridBuildSystem>();
            m_PlayerDataModel = this.GetModel<IPlayerDataModel>();
        }

        // ���t���[���̍X�V
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

        // �m�[�}����Ԃ̏���
        private void HandleNormalState()
        {
            if (Input.GetMouseButton(0)) TryOpenBuildPanel();

            if (Input.GetMouseButton(1)) m_BuildDataPanelManager.ClosePanel();
        }

        // �r���h��Ԃ̏���
        private void HandleBuildState()
        {
            m_GridBuildSystem.VisualBuildingFollowMouse();

            if (Input.GetMouseButtonDown(0)) TrySetBuilding();

            if (Input.GetMouseButtonDown(1)) m_GridBuildSystem.CancelSelect();

            if (Input.GetKeyDown(KeyCode.R)) m_GridBuildSystem.BuildingRota();
        }

        // ���z�p�l�����J������
        private void TryOpenBuildPanel()
        {
            if (!Utils.IsMouseOverUI())
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask(Global.TARGET_STRING_BUILDING)))
                    m_BuildDataPanelManager.OpenPanel(hit.transform.parent.gameObject);
            }
        }

        // ���z�ݒ�����݂�
        private void TrySetBuilding()
        {
            if (!Utils.IsMouseOverUI()) m_GridBuildSystem.SetBuilding();
        }
    }
}