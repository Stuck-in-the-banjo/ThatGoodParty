using UnityEngine;

[ExecuteInEditMode]
public class BlurShader : MonoBehaviour
{
    public Material BlurMaterial;
    [Range(0, 10)]
    public int iterations;
    [Range(0, 4)]
    public int downResolution;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        int width = src.width >> downResolution;
        int height = src.height >> downResolution;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(src, rt);

        for (int i = 0; i < iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, BlurMaterial);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, dst);
        RenderTexture.ReleaseTemporary(rt);
    }
}