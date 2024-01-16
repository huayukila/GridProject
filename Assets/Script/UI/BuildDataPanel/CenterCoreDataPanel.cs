namespace Framework.BuildProject
{
    public class CenterCoreDataPanel : BuildDataPanelBase
    {
        protected override void ShowData()
        {
            DataText.text =
                $"Name:{m_BuildBase.BuildingName}\nLevel:{m_BuildBase.BuildingLevel}\nHP:{m_BuildBase.BuildingHp}";
        }
    }
}