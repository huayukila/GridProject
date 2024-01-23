namespace Framework.BuildProject
{
    public class CenterCoreDataPanel : BuildDataPanelBase
    {
        protected override void ShowData()
        {
            NameText.text = m_BuildBase.BuildingName;
            HpText.text = $"{m_BuildBase.BuildingHp}/{m_BuildBase.BuildingMaxHp}";
        }
    }
}