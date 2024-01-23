using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class ResoureceDataTopPanel : BuildController
    {
        public Text WoodTxt;
        public Text StoneTxt;
        public Text GoldTxt;
        public Text IdleWorkerTxt;
        public Text MaxWorkerTxt;
        private IResourceDataModel dataModel;

        private float m_DurationTime;


        // Start is called before the first frame update
        private void Start()
        {
            dataModel = this.GetModel<IResourceDataModel>();
            RefreshResPanel();
            this.RegisterEvent<RefreshResPanel>(e => RefreshResPanel()
            ).UnregisterWhenGameObjectDestroyed(gameObject);
            m_DurationTime = Time.time;
        }


        private void Update()
        {
            if (Time.time - m_DurationTime < 1.0f) return;

            m_DurationTime = Time.time;
            RefreshResPanel();
        }

        private void RefreshResPanel()
        {
            MaxWorkerTxt.text = dataModel.MaxWorkerNum.ToString();
            WoodTxt.text = dataModel.GetRes(ResourceType.Wood).ToString();
            StoneTxt.text = dataModel.GetRes(ResourceType.Stone).ToString();
            GoldTxt.text = dataModel.GetRes(ResourceType.Gold).ToString();
            IdleWorkerTxt.text = dataModel.GetRes(ResourceType.Worker).ToString();
        }
    }
}