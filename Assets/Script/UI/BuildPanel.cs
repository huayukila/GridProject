using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildPanel : BuildController
    {
        public Button AddButton;
        public Button RemoveButton;
        public Text DataText;
        public GameObject DataLabe;
        GameObject BuildObj;
        // Start is called before the first frame update
        void Start()
        {
            AddButton.onClick.AddListener(AddWorker);
            RemoveButton.onClick.AddListener(RemoveWorker);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void AddWorker()
        {
            this.SendCommand(new AddWorker(BuildObj));
            ShowData();
        }

        void RemoveWorker()
        {
            this.SendCommand(new RemoveWorker(BuildObj));
            ShowData();
        }
        public void ShowData()
        {
            Buildingobj obj = this.GetModel<IBuildingModel>().GetBuildData(BuildObj);
            if (obj != null)
            {
                DataText.text = "HP:" + obj.BuildingHp + "\nLevel:" + obj.BuildingLevel + "\nWorkerNums:" + obj.WorkerNum+"/"+obj.MaxWorkerNum;
            }
        }
        public void OpenLabe(GameObject obj)
        {
            BuildObj = obj;
            DataLabe.SetActive(true);
            ShowData();
        }
        public void CloseLabe()
        {
            DataLabe.SetActive(false);
        }

        private void OnDestroy()
        {
            AddButton.onClick.RemoveAllListeners();
            RemoveButton.onClick.RemoveAllListeners();
        }
    }
}