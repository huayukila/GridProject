namespace Framework.BuildProject
{
    interface IPlayerDataModel : IModel
    {
        PlayerState playerState { get; set; }
        void InitByManager();
    }

    public class PlayerDataModel : AbstractModel, IPlayerDataModel
    {
        public PlayerState playerState { get; set; }

        public void InitByManager()
        {
            playerState = PlayerState.Normal;
        }

        protected override void OnInit()
        {
            playerState = PlayerState.Normal;
        }
    }
}