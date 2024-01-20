using Unity.Mathematics;
using UnityEngine;

namespace Framework.BuildProject
{
    public class TitleController : BuildController
    {
        public float intensityMultiplier = 1.0f;
        public MeshRenderer renderer;
        public float speed;

        private bool m_IsRay;
        private Material m_Material;
        private Color baseEmissionColor;
        private float m_Time;

        private Camera m_MainCamera;
        private BrightnessSaturationAndContrast m_Brightness;

        private bool m_GameStart = false;

        // スタート時の初期化
        private void Start()
        {
            m_IsRay = false;
            m_Material = renderer.sharedMaterial;
            m_MainCamera = Camera.main;
            m_Brightness = m_MainCamera.GetComponent<BrightnessSaturationAndContrast>();
        }

        // 毎フレームの更新
        private void Update()
        {
            if (m_GameStart)
            {
                HandleGameStart();
                return;
            }

            HandleRayCast();
            UpdateMaterial();
        }

        // ゲームスタート時の処理
        private void HandleGameStart()
        {
            m_Brightness.brightness -= Time.deltaTime * 0.3f;
            if (m_Brightness.brightness <= 0f)
            {
                m_Material.SetColor("_EmissionColor", 0.66f * Color.white);
                this.SendEvent<GameStartEvent>();
            }
        }

        // レイキャストの処理
        private void HandleRayCast()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_IsRay = Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask("Default")) &&
                      hit.transform.CompareTag("Building");
        }

        // 材料の更新
        private void UpdateMaterial()
        {
            if (m_IsRay)
            {
                m_Time += Time.deltaTime;
                float intensity = (-Mathf.Cos(m_Time * speed) + 1) / 3;
                Color emissionColor = baseEmissionColor + intensity * intensityMultiplier * Color.white;
                m_Material.SetColor("_EmissionColor", emissionColor);

                if (Input.GetMouseButtonDown(0))
                {
                    m_GameStart = true;
                }
            }
            else
            {
                ResetMaterial();
            }
        }

        // 材料をリセット
        private void ResetMaterial()
        {
            m_Time = 0;
            baseEmissionColor = Color.black;
            m_Material.SetColor("_EmissionColor", baseEmissionColor);
        }
    }
}