using UnityEngine;

namespace Framework.BuildProject
{
    public class RemoveWorker : AbstractCommand
    {
        private int _id;

        protected override void OnExecute()
        {
            this.GetSystem<IBuildingSystem>().RemoveWorker(_id);
            this.SendEvent<RefreshResPanel>();
        }

        public RemoveWorker(int id)
        {
            _id = id;
        }
    }
}