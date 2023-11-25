using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class FactoryDataPanel : BuildDataBasePanel
    {
        public Button AddButton;

        public Button RemoveButton;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            AddButton.onClick.AddListener(AddWorker);
            RemoveButton.onClick.AddListener(RemoveWorker);
        }

        // Update is called once per frame
        void Update()
        {
        }

        protected override void ShowData()
        {
            DataText.text =
                $"Name:{m_BuildObj.m_BuildingName}\nLevel:{m_BuildObj.m_BuildingLevel}\nHP:{m_BuildObj.m_BuildingHp}\nWorkerNums:{m_BuildObj.m_WorkerNum}/{m_BuildObj.m_MaxWorkerNum}";
        }

        void AddWorker()
        {
            this.SendCommand(new AddWorker(m_BuildObj.GetGameObj().GetInstanceID()));
            ShowData();
        }

        void RemoveWorker()
        {
            this.SendCommand(new RemoveWorker(m_BuildObj.GetGameObj().GetInstanceID()));
            ShowData();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AddButton.onClick.RemoveAllListeners();
            RemoveButton.onClick.RemoveAllListeners();
        }
    }
}