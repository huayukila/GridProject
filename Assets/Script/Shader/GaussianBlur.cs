using UnityEngine;

public class GaussianBlur : PostEffectsBase
{
    public Shader gaussianBlurShader;

    //Ÿ”
    [Range(0, 4)] public int iterations = 3;

    //”ÍˆÍ
    [Range(0.2f, 3.0f)] public float blurSpread = 0.6f;

    //scaleŒn”  
    [Range(1, 8)] public int downSample = 2;
    private Material gaussianBlurMaterial;

    public Material material
    {
        get
        {
            gaussianBlurMaterial = CheckShaderAndCreateMaterial(gaussianBlurShader, gaussianBlurMaterial);
            return gaussianBlurMaterial;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            var rtW = src.width / downSample;
            var rtH = src.height / downSample;

            var buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);

            buffer0.filterMode = FilterMode.Bilinear;
            Graphics.Blit(src, buffer0);

            for (var i = 0; i < iterations; i++)
            {
                material.SetFloat("_BlurSize", 1.0f + i * blurSpread);
                var buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, material, 0);
                RenderTexture.ReleaseTemporary(buffer0);

                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, material, 1);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }

            Graphics.Blit(buffer0, dest);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}