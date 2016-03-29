using UnityEngine;

public class ImageEffect2CameraBlend_Ramp : ImageEffect2CameraBlend
{
    public Texture Ramp;
    public bool Reverse;
    [Range(-0.5f, 0.5f)]
    public float Bias;

    public override void SetMaterial(Material material)
    {
        material.SetFloat("_Blend",(Blend-0.1f)*1.1111111f);
        material.SetFloat("_Bias", Bias);
        material.SetTexture("_Ramp", Ramp);
        material.SetFloat("_Reverse", Reverse ? 1 : 0);
        if (Camera2 != null)
            material.SetTexture("_MainTex2", Camera2tex);
    }
    public override string ShaderName()
    {
        return "Shader Forge/Blend2Camera Ramp";
    }
}
