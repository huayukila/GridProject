using System.Collections.Generic;

namespace Framework.BuildProject
{
    public interface IBuildingObjModel : IModel
    {
        void RegisterBuild(int ID, BuildingObj buildingObj);
        void UnregisterBuild(int ID);
        BuildingObj GetBuildData(int ID);
        void UpdateBuildings();
        List<BuildingObj> GetBuildDataList(BuildingType type);
    }

    public class BuildingObjModel : AbstractModel, IBuildingObjModel
    {
        Dictionary<int, BuildingObj> m_BuildingDic;

        protected override void OnInit()
        {
            m_BuildingDic = new Dictionary<int, BuildingObj>();
        }


        public void UnregisterBuild(int ID)
        {
            if (m_BuildingDic.TryGetValue(ID, out BuildingObj obj))
            {
                obj.Reset();
                m_BuildingDic.Remove(ID);
            }
        }

        public BuildingObj GetBuildData(int ID)
        {
            m_BuildingDic.TryGetValue(ID, out BuildingObj data);
            return data;
        }

        public void UpdateBuildings()
        {
            if (m_BuildingDic.Count <= 0) return;
            foreach (IBuildingObj buildingObj in m_BuildingDic.Values)
            {
                buildingObj.Update();
            }
        }

        public List<BuildingObj> GetBuildDataList(BuildingType type)
        {
            List<BuildingObj> tempList = new List<BuildingObj>();
            foreach (BuildingObj buildingObj in m_BuildingDic.Values)
            {
                if (buildingObj.m_BuildingType == type)
                {
                    tempList.Add(buildingObj);
                }
            }

            return tempList;
        }

        public void RegisterBuild(int ID, BuildingObj buildingObj)
        {
            if (!m_BuildingDic.ContainsKey(ID))
            {
                m_BuildingDic.Add(ID, buildingObj);
            }
        }
    }
}