using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildDataBasePanel : BuildController
    {
        public Button DeletButton;
        public Text DataText;
        protected BuildingObj m_BuildObj;
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
            this.SendCommand(new DeleteBuildingCmd(m_BuildObj.GetGameObj()));
            gameObject.SetActive(false);
        }

        protected virtual void ShowData()
        {
        }

        public void OpenLabe(GameObject obj)
        {
            m_BuildObj = m_DataModel.GetBuildData(obj.GetInstanceID());
            if (m_BuildObj == null)
                return;
            gameObject.SetActive(true);
            ShowData();
        }

        public void CloseLabe()
        {
            gameObject.SetActive(false);
            m_BuildObj = null;
        }

        protected virtual void OnDestroy()
        {
            DeletButton.onClick.RemoveAllListeners();
        }
    }
}