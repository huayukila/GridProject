using Kit;
using UnityEngine;

namespace Framework.BuildProject
{
    public class GamingDirector : BuildController
    {
        public RTSCameraController cameraCtrl;
        public GameObject canvas;
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
                        canvas.SetActive(true);
                        break;
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);
        }

        private void Start()
        {
            canvas.SetActive(false);
        }
    }
}