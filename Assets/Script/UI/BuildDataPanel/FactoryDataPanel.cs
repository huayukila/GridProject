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
                $"Name:{MBuildBase.BuildingName}\nLevel:{MBuildBase.BuildingLevel}\nHP:{MBuildBase.BuildingHp}\nWorkerNums:{MBuildBase.m_WorkerNum}/{MBuildBase.m_MaxWorkerNum}";
        }

        void AddWorker()
        {
            this.SendCommand(new AddWorker(MBuildBase.GetGameObj().GetInstanceID()));
            ShowData();
        }

        void RemoveWorker()
        {
            this.SendCommand(new RemoveWorker(MBuildBase.GetGameObj().GetInstanceID()));
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