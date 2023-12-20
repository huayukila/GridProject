using UnityEngine;

namespace Framework.BuildProject
{
    public class GameManager : BuildController
    {
        private int StageLevel;
        public Transform m_RootNode;
        private void Awake()
        {
            StageLevel = 0;
            DontDestroyOnLoad(this);
            this.RegisterEvent<ReStartEvent>(e =>
            {
                Restart();
            });
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void Restart()
        {
            StageLevel = 0;
        }
    }
}