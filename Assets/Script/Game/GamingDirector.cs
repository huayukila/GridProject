using UnityEngine;

namespace Framework.BuildProject
{
    public class GamingDirector : BuildController
    {
        public RTSCameraController cameraCtrl;
        public GameObject mainCanvas;
        public GameObject subCanvas;
        public Vector3 targetPos;
        public float targetAngle;
        public float targetBrightness;

        private void Awake()
        {
            this.RegisterEvent<CameraEvent>(e =>
            {
                switch (e.State)
                {
                    case CameraState.Idle:
                        cameraCtrl.GameSceneEnter(targetBrightness, targetPos, targetAngle);
                        break;
                    case CameraState.OnGaming:
                        mainCanvas.SetActive(true);
                        break;
                    case CameraState.OnEnd:
                        subCanvas.SetActive(true);
                        break;
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<GameOverEvent>(e => OnGameEnd())
                .UnregisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<GameClearEvent>(e => OnGameEnd())
                .UnregisterWhenGameObjectDestroyed(gameObject);
        }

        private void Start()
        {
            mainCanvas.SetActive(false);
            subCanvas.SetActive(false);
        }

        private void OnGameEnd()
        {
            this.GetModel<IPlayerDataModel>().playerState = PlayerState.Win;
            mainCanvas.SetActive(false);
            cameraCtrl.ChangeToEnd();
            Time.timeScale = 0;
        }
    }
}