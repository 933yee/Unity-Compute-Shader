using System;
using UnityEngine;

public class CPUReadBack : MonoBehaviour
{
    public ComputeShader computeShader;
    private int _kernelIndex;
    private ComputeBuffer _CubeBuffer;
    private Vector3[] _CubeArray;
    private GameObject[] _Cubes;
    public int Instances = 16384;

    private void InitCubes()
    {
        _Cubes = new GameObject[Instances];
        _CubeArray = new Vector3[Instances];
        for (int i = 0; i < _CubeArray.Length; i++)
        {
            _Cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _Cubes[i].transform.localScale = new Vector3(10, 10, 10);
            _Cubes[i].transform.position = new Vector3(i * 2, 0, transform.position.z);
            _CubeArray[i] = _Cubes[i].transform.position;
        }
    }

    private void InitShader()
    {
        _kernelIndex = computeShader.FindKernel("CSMain");
        _CubeBuffer = new ComputeBuffer(_CubeArray.Length, 3 * sizeof(float));
        _CubeBuffer.SetData(_CubeArray);
        computeShader.SetBuffer(_kernelIndex, "CubeBuffer", _CubeBuffer);
    }


    void Start()
    {
        InitCubes();
        InitShader();
    }

    private void Update()
    {
        computeShader.SetFloat("Time", Time.time);
        computeShader.Dispatch(_kernelIndex, 16, 1, 1);

        _CubeBuffer.GetData(_CubeArray);
        for (int i = 0; i < _CubeArray.Length; i++)
        {
            _Cubes[i].transform.position = _CubeArray[i];
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
