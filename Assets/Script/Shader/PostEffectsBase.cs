using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PostEffectsBase : MonoBehaviour
{
// Use this for initialization
    private void Start()
    {
    }

    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null) return null;

        if (shader.isSupported && material && material.shader == shader) return material;

        if (!shader.isSupported) return null;

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if (material)
            return material;
        return null;
    }
}