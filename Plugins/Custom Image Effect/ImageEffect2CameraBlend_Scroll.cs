using UnityEngine;

public class ImageEffect2CameraBlend_Scroll : ImageEffect2CameraBlend
{
    public bool Direction;
    public bool Reverse;

    public override void SetMaterial(Material material,RenderTexture sourceTexture,RenderTexture destTexture)
    {
        material.SetFloat("_Blend", Blend);
        material.SetFloat("_Reverse", Reverse ? 1f : 0f);
        material.SetFloat("_Direction", Direction ? 1f : 0f);

        if (Camera2 != null)
            material.SetTexture("_MainTex2",Camera2tex);
    }
    public override string ShaderName()
    {
        return "Shader Forge/Blend2Camera Scroll";
    }
}
