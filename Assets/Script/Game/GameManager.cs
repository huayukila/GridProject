using Kit;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.BuildProject
{
    public class GameManager : BuildController
    {
        private int StageLevel;
        private IGridBuildSystem m_GridBuildSystem;
        private IBulletSystem m_BulletSystem;

        private void Awake()
        {
            StageLevel = 0;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            m_GridBuildSystem = this.GetSystem<IGridBuildSystem>();
            m_BulletSystem = this.GetSystem<IBulletSystem>();

            this.RegisterEvent<ReStartEvent>(e => { Restart(); }).UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<GameStartEvent>(e => { SceneManager.LoadSceneAsync("Gaming"); })
                .UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<StopOrContinueGameEvent>(e =>
            {
                if (!e.Switch)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1.0f;
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);


            this.RegisterEvent<GameClearEvent>(e => { SceneManager.LoadScene("GameClear"); })
                .UnregisterWhenGameObjectDestroyed(gameObject);


            this.RegisterEvent<BackToTitleEvent>(e => { SceneManager.LoadScene("Title"); })
                .UnregisterWhenGameObjectDestroyed(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Title");
        }


        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Gaming")
            {
                GameObject canvas = GameObject.Find("Canvas");
                
                m_GridBuildSystem.CreatGrid(Global.GRID_SIZE_WIDTH, Global.GRID_SIZE_HEIGHT, Global.GRID_SIZE_CELL);
                m_GridBuildSystem.CreatBuilding(BuildingType.Core, new Vector2Int(4, 4), Dir.Down);
                m_BulletSystem.InitByManager();
                this.GetSystem<IEnemySystem>().InitByManager();
                this.GetModel<IPlayerDataModel>().playerState = PlayerState.Normal;
            }
        }

        void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == "Gaming")
            {
                m_BulletSystem.Deinit();
                this.GetSystem<IEnemySystem>().Deinit();
                this.GetSystem<IGridBuildSystem>().Deinit();
                this.GetModel<IBuildingObjModel>().Deinit();
                this.GetModel<IResourceDataModel>().Deinit();
            }
        }

        void Restart()
        {
            StageLevel = 0;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}