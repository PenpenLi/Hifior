using UnityEngine;

public class ImageEffect2CameraBlend_AlphaRamp : ImageEffect2CameraBlend
{
    public bool Reverse;
    public Texture Ramp;

    public override void SetMaterial(Material material)
    {
        material.SetFloat("_Blend", (Blend - 0.5f) * 2);
        material.SetTexture("_Ramp", Ramp);
        material.SetFloat("_Reverse", Reverse ? 1 : 0);
        if (Camera2 != null)
            material.SetTexture("_MainTex2", Camera2tex);
    }
    public override string ShaderName()
    {
        return "Shader Forge/Blend2Camera AlphaRamp";
    }
}