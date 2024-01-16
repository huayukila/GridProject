namespace Framework.BuildProject
{
    public class HouseDataPanel : BuildDataPanelBase
    {
        protected override void ShowData()
        {
            DataText.text =
                $"Name:{m_BuildBase.BuildingName}\nLevel:{m_BuildBase.BuildingLevel}\nHP:{m_BuildBase.BuildingHp}\nWorker:{m_BuildBase.m_MaxWorkerNum}";
        }
    }
}