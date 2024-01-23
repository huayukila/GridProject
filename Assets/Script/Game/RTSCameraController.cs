using UnityEngine;

namespace Framework.BuildProject
{
    public enum CameraState
    {
        Idle,
        OnGameSceneEnter,
        OnGaming,
        OnEnd
    }

    public class RTSCameraController : BuildController
    {
        public float movementAmount;
        public float lerpT;
        public float rotationAmount;
        public Vector3 zoomAmount;
        public float minZoom;
        public float maxZoom;
        private readonly Vector3 CenterPos = new(50, 10, 50);

        //スクリーン処理用
        private BrightnessSaturationAndContrast m_Brightness;
        private Transform m_CameraTrans;
        private Vector3 m_DragCurrentPostion;
        private Plane m_DragPlane;
        private Vector3 m_DragStartPosition;
        private readonly float m_DurationTime = 6f;
        private GaussianBlur m_GBlur;

        private Vector3 m_NewPosition;
        private Quaternion m_NewRotation;
        private Vector3 m_NewZoom;
        private Vector3 m_RotateCurrentPosition;
        private Vector3 m_RotateStartPosition;
        private float m_StartTime;
        private CameraState m_State = CameraState.Idle;
        private float m_TargetAngle;
        private float m_TargetBrightness;
        private Vector3 m_TargetPos;

        private void Start()
        {
            m_CameraTrans = Camera.main.transform;
            m_Brightness = m_CameraTrans.GetComponent<BrightnessSaturationAndContrast>();
            m_GBlur = m_CameraTrans.GetComponent<GaussianBlur>();
            var transform1 = transform;
            m_NewPosition = transform1.position;
            m_NewRotation = transform1.rotation;
            m_NewZoom = m_CameraTrans.localPosition;
            m_DragPlane = new Plane(Vector3.up, Vector3.zero);

            m_TargetAngle = 0;
            m_TargetBrightness = 0;
            m_TargetPos = m_CameraTrans.localPosition;
            this.SendEvent(new CameraEvent { State = m_State });
        }


        private void Update()
        {
            switch (m_State)
            {
                case CameraState.Idle:
                    break;
                case CameraState.OnGameSceneEnter:
                    HandleGameSceneEnter();
                    break;
                case CameraState.OnGaming:
                    HandleMouseInput();
                    HandleMovemtInput();
                    transform.position = Vector3.Lerp(transform.position, m_NewPosition, Time.deltaTime * lerpT);
                    transform.rotation = Quaternion.Lerp(transform.rotation, m_NewRotation, Time.deltaTime * lerpT);
                    m_CameraTrans.localPosition =
                        Vector3.Lerp(m_CameraTrans.localPosition, m_NewZoom, Time.deltaTime * lerpT);
                    break;
                case CameraState.OnEnd:
                    transform.position = Vector3.Lerp(transform.position, CenterPos,
                        Time.unscaledDeltaTime * lerpT);
                    transform.rotation = transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.Euler(0, 0, 0), Time.unscaledDeltaTime * lerpT);
                    m_CameraTrans.localPosition = Vector3.Lerp(m_CameraTrans.localPosition, m_CameraTrans.forward * -45,
                        Time.unscaledDeltaTime * lerpT);
                    if (Vector3.Distance(CenterPos, transform.position) > 0.1f)
                        break;
                    if (Time.unscaledTime - m_StartTime < m_DurationTime)
                        break;
                    m_GBlur.enabled = true;
                    m_GBlur.blurSpread += Time.unscaledDeltaTime;
                    if (m_GBlur.blurSpread >= 3f)
                    {
                        this.SendEvent(new CameraEvent { State = m_State });
                        m_State = CameraState.Idle;
                    }

                    break;
            }
        }

        public void ChangeToEnd()
        {
            m_StartTime = Time.time;
            m_State = CameraState.OnEnd;
        }

        #region 内部用

        private void HandleGameSceneEnter()
        {
            if (m_Brightness.brightness - m_TargetBrightness < 0.01f)
            {
                m_Brightness.brightness += Time.deltaTime * 0.3f;
                return;
            }

            m_CameraTrans.localPosition =
                Vector3.Lerp(m_CameraTrans.localPosition, m_TargetPos, Time.deltaTime * 0.8f);
            m_CameraTrans.localRotation = Quaternion.Lerp(m_CameraTrans.localRotation,
                Quaternion.Euler(m_TargetAngle, 0, 0), 0.8f * Time.deltaTime);
            if (Vector3.Distance(m_CameraTrans.localPosition, m_TargetPos) < 0.1f)
            {
                m_CameraTrans.localPosition = m_TargetPos;
                m_State = CameraState.OnGaming;
                m_NewZoom = m_TargetPos;
                this.SendEvent(new CameraEvent { State = m_State });
            }
        }

        private void HandleMovemtInput()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                m_NewPosition += transform.forward * movementAmount;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                m_NewPosition -= transform.forward * movementAmount;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                m_NewPosition += transform.right * movementAmount;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                m_NewPosition -= transform.right * movementAmount;

            if (Input.GetKey(KeyCode.Q)) m_NewRotation *= Quaternion.Euler(Vector3.up * rotationAmount);

            if (Input.GetKey(KeyCode.E)) m_NewRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        private void HandleMouseInput()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                var zoomDelta = Input.mouseScrollDelta.y * zoomAmount;
                var potentialZoom = m_NewZoom + zoomDelta;
                if (potentialZoom.magnitude >= minZoom && potentialZoom.magnitude <= maxZoom) m_NewZoom = potentialZoom;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = m_CameraTrans.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (m_DragPlane.Raycast(ray, out var entry)) m_DragStartPosition = ray.GetPoint(entry);
            }

            if (Input.GetMouseButton(0))
            {
                var ray = m_CameraTrans.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (m_DragPlane.Raycast(ray, out var entry))
                {
                    m_DragCurrentPostion = ray.GetPoint(entry);

                    m_NewPosition = transform.position + m_DragStartPosition - m_DragCurrentPostion;
                }
            }

            if (Input.GetMouseButtonDown(2)) m_RotateStartPosition = Input.mousePosition;

            if (Input.GetMouseButton(2))
            {
                m_RotateCurrentPosition = Input.mousePosition;

                var difference = m_RotateStartPosition - m_RotateCurrentPosition;

                m_RotateStartPosition = m_RotateCurrentPosition;

                m_NewRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }

        public void GameSceneEnter(float targetBrightness_, Vector3 targetPos_, float targetAngle_)
        {
            m_State = CameraState.OnGameSceneEnter;
            m_TargetBrightness = targetBrightness_;
            m_TargetPos = targetPos_;
            m_TargetAngle = targetAngle_;
        }

        #endregion
    }
}