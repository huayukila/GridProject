using UnityEngine;

namespace Framework.BuildProject
{
    public class GameManager : BuildController
    {
        public Transform m_RootNode;
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}