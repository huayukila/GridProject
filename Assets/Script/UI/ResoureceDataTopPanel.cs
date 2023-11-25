using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class ResoureceDataTopPanel : BuildController
    {
        public Text resoreceDataTxt;

        private float m_DurationTime;
        IResourceDataModel dataModel;
        

        // Start is called before the first frame update
        void Start()
        {
            dataModel = this.GetModel<IResourceDataModel>();
            RefreshResPanel();
            this.RegisterEvent<RefreshResPanel>(e => RefreshResPanel()
            ).UnregisterWhenGameObjectDestroyed(gameObject);
            m_DurationTime = Time.time;
        }


        private void Update()
        {
            if (Time.time - m_DurationTime < 1.0f)
            {
                return;
            }
            RefreshResPanel();
        }

        void RefreshResPanel()
        {
            resoreceDataTxt.text = "Mood:" + dataModel.GetRes(ResourceType.Wood) + " Stone:" +
                                   dataModel.GetRes(ResourceType.Stone)
                                   + " Gold:" + dataModel.GetRes(ResourceType.Gold) + " Idle Population:" +
                                   dataModel.GetRes(ResourceType.Worker)+"/Max Population:"+dataModel.MaxWorkerNum;
        }
    }
}