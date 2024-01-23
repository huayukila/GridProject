namespace Framework.BuildProject
{
    public interface IGameDataModel : IModel
    {
    }

    public class GameDataModel : AbstractModel, IModel
    {
        private bool m_IsClearAllStage;
        private int m_StageLevel;

        protected override void OnInit()
        {
        }
    }
}