using UnityEngine;

public class EdgeDetection : PostEffectsBase
{
    public Shader edgeDetectShader;

    [Range(0.0f, 1.0f)]
    //Ç±ÇÍ1Ç…Ç»ÇÈÇ∆ÅAäÆëSÇ…å≥ÇÃâfëúå©Ç¶Ç»Ç¢
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