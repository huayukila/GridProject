using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class SubEndPanel : BuildController
    {
        public RectTransform winWindowTrans;
        public Button winBtn;
        public RectTransform failWindowTrans;
        public Button failBtn;

        private IPlayerDataModel m_PlayerDataModel;
        private Vector3 m_TargetPos = Vector3.zero;

        private void Awake()
        {
            winBtn.onClick.AddListener(ReturnToTitle);
            failBtn.onClick.AddListener(ReturnToTitle);
            m_PlayerDataModel = this.GetModel<IPlayerDataModel>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_PlayerDataModel.playerState == PlayerState.Win)
            {
                winWindowTrans.gameObject.SetActive(true);
                if (Vector3.Distance(winWindowTrans.localPosition, m_TargetPos) < 0.1f)
                    return;
                winWindowTrans.localPosition = Vector3.Slerp(winWindowTrans.localPosition, m_TargetPos,
                    2f * Time.unscaledDeltaTime);
            }

            if (m_PlayerDataModel.playerState == PlayerState.Fail)
            {
                failWindowTrans.gameObject.SetActive(true);
                if (Vector3.Distance(failWindowTrans.localPosition, m_TargetPos) < 0.1f)
                    return;
                failWindowTrans.localPosition = Vector3.Slerp(failWindowTrans.localPosition, m_TargetPos,
                    2f * Time.unscaledDeltaTime);
            }
        }

        void ReturnToTitle()
        {
            this.SendEvent<BackToTitleEvent>();
        }

        private void OnDestroy()
        {
            winBtn.onClick.RemoveAllListeners();
            failBtn.onClick.RemoveAllListeners();
        }
    }
}