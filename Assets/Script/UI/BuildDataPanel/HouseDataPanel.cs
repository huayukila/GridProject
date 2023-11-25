namespace Framework.BuildProject
{
    public class HouseDataPanel : BuildDataBasePanel
    {
        protected override void ShowData()
        {
            DataText.text =
                $"Name:{m_BuildObj.m_BuildingName}\nHP:{m_BuildObj.m_BuildingHp}\nWorker:{m_BuildObj.m_MaxWorkerNum}";
        }
    }
}