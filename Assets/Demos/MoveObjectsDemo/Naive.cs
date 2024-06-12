using System;
using UnityEngine;

public class Naive : MonoBehaviour
{
    private GameObject[] _Cubes;
    public int Instances = 16384;

    private void InitCubes()
    {
        _Cubes = new GameObject[Instances];
        for (int i = 0; i < _Cubes.Length; i++)
        {
            _Cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _Cubes[i].transform.localScale = new Vector3(10, 10, 10);
            _Cubes[i].transform.position = new Vector3(i * 2, 0, transform.position.z);
        }
    }

    void Start()
    {
        InitCubes();
    }

    private void Update()
    {
        for (int i = 0; i < _Cubes.Length; i++)
        {
            float x = _Cubes[i].transform.position.x;
            float y = _Cubes[i].transform.position.y;
            float z = _Cubes[i].transform.position.z;

            // 我隨便亂寫的位置移動公式
            Vector3 newPos = new Vector3(
                    (float)x,
                    (float)(Math.Tan(x / 500 + Time.time) - Math.Cos(x / 10 + z / 200 + Time.time)) * 100,
                    (float)Math.Sin(x / 10 + y / 200 - Time.time) * 200
                );
            _Cubes[i].transform.position = newPos;
        }
    }
}
