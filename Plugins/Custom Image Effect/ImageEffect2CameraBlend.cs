using UnityEngine;
using System.Collections;

public class ImageEffect2CameraBlend : ImageEffectBase
{
    [Header("Camera Blend")]
    [Range(0f, 1f)]
    public float Blend;
    protected RenderTexture Camera2tex;
    public Camera Camera2;
    [Header("Play Parameter")]
    public float Speed;
    private float currentBlend;

    public bool FinishBlend
    {
        get
        {
            return currentBlend == 1f;
        }
    }
    public void CameraTransition(Camera CameraTo)
    {
        currentBlend = 0;
        Camera2 = CameraTo;
        if (Camera2)
            StartCoroutine(Transition());
    }
    protected override void OnEnable()
    {
        CameraTransition(Camera2);
    }
    IEnumerator Transition()
    {
        while (currentBlend < 1)
        {
            currentBlend += Speed * Time.deltaTime;
            currentBlend = Mathf.Clamp01(currentBlend);
            Blend = currentBlend;
            yield return null;
        }
    }
    public override void SetMaterial(Material material,RenderTexture sourceTexture,RenderTexture destTexture)
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
