using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class TitlePanel : BuildController
    {
        public Button GameStartBtn;

        // Start is called before the first frame update
        void Start()
        {
            GameStartBtn.onClick.AddListener(this.SendEvent<GameStartEvent>);
        }
    }
}