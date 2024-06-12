using UnityEngine;

public class IndirectRendering : MonoBehaviour
{
    public ComputeShader computeShader;
    private int _kernelIndex;
    private ComputeBuffer _CubeBuffer;
    private Vector3[] _CubeArray;

    public int Instances = 16384;
    public Mesh mesh;
    public Material material;

    private void InitCubes()
    {
        _CubeArray = new Vector3[Instances];

        for (int i = 0; i < Instances; i++)
        {
            Vector3 position = new Vector3(i * 2, 0, transform.position.z);
            _CubeArray[i] = position;
        }
    }

    private void InitShader()
    {
        _kernelIndex = computeShader.FindKernel("CSMain");
        _CubeBuffer = new ComputeBuffer(_CubeArray.Length, 3 * sizeof(float));
        _CubeBuffer.SetData(_CubeArray);
        computeShader.SetBuffer(_kernelIndex, "CubeBuffer", _CubeBuffer);
        material.SetBuffer("CubeBuffer", _CubeBuffer);
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

        RenderParams rp = new RenderParams(material);
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 10f));
        Graphics.RenderMeshPrimitives(rp, mesh, 0, Instances);
    }

    void OnDestroy()
    {
        if (_CubeBuffer != null)
        {
            _CubeBuffer.Release();
        }
    }
}
