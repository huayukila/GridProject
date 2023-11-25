using UnityEngine;

namespace Framework.BuildProject
{
    public class AddWorker : AbstractCommand
    {
        private int _id;

        protected override void OnExecute()
        {
            this.GetSystem<IBuildingSystem>().AddWorker(_id);
            this.SendEvent<RefreshResPanel>();
        }

        public AddWorker(int id)
        {
            _id = id;
        }
    }
}