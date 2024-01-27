using TMPro;

namespace Framework.BuildProject
{
    public class ResoureceDataTopPanel : BuildController
    {
        public TextMeshProUGUI WoodTxt;
        public TextMeshProUGUI StoneTxt;
        public TextMeshProUGUI GoldTxt;
        public TextMeshProUGUI IdleWorkerTxt;
        public TextMeshProUGUI MaxWorkerTxt;
        private IResourceDataModel dataModel;

        // Start is called before the first frame update
        private void Start()
        {
            dataModel = this.GetModel<IResourceDataModel>();
            RefreshResPanel();
            this.RegisterEvent<RefreshResPanel>(e => RefreshResPanel()
            ).UnregisterWhenGameObjectDestroyed(gameObject);
        }

        private void RefreshResPanel()
        {
            WoodTxt.text = dataModel.GetRes(ResourceType.Wood).ToString();
            StoneTxt.text = dataModel.GetRes(ResourceType.Stone).ToString();
            GoldTxt.text = dataModel.GetRes(ResourceType.Gold).ToString();
            IdleWorkerTxt.text = dataModel.GetRes(ResourceType.Worker).ToString();
            MaxWorkerTxt.text = dataModel.MaxWorkerNum.ToString();
        }
    }
}