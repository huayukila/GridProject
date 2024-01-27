using TMPro;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class FactoryDataPanel : BuildDataPanelBase
    {
        public Button AddButton;

        public Button RemoveButton;

        public TextMeshProUGUI WorkText;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            AddButton.onClick.AddListener(AddWorker);
            RemoveButton.onClick.AddListener(RemoveWorker);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AddButton.onClick.RemoveAllListeners();
            RemoveButton.onClick.RemoveAllListeners();
        }

        protected override void ShowData()
        {
            NameText.text = $"{m_BuildBase.BuildingName}";
            HpText.text = $"{m_BuildBase.BuildingHp}/{m_BuildBase.BuildingMaxHp}";
            WorkText.text = $"{m_BuildBase.WorkerNum}/{m_BuildBase.MaxWorkerNum}";
        }

        private void AddWorker()
        {
            this.GetSystem<IBuildingSystem>().AddWorker(m_BuildBase.gameObject);
            ShowData();
            this.SendEvent<RefreshResPanel>();
        }

        private void RemoveWorker()
        {
            this.GetSystem<IBuildingSystem>().RemoveWorker(m_BuildBase.gameObject);
            ShowData();
            this.SendEvent<RefreshResPanel>();
        }
    }
}