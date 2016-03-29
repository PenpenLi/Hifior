using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public abstract class ImageEffectBase : MonoBehaviour
{
    #region Variables  
    public Shader curShader;
    private Material curMaterial;
    #endregion

    #region Properties  
    public Material Material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    #endregion
    public abstract string ShaderName();
    public abstract void SetMaterial(Material material);
    // Use this for initialization  
    protected virtual void Start()
    {
        if (SystemInfo.supportsImageEffects == false)
        {
            enabled = false;
            return;
        }

        curShader = Shader.Find(ShaderName());
        if (curShader != null && curShader.isSupported == false)
        {
            Debug.Log(ShaderName() + " Shader is not exist or not supported!");
            enabled = false;
        }
        OnEnable();
    }
    protected virtual void OnEnable()
    {

    }
    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        SetMaterial(Material);
        if (curShader != null)
        {
            Graphics.Blit(sourceTexture, destTexture, Material);
        }
        else {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    protected virtual void OnDisable()
    {
        if (curMaterial != null)
        {
            DestroyImmediate(curMaterial);
        }
    }
}
