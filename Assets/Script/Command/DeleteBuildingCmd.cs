using UnityEngine;

namespace Framework.BuildProject
{
    public class DeleteBuildingCmd : AbstractCommand
    {
        
        GameObject _Obj;
        protected override void OnExecute()
        {
            this.GetSystem<IGridBuildSystem>().DestroyBuilding(_Obj);
        }

        public DeleteBuildingCmd(GameObject obj)
        {
            _Obj = obj;
        }
    }
}