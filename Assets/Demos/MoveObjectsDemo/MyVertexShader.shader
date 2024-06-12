// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/NewUnlitShader"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            uniform float4x4 _ObjectToWorld;
            StructuredBuffer<float3> CubeBuffer;

            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                v2f o;
                float4 bufferData = float4(CubeBuffer[instanceID], 0);
                float4x4 translationMatrix = float4x4(
                    1.0, 0.0, 0.0, bufferData.x,
                    0.0, 1.0, 0.0, bufferData.y,
                    0.0, 0.0, 1.0, bufferData.z,
                    0.0, 0.0, 0.0, 1.0
                );

                float4 worldPos = mul(translationMatrix, mul(_ObjectToWorld, v.vertex));
                o.vertex = UnityObjectToClipPos(worldPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
