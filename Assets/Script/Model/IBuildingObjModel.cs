using System.Collections.Generic;
using System.Linq;

namespace Framework.BuildProject
{
    public interface IBuildingObjModel : IModel
    {
        void RegisterBuild(int id_, BuildingBase buildingBase_);
        void UnregisterBuild(int id_);
        BuildingBase GetBuildData(int id_);
        List<BuildingBase> GetBuildDataList(BuildingType type_);
        BuildingBase[] GetBuildingObjsDataForArchiveSystem();
        void Deinit();
    }

    public class BuildingObjModel : AbstractModel, IBuildingObjModel
    {
        private Dictionary<int, BuildingBase> m_BuildingDic;

        // �����̓o�^
        public void RegisterBuild(int id_, BuildingBase buildingBase_)
        {
            if (!m_BuildingDic.ContainsKey(id_)) m_BuildingDic.Add(id_, buildingBase_);
        }

        // �����̓o�^����
        public void UnregisterBuild(int id_)
        {
            if (m_BuildingDic.TryGetValue(id_, out var obj))
            {
                obj.Reset();
                m_BuildingDic.Remove(id_);
            }
        }

        // �����f�[�^�̎擾
        public BuildingBase GetBuildData(int id_)
        {
            m_BuildingDic.TryGetValue(id_, out var data);
            return data;
        }

        // ����^�C�v�̌����f�[�^���X�g���擾
        public List<BuildingBase> GetBuildDataList(BuildingType type_)
        {
            return m_BuildingDic.Values.Where(buildingObj => buildingObj.BuildingType == type_).ToList();
        }

        // �A�[�J�C�u�V�X�e���p�̌����f�[�^���擾
        public BuildingBase[] GetBuildingObjsDataForArchiveSystem()
        {
            return m_BuildingDic.Values.ToArray();
        }

        // �I������
        public void Deinit()
        {
            m_BuildingDic.Clear();
        }

        // ����������
        protected override void OnInit()
        {
            m_BuildingDic = new Dictionary<int, BuildingBase>();
        }
    }
}