using System.Globalization;
using Kit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class PreparePanel : BuildController
    {
        public Button StartStageButton;
        public TextMeshProUGUI CounterText;
        public GameObject ShowStartImg;
        public GameObject ShowObj;

        private bool m_IsCountDownStart;
        private float m_PreCD;

        private void Awake()
        {
            StartStageButton.onClick.AddListener(() =>
            {
                this.SendEvent<StartStage>();
                m_IsCountDownStart = false;
                ShowObj.SetActive(false);
            });
            m_IsCountDownStart = false;
            ShowStartImg.SetActive(true);
            CounterText.text = Global.PREPARE_TIME.ToString(CultureInfo.CurrentCulture);
        }

        // Start is called before the first frame update
        private void Start()
        {
            StartStageButton.gameObject.SetActive(false);
            m_PreCD = Global.PREPARE_TIME;
            this.RegisterEvent<StageClearEvent>(e =>
            {
                m_PreCD = Global.PREPARE_TIME;
                ShowObj.SetActive(true);
                m_IsCountDownStart = true;
            }).UnregisterWhenGameObjectDestroyed(gameObject);
            ActionKit.Delay(1, () =>
            {
                StartStageButton.gameObject.SetActive(true);
                m_IsCountDownStart = true;
                ShowStartImg.gameObject.SetActive(false);
            }).Start(this);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!m_IsCountDownStart)
                return;
            m_PreCD -= Time.deltaTime;

            if (m_PreCD <= 0)
            {
                ShowObj.SetActive(false);
                m_IsCountDownStart = false;
                this.SendEvent<StartStage>();
            }
        }

        private void OnDestroy()
        {
            StartStageButton.onClick.RemoveAllListeners();
        }
    }
}