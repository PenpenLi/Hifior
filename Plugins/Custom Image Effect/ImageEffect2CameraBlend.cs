using UnityEngine;

public class ImageEffect2CameraBlend : ImageEffectBase
{
    [Range(0f, 1f)]
    public float Blend;
    protected RenderTexture Camera2tex;
    public Camera Camera2;

    public override void SetMaterial(Material material)
    {
        material.SetFloat("_Blend", Blend);
        if (Camera2 != null)
            material.SetTexture("_MainTex2", Camera2tex);
    }
    protected override void Start()
    {
        base.Start();
        Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
        if (Camera2)
            Camera2.targetTexture = Camera2tex;
    }
    public override string ShaderName()
    {
        return "Shader Forge/Blend2Camera Basic";
    }
}
