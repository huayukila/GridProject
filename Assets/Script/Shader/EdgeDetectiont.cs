using UnityEngine;

public class EdgeDetection : PostEffectsBase
{
    public Shader edgeDetectShader;

    [Range(0.0f, 1.0f)]
    //これ1になると、完全に元の映像見えない
    public float edgesOnly;

    public Color edgeColor = Color.black;

    public Color backgroundColor = Color.white;
    private Material edgeDetectMaterial;

    public Material material
    {
        get
        {
            edgeDetectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDetectMaterial);
            return edgeDetectMaterial;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_EdgeOnly", edgesOnly);
            material.SetColor("_EdgeColor", edgeColor);
            material.SetColor("_BackgroundColor", backgroundColor);

            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}