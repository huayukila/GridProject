using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.BuildProject
{
    public class GameManager : BuildController
    {
        private IBulletSystem m_BulletSystem;
        private IGridBuildSystem m_GridBuildSystem;
        private int StageLevel;

        private void Awake()
        {
            StageLevel = 0;
            DontDestroyOnLoad(this);
            RegisterEvents();
        }

        // システムの初期化
        private void Start()
        {
            InitializeSystems();
            LoadInitialScene();
        }


        private void Update()
        {
            //いつでもescキーを押すとゲーム退出できるように
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        // 破棄時の処理
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        // 必要なシステムを取得して初期化する
        private void InitializeSystems()
        {
            m_GridBuildSystem = this.GetSystem<IGridBuildSystem>();
            m_BulletSystem = this.GetSystem<IBulletSystem>();
        }

        // イベント登録
        private void RegisterEvents()
        {
            // イベントリスナーを登録し、必要に応じて削除する
            this.RegisterEvent<GameStartEvent>(e => LoadScene(GameScene.Gaming))
                .UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<BackToTitleEvent>(e =>
                {
                    Time.timeScale = 1.0f;
                    LoadScene(GameScene.Title);
                })
                .UnregisterWhenGameObjectDestroyed(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        // シーンを読み込む
        private void LoadScene(GameScene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        // 最初のシーンを読み込む
        private void LoadInitialScene()
        {
            LoadScene(GameScene.Title);
        }

        // シーンが読み込まれたときの処理
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameScene.Gaming.ToString()) InitializeGamingScene();
        }

        // 「Gaming」シーンの初期化
        private void InitializeGamingScene()
        {
            m_GridBuildSystem.CreateGrid(Global.GRID_SIZE_WIDTH, Global.GRID_SIZE_HEIGHT, Global.GRID_SIZE_CELL);
            m_GridBuildSystem.CreateBuilding("Core", new Vector2Int(4, 4), Dir.Down);
            m_BulletSystem.InitByManager();
            this.GetSystem<IEnemySystem>().InitByManager();
            this.GetModel<IPlayerDataModel>().playerState = PlayerState.Normal;
        }

        // シーンがアンロードされたときの処理
        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == GameScene.Gaming.ToString()) DeinitializeGamingScene();
        }

        // 「Gaming」シーンの終了処理
        private void DeinitializeGamingScene()
        {
            m_BulletSystem.Deinit();
            this.GetSystem<IEnemySystem>().Deinit();
            this.GetSystem<IGridBuildSystem>().Deinit();
            this.GetModel<IBuildingObjModel>().Deinit();
            this.GetModel<IResourceDataModel>().Deinit();
        }

        // ゲームを再スタートする
        private void Restart()
        {
            StageLevel = 0;
        }
    }

    // ゲームシーンの列挙型
    public enum GameScene
    {
        Title,
        Gaming
    }
}