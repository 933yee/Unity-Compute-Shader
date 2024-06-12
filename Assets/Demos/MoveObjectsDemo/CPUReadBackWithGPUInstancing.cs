using System.Collections.Generic;
using UnityEngine;

public class CPUReadBackWithGPUInstancing : MonoBehaviour
{
    public ComputeShader computeShader;
    private int _kernelIndex;
    private ComputeBuffer _CubeBuffer;
    private Vector3[] _CubeArray;

    public int Instances = 16384;
    public Mesh mesh;
    public Material material;
    private List<List<Matrix4x4>> batches = new List<List<Matrix4x4>>();

    private void InitCubes()
    {
        int AddedMatrices = 0;
        _CubeArray = new Vector3[Instances];
        batches.Add(new List<Matrix4x4>());

        for (int i = 0; i < Instances; i++)
        {
            if (AddedMatrices >= 1000)
            {
                AddedMatrices = 0;
                batches.Add(new List<Matrix4x4>());
            }
            else
            {
                AddedMatrices++;
                Vector3 position = new Vector3(i * 2, 0, transform.position.z);
                batches[batches.Count - 1].Add(Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 10f));
                _CubeArray[i] = position;
            }

        }
    }

    private void InitShader()
    {
        _kernelIndex = computeShader.FindKernel("CSMain");
        _CubeBuffer = new ComputeBuffer(_CubeArray.Length, 3 * sizeof(float));
        _CubeBuffer.SetData(_CubeArray);
        computeShader.SetBuffer(_kernelIndex, "CubeBuffer", _CubeBuffer);
    }
    private void Start()
    {
        InitCubes();
        InitShader();
    }

    void Update()
    {
        computeShader.SetFloat("Time", Time.time);
        computeShader.Dispatch(_kernelIndex, 16, 1, 1);
        _CubeBuffer.GetData(_CubeArray);
        int batchIndex = 0;
        for (int i = 0; i < batches.Count; i++)
        {
            for (int j = 0; j < batches[i].Count; j++)
            {
                batches[i][j] = Matrix4x4.TRS(
                    _CubeArray[batchIndex],
                    Quaternion.identity,
                    Vector3.one * 10f
                );
                batchIndex++;
            }
        }

        for (int i = 0; i < batches.Count; i++)
        {
            Graphics.DrawMeshInstanced(mesh, 0, material, batches[i]);
        }
    }

    void OnDestroy()
    {
        if (_CubeBuffer != null)
        {
            _CubeBuffer.Release();
        }
    }
}
