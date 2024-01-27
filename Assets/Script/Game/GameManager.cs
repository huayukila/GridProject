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

        // �V�X�e���̏�����
        private void Start()
        {
            InitializeSystems();
            LoadInitialScene();
        }


        private void Update()
        {
            //���ł�esc�L�[�������ƃQ�[���ޏo�ł���悤��
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        // �j�����̏���
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        // �K�v�ȃV�X�e�����擾���ď���������
        private void InitializeSystems()
        {
            m_GridBuildSystem = this.GetSystem<IGridBuildSystem>();
            m_BulletSystem = this.GetSystem<IBulletSystem>();
        }

        // �C�x���g�o�^
        private void RegisterEvents()
        {
            // �C�x���g���X�i�[��o�^���A�K�v�ɉ����č폜����
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

        // �V�[����ǂݍ���
        private void LoadScene(GameScene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        // �ŏ��̃V�[����ǂݍ���
        private void LoadInitialScene()
        {
            LoadScene(GameScene.Title);
        }

        // �V�[�����ǂݍ��܂ꂽ�Ƃ��̏���
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameScene.Gaming.ToString()) InitializeGamingScene();
        }

        // �uGaming�v�V�[���̏�����
        private void InitializeGamingScene()
        {
            m_GridBuildSystem.CreateGrid(Global.GRID_SIZE_WIDTH, Global.GRID_SIZE_HEIGHT, Global.GRID_SIZE_CELL);
            m_GridBuildSystem.CreateBuilding("Core", new Vector2Int(4, 4), Dir.Down);
            m_BulletSystem.InitByManager();
            this.GetSystem<IEnemySystem>().InitByManager();
            this.GetModel<IPlayerDataModel>().playerState = PlayerState.Normal;
        }

        // �V�[�����A�����[�h���ꂽ�Ƃ��̏���
        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == GameScene.Gaming.ToString()) DeinitializeGamingScene();
        }

        // �uGaming�v�V�[���̏I������
        private void DeinitializeGamingScene()
        {
            m_BulletSystem.Deinit();
            this.GetSystem<IEnemySystem>().Deinit();
            this.GetSystem<IGridBuildSystem>().Deinit();
            this.GetModel<IBuildingObjModel>().Deinit();
            this.GetModel<IResourceDataModel>().Deinit();
        }

        // �Q�[�����ăX�^�[�g����
        private void Restart()
        {
            StageLevel = 0;
        }
    }

    // �Q�[���V�[���̗񋓌^
    public enum GameScene
    {
        Title,
        Gaming
    }
}