using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class GameClearPanel : BuildController
    {
        public Button BackToTitleBtn;
        private void Start()
        {
            BackToTitleBtn.onClick.AddListener(() => { this.SendEvent<BackToTitleEvent>(); });
        }

        private void OnDestroy()
        {
            BackToTitleBtn.onClick.RemoveAllListeners();
        }
    }
}