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

        private void Start()
        {
            m_IsRay = false;
            m_Material = renderer.sharedMaterial;
        }

        private void Update()
        {
            RayCast();
            if (m_IsRay)
            {
                m_Time += Time.deltaTime;
                float intensity = (-Mathf.Cos(m_Time * speed) + 1) / 3;
                Color emissionColor = baseEmissionColor + intensity * intensityMultiplier * Color.white;
                m_Material.SetColor("_EmissionColor", emissionColor);
            }
            else
            {
                m_Time = 0;
                baseEmissionColor = Color.black;
                m_Material.SetColor("_EmissionColor", baseEmissionColor);
            }
        }

        void RayCast()
        {
            m_IsRay = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask("Default")))
                return;
            if (hit.transform.CompareTag("Building"))
            {
                m_IsRay = true;
            }
        }
    }
}