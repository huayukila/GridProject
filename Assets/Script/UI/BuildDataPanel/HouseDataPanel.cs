namespace Framework.BuildProject
{
    public class HouseDataPanel : BuildDataBasePanel
    {
        protected override void ShowData()
        {
            House house=MBuildBase as House;
            DataText.text =
                $"Name:{house.BuildingName}\nLevel:{house.BuildingLevel}\nHP:{house.BuildingHp}\nWorker:{house.m_MaxWorkerNum}";
        }
    }
}