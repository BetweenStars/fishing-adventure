Shader "Custom/CenterInstancedShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // 색상
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing // GPU 인스턴싱 필수

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID // 인스턴스 ID 매크로
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color;

            // ----------------------------------------------------
            // 버텍스 쉐이더: CPU가 보낸 Matrix4x4 적용
            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v); 
                
                // CPU가 보낸 Matrix4x4 (instanceMatrix[0])를 사용하여 
                // 메쉬의 로컬 정점을 월드 공간으로 변환합니다.
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                // 월드 공간 위치를 클립 공간(화면) 위치로 변환
                o.pos = mul(UNITY_MATRIX_VP, worldPos);

                return o;
            }
            // ----------------------------------------------------

            // ----------------------------------------------------
            // 프래그먼트 쉐이더: 단색 출력
            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            // ----------------------------------------------------
            
            ENDCG
        }
    }
}