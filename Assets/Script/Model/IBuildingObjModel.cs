using System.Collections.Generic;
using System.Linq;

namespace Framework.BuildProject
{
    public interface IBuildingObjModel : IModel
    {
        void RegisterBuild(int ID, BuildingBase buildingBase);
        void UnregisterBuild(int ID);
        BuildingBase GetBuildData(int ID);
        void UpdateBuildings();
        List<BuildingBase> GetBuildDataList(BuildingType type);

        BuildingBase[] GetBuildingObjsDataForArchiveSystem();
    }

    public class BuildingObjModel : AbstractModel, IBuildingObjModel
    {
        Dictionary<int, BuildingBase> m_BuildingDic;

        protected override void OnInit()
        {
            m_BuildingDic = new Dictionary<int, BuildingBase>();
        }


        public void UnregisterBuild(int ID)
        {
            if (m_BuildingDic.TryGetValue(ID, out BuildingBase obj))
            {
                obj.Reset();
                m_BuildingDic.Remove(ID);
            }
        }

        public BuildingBase GetBuildData(int ID)
        {
            m_BuildingDic.TryGetValue(ID, out BuildingBase data);
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

        public List<BuildingBase> GetBuildDataList(BuildingType type)
        {
            List<BuildingBase> tempList = new List<BuildingBase>();
            foreach (BuildingBase buildingObj in m_BuildingDic.Values)
            {
                if (buildingObj.BuildingType == type)
                {
                    tempList.Add(buildingObj);
                }
            }

            return tempList;
        }

        public BuildingBase[] GetBuildingObjsDataForArchiveSystem()
        {
            return m_BuildingDic.Values.ToArray();
        }

        public void RegisterBuild(int ID, BuildingBase buildingBase)
        {
            if (!m_BuildingDic.ContainsKey(ID))
            {
                m_BuildingDic.Add(ID, buildingBase);
            }
        }
    }
}