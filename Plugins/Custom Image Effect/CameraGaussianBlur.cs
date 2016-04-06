using UnityEngine;

public class CameraGaussianBlur : ImageEffectBase
{
    public int BlurSize;
    public override void SetMaterial(Material material, RenderTexture sourceTexture, RenderTexture destTexture)
    {
        int rtW = sourceTexture.width / 8;
        int rtH = sourceTexture.height / 8;


        RenderTexture rtTempA = RenderTexture.GetTemporary(rtW, rtH, 0, sourceTexture.format);
        rtTempA.filterMode = FilterMode.Bilinear;


        Graphics.Blit(sourceTexture, rtTempA);

        for (int i = 0; i < 2; i++)
        {

            float iteraionOffs = i * 1.0f;
            material.SetFloat("_blurSize", BlurSize + iteraionOffs);

            //vertical blur  
            RenderTexture rtTempB = RenderTexture.GetTemporary(rtW, rtH, 0, sourceTexture.format);
            rtTempB.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rtTempA, rtTempB, material, 0);
            RenderTexture.ReleaseTemporary(rtTempA);
            rtTempA = rtTempB;

            //horizontal blur  
            rtTempB = RenderTexture.GetTemporary(rtW, rtH, 0, sourceTexture.format);
            rtTempB.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rtTempA, rtTempB, material, 1);
            RenderTexture.ReleaseTemporary(rtTempA);
            rtTempA = rtTempB;

        }
        Graphics.Blit(rtTempA, destTexture);

        RenderTexture.ReleaseTemporary(rtTempA);
    }

    public override string ShaderName()
    {
        return "Shader Forge/Camera GaussianBlur";
    }
}
