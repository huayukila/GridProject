namespace Framework.BuildProject
{
    public interface IGameDataModel : IModel
    {
        
    }

    public class GameDataModel : AbstractModel, IModel
    {
        private int m_StageLevel;
        private bool m_IsClearAllStage;
        protected override void OnInit()
        {
        }
    }
}