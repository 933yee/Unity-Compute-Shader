using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComputeShaderBasic : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    private int _kernelIndex;
    void Start()
    {
        _kernelIndex = computeShader.FindKernel("CSMain");

        computeShader.SetTexture(_kernelIndex, "Result", renderTexture);
        computeShader.SetFloat("Resolution", renderTexture.width);
        computeShader.Dispatch(_kernelIndex, renderTexture.width / 8, renderTexture.height / 8, 1);
    }
}
