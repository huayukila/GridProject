using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildDataPanelBase : BuildController
    {
        public Button DeletButton;
        public Text DataText;
        protected BuildingBase m_BuildBase;

        protected virtual void Start()
        {
            DeletButton.onClick.AddListener(DeleteBuilding);
        }

        void DeleteBuilding()
        {
            this.GetSystem<IGridBuildSystem>().DestroyBuilding(m_BuildBase.gameObject);
            m_BuildBase = null;
            gameObject.SetActive(false);
        }

        protected virtual void ShowData()
        {
        }

        public void OpenLabe(GameObject obj)
        {
            if (!obj.TryGetComponent(out m_BuildBase))
                return;
            gameObject.SetActive(true);
            ShowData();
        }

        public void CloseLabe()
        {
            gameObject.SetActive(false);
            m_BuildBase = null;
        }

        protected virtual void OnDestroy()
        {
            DeletButton.onClick.RemoveAllListeners();
        }
    }
}