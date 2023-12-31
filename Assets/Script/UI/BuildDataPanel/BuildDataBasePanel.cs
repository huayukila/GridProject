using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildDataBasePanel : BuildController
    {
        public Button DeletButton;
        public Text DataText;
        protected BuildingBase MBuildBase;
        protected IBuildingObjModel m_DataModel;

        // Start is called before the first frame update
        void Awake()
        {
            m_DataModel = this.GetModel<IBuildingObjModel>();
        }

        protected virtual void Start()
        {
            DeletButton.onClick.AddListener(DeleteBuilding);
        }

        // Update is called once per frame
        void Update()
        {
        }


        void DeleteBuilding()
        {
            this.SendCommand(new DeleteBuildingCmd(MBuildBase.GetGameObj()));
            gameObject.SetActive(false);
        }

        protected virtual void ShowData()
        {
        }

        public void OpenLabe(GameObject obj)
        {
            MBuildBase = m_DataModel.GetBuildData(obj.GetInstanceID());
            if (MBuildBase == null)
                return;
            gameObject.SetActive(true);
            ShowData();
        }

        public void CloseLabe()
        {
            gameObject.SetActive(false);
            MBuildBase = null;
        }

        protected virtual void OnDestroy()
        {
            DeletButton.onClick.RemoveAllListeners();
        }
    }
}