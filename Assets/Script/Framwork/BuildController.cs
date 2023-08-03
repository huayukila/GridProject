using UnityEngine;

namespace Framework.BuildProject
{
    public class BuildController : MonoBehaviour,IController
    { 
        public IArchitecture GetArchitecture()
        {
            return BuildProject.Interface;
        }
    }
}