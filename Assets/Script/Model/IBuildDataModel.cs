using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public interface IBuilDataModel : IModel
    {
        BuildingData GetBuildingConfig(string name_);
    }

    public class BuildDataModel : AbstractModel, IBuilDataModel
    {
        private Dictionary<string, BuildingData> m_BuildingConfigDic;

        // ���z�̐ݒ���擾
        public BuildingData GetBuildingConfig(string name_)
        {
            if (m_BuildingConfigDic.TryGetValue(name_, out var buildingData))
            {
                return buildingData;
            }

            Debug.LogWarning($"�T���Ă��錚���̃^�C�v ({name_}) �͑��݂��Ă��Ȃ�");
            return null;
        }

        // ���f���̏�����
        protected override void OnInit()
        {
            LoadBuildingData();
        }

        // ���z�f�[�^�̓ǂݍ���
        private void LoadBuildingData()
        {
            m_BuildingConfigDic = new Dictionary<string, BuildingData>();
            var database = Resources.Load<BuildingDatabase>("BuildingDatabase");

            foreach (var data in database.BuildingDatas) m_BuildingConfigDic.Add(data.NameString, data);

            Resources.UnloadAsset(database);
        }
    }
}