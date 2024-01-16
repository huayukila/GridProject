using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Kit;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class PreparePanel : BuildController
    {
        public Button StartStageButton;
        public Text CounterText;
        public Text ShowMsgTxt;
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
            ShowMsgTxt.text = "Game Start";
            CounterText.text = Global.PREPARE_TIME.ToString(CultureInfo.CurrentCulture);
        }

        // Start is called before the first frame update
        void Start()
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
                ShowMsgTxt.gameObject.SetActive(false);
            }).Start(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_IsCountDownStart)
                return;
            m_PreCD -= Time.deltaTime;
            CounterText.text = Math.Round(m_PreCD, 1).ToString(CultureInfo.CurrentCulture);
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