using UnityEngine;

namespace Framework.BuildProject
{
    public class TitleController : BuildController
    {
        public float intensityMultiplier = 1.0f;
        public MeshRenderer CoreRenderer;
        public float speed;
        public GameObject canvasObj;
        public GameObject clickObj;
        private Color baseEmissionColor;
        private BrightnessSaturationAndContrast m_Brightness;


        private bool m_IsRay;
        private Camera m_MainCamera;
        private Material m_Material;
        private State m_State;
        private float m_Time;

        // �X�^�[�g���̏�����
        private void Start()
        {
            m_IsRay = false;
            m_Material = CoreRenderer.sharedMaterial;
            m_MainCamera = Camera.main;
            m_Brightness = m_MainCamera.GetComponent<BrightnessSaturationAndContrast>();
            m_State = State.OnGameOpen;
        }

        // ���t���[���̍X�V
        private void Update()
        {
            switch (m_State)
            {
                case State.OnGameOpen:
                    m_Brightness.brightness = Mathf.Lerp(m_Brightness.brightness, 0.91f, 2f * Time.deltaTime);
                    if (0.91f - m_Brightness.brightness < 0.01f)
                    {
                        canvasObj.SetActive(true);
                        m_State = State.Normal;
                    }

                    break;
                case State.Normal:
                    HandleRayCast();
                    clickObj.SetActive(m_IsRay);
                    if (m_IsRay) clickObj.transform.position = Input.mousePosition + Vector3.left * 45f;

                    if (Input.GetMouseButtonDown(0)) m_State = State.OnEnterGaming;

                    break;
                case State.OnEnterGaming:
                    HandleGameStart();
                    break;
            }

            UpdateMaterial();
        }

        // �Q�[���X�^�[�g���̏���
        private void HandleGameStart()
        {
            canvasObj.SetActive(false);
            m_MainCamera.transform.Translate(Vector3.forward * (8f * Time.deltaTime), Space.World);
            m_Brightness.brightness -= Time.deltaTime * 0.3f;
            if (m_Brightness.brightness <= 0f)
            {
                m_Material.SetColor("_EmissionColor", 0.8f * Color.white);
                this.SendEvent<GameStartEvent>();
            }
        }

        // ���C�L���X�g�̏���
        private void HandleRayCast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_IsRay = Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask("Default")) &&
                      hit.transform.CompareTag("Building");
        }

        // �ޗ��̍X�V
        private void UpdateMaterial()
        {
            if (m_IsRay)
            {
                m_Time += Time.deltaTime;
                var intensity = (-Mathf.Cos(m_Time * speed) + 1) / 3;
                var emissionColor = baseEmissionColor + intensity * intensityMultiplier * Color.white;
                m_Material.SetColor("_EmissionColor", emissionColor);
            }
            else
            {
                ResetMaterial();
            }
        }

        // �ޗ������Z�b�g
        private void ResetMaterial()
        {
            m_Time = 0;
            baseEmissionColor = Color.black;
            m_Material.SetColor("_EmissionColor", baseEmissionColor);
        }

        private enum State
        {
            OnGameOpen,
            Normal,
            OnEnterGaming
        }
    }
}