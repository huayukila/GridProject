using TMPro;
using UnityEngine;

namespace Framework.BuildProject
{
    public class Descripter : BuildController
    {
        public TextMeshProUGUI dataTxt;
        public float Offset_Y;
        private IBuilDataModel _dataModel;

        private void Start()
        {
            _dataModel = this.GetModel<IBuilDataModel>();
            gameObject.SetActive(false);
        }

        public void ShowData_House(Vector3 pos_, string buildingName_)
        {
            transform.position = pos_+Vector3.up*Offset_Y;
            BuildingData data=_dataModel.GetBuildingConfig(buildingName_);
            dataTxt.text = $"��{�f�[�^\n���O:{data.name}\n�ϋv:{data.LevelDatasList[0].MaxHp}\n�J����:{data.LevelDatasList[0].MaxWorker}\n�R�[�X�g";
            foreach (var cost in data.LevelDatasList[0].CostList)
            {
                dataTxt.text += $"\n{cost.resType.ToString()}:{cost.Cost}";
            }
            gameObject.SetActive(true);
        }

        public void ShowData_Tower(Vector3 pos_, string buildingName_)
        {
            transform.position = pos_+Vector3.up*Offset_Y;
            BuildingData data=_dataModel.GetBuildingConfig(buildingName_);
            dataTxt.text = $"��{�f�[�^\n���O:{data.name}\n�ϋv:{data.LevelDatasList[0].MaxHp}\n�J����:0/{data.LevelDatasList[0].MaxWorker}\n�R�[�X�g";
            foreach (var cost in data.LevelDatasList[0].CostList)
            {
                dataTxt.text += $"\n{cost.resType.ToString()}:{cost.Cost}";
            }
            gameObject.SetActive(true);
        }

        public void ShowData_Factory(Vector3 pos_, string buildingName_)
        {
            transform.position = pos_+Vector3.up*Offset_Y;
            BuildingData data=_dataModel.GetBuildingConfig(buildingName_);
            dataTxt.text = $"��{�f�[�^\n���O:{data.name}\n�ϋv:{data.LevelDatasList[0].MaxHp}\n�J����:0/{data.LevelDatasList[0].MaxWorker}\n�R�[�X�g";
            foreach (var cost in data.LevelDatasList[0].CostList)
            {
                dataTxt.text += $"\n{cost.resType.ToString()}:{cost.Cost}";
            }
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            dataTxt.text = "";
            gameObject.SetActive(false);
        }
    }
}