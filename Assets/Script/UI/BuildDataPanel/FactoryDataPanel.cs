using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class FactoryDataPanel : BuildDataPanelBase
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
                $"Name:{m_BuildBase.BuildingName}\nLevel:{m_BuildBase.BuildingLevel}\nHP:{m_BuildBase.BuildingHp}\nWorkerNums:{m_BuildBase.m_WorkerNum}/{m_BuildBase.m_MaxWorkerNum}";
        }

        void AddWorker()
        {
            this.GetSystem<IBuildingSystem>().AddWorker(m_BuildBase.gameObject);
            ShowData();
        }

        void RemoveWorker()
        {
            this.GetSystem<IBuildingSystem>().RemoveWorker(m_BuildBase.gameObject);
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