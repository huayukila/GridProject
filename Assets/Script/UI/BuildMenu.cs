using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildMenu : BuildController
    {
        public List<Button> buttonList;
        public Descripter Descripter;

        private string[] m_buildingName =
        {
            "House",
            "Sawmill",
            "StoneMine",
            "GoldenMine",
            "BallistaTower",
            "MagicTower",
            "CannonTower",
        };

        // Start is called before the first frame update
        private void Start()
        {
            for (int i = 0; i < m_buildingName.Length;)
            {
                string buildingName = m_buildingName[i];
                buttonList[i].onClick.AddListener(() => SelectBuilding(buildingName));
                ++i;
            }
            
            AddEventTrigger(buttonList[0].GetComponent<EventTrigger>(), ShowDetailPanel_House,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[0].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
            
            AddEventTrigger(buttonList[1].GetComponent<EventTrigger>(), ShowDetailPanel_Tower1,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[1].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
            
            AddEventTrigger(buttonList[2].GetComponent<EventTrigger>(), ShowDetailPanel_Tower2,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[2].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
            
            AddEventTrigger(buttonList[3].GetComponent<EventTrigger>(), ShowDetailPanel_Tower3,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[3].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
            
            AddEventTrigger(buttonList[4].GetComponent<EventTrigger>(), ShowDetailPanel_Factory1,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[4].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
            
            AddEventTrigger(buttonList[5].GetComponent<EventTrigger>(), ShowDetailPanel_Factory2,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[5].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
            
            AddEventTrigger(buttonList[6].GetComponent<EventTrigger>(), ShowDetailPanel_Factory3,
                EventTriggerType.PointerEnter);
            AddEventTrigger(buttonList[6].GetComponent<EventTrigger>(), CloseDetailPanel,
                EventTriggerType.PointerExit);
        }
        
        private void OnDestroy()
        {
            foreach (var button in buttonList) button.onClick.RemoveAllListeners();

            buttonList.Clear();
        }

        private void SelectBuilding(string name_)
        {
            if (this.GetModel<IPlayerDataModel>().playerState == PlayerState.Build)
                return;
            var buildingData =
                this.GetModel<IBuilDataModel>().GetBuildingConfig(name_);

            if (this.GetModel<IResourceDataModel>().IsResEnough(buildingData.LevelDatasList[0].CostList))
                this.GetSystem<IGridBuildSystem>().SelectBuilding(buildingData);
        }

        void ShowDetailPanel_House(BaseEventData data)
        {
            Descripter.ShowData_House(buttonList[0].transform.position, m_buildingName[0]);
        }
        
        void ShowDetailPanel_Tower1(BaseEventData data)
        {
            Descripter.ShowData_Tower(buttonList[1].transform.position, m_buildingName[1]);
        }
        void ShowDetailPanel_Tower2(BaseEventData data)
        {
            Descripter.ShowData_Tower(buttonList[2].transform.position, m_buildingName[2]);
        }
        void ShowDetailPanel_Tower3(BaseEventData data)
        {
            Descripter.ShowData_Tower(buttonList[3].transform.position, m_buildingName[3]);
        }
        
        void ShowDetailPanel_Factory1(BaseEventData data)
        {
            Descripter.ShowData_Factory(buttonList[4].transform.position, m_buildingName[4]);
        }
        void ShowDetailPanel_Factory2(BaseEventData data)
        {
            Descripter.ShowData_Factory(buttonList[5].transform.position, m_buildingName[5]);
        }
        void ShowDetailPanel_Factory3(BaseEventData data)
        {
            Descripter.ShowData_Factory(buttonList[6].transform.position, m_buildingName[6]);
        }

        void CloseDetailPanel(BaseEventData data)
        {
            Descripter.ClosePanel();
        }
        
        void AddEventTrigger(EventTrigger buttonEventTrigger, UnityEngine.Events.UnityAction<BaseEventData> action,
            EventTriggerType triggerType)
        {
            EventTrigger.Entry trigger = new EventTrigger.Entry();
            trigger.eventID = triggerType;
            trigger.callback.AddListener(action);
            buttonEventTrigger.triggers.Add(trigger);
        }
    }
}