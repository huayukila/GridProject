using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class Bloom : PostEffectsBase
{
    public Shader bloomShader;
    [SerializeField] [Range(0, 3)] public float threshold = 0.8f;
    [SerializeField] [Range(0, 7)] public int blurTime = 5;
    private Material bloomMaterial;

    public Material material
    {
        get
        {
            bloomMaterial = CheckShaderAndCreateMaterial(bloomShader, bloomMaterial);
            return bloomMaterial;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //‘æˆê”Åbloom
        // material.SetFloat("_Threshold", threshold);
        // material.SetTexture("_SourceTex", source);
        // int width = source.width;
        // int height = source.height;
        // RenderTexture tmpSource, tmpDest;
        // tmpSource = RenderTexture.GetTemporary(width, height, 0, source.format);
        // Graphics.Blit(source, tmpSource, material, 0);
        // tmpDest = tmpSource;
        // RenderTexture[] textures = new RenderTexture[blurTime];
        // int i = 1;
        // for (; i < blurTime; i++)
        // {
        //     width /= 2;
        //     height /= 2;
        //     tmpDest = RenderTexture.GetTemporary(width, height, 0, source.format);
        //     textures[i] = tmpDest;
        //     Graphics.Blit(tmpSource, tmpDest, material, 1);
        //     RenderTexture.ReleaseTemporary(tmpSource);
        //     tmpSource = tmpDest;
        // }
        //
        // for (i -= 1; i > 0; i--)
        // {
        //     RenderTexture.ReleaseTemporary(tmpDest);
        //     tmpDest = textures[i];
        //     textures[i] = null;
        //     Graphics.Blit(tmpSource, tmpDest, material, 2);
        //     RenderTexture.ReleaseTemporary(tmpSource);
        //     tmpSource = tmpDest;
        // }
        // Graphics.Blit(tmpSource, destination, material, 3);
        material.SetFloat("_Threshold", threshold);
        material.SetTexture("_SourceTex", source);

        var width = source.width;
        var height = source.height;

        var tmpSource = RenderTexture.GetTemporary(width, height, 0, source.format);
        Graphics.Blit(source, tmpSource, material, 0);

        var textures = new RenderTexture[blurTime];

        for (var i = 1; i < blurTime; i++)
        {
            width /= 2;
            height /= 2;

            var tmpDest = RenderTexture.GetTemporary(width, height, 0, source.format);
            textures[i] = tmpDest;

            Graphics.Blit(tmpSource, tmpDest, material, 1);

            RenderTexture.ReleaseTemporary(tmpSource);
            tmpSource = tmpDest;
        }

        Graphics.Blit(tmpSource, destination, material, 3);

        RenderTexture.ReleaseTemporary(tmpSource);

        for (var i = 1; i < blurTime; i++) RenderTexture.ReleaseTemporary(textures[i]);
    }
}