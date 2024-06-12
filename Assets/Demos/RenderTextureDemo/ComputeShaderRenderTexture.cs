using UnityEngine;

public class ComputeShaderRenderTexture : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    private int _kernelIndex;
    void Start()
    {
        _kernelIndex = computeShader.FindKernel("CSMain");

        computeShader.SetTexture(_kernelIndex, "Result", renderTexture);
        computeShader.SetFloat("Resolution", renderTexture.width);
        computeShader.SetFloat("Time", 0);
    }

    void Update()
    {
        computeShader.SetFloat("Time", Time.time);
        computeShader.Dispatch(_kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
    }
}
