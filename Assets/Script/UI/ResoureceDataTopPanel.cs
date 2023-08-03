using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class ResoureceDataTopPanel : BuildController
    {
        public Text resoreceDataTxt;
        IResourceDataModel dataModel;
        // Start is called before the first frame update
        void Start()
        {
            dataModel = this.GetModel<IResourceDataModel>();
            resoreceDataTxt.text = "Mood:" + dataModel.GetRes(ResourceType.Mood) + " Stone:" + dataModel.GetRes(ResourceType.Stone)
                + " Gold:" + dataModel.GetRes(ResourceType.Gold) + " Worker:" + dataModel.GetRes(ResourceType.Worker);
            this.RegisterEvent<RefreshResPanel>(e =>
            {
                resoreceDataTxt.text = "Mood:" + dataModel.GetRes(ResourceType.Mood) + " Stone:" + dataModel.GetRes(ResourceType.Stone)
                + " Gold:" + dataModel.GetRes(ResourceType.Gold) + " Worker:" + dataModel.GetRes(ResourceType.Worker);
            }).UnregisterWhenGameObjectDestroyde(gameObject);
        }
    }
}