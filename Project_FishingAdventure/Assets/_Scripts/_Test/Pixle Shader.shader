Shader "Custom/PixelArtInstancedLine"
{
    Properties
    {
        _Color ("Line Color", Color) = (1,1,1,1) // 선의 색상
        _MainTex ("Texture", 2D) = "white" {} // 텍스처를 사용할 경우
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "DisableBatching" = "True" // 인스턴싱을 위해 배치 비활성화는 불필요하지만, 명시적으로 둡니다.
        }
        LOD 100

        Pass
        {
            // 투명도 처리 설정 (선이 겹치거나 투명할 경우)
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off // 2D에서 깊이 쓰기를 끄면 렌더링 순서에 유리할 수 있습니다.
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing // GPU 인스턴싱을 위한 필수 매크로

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID // 인스턴스 ID를 위한 매크로
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            sampler2D _MainTex;

            // ----------------------------------------------------
            // 버텍스 쉐이더: 각 인스턴스의 Matrix4x4를 적용하여 위치 설정
            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v); // 현재 렌더링 중인 인스턴스의 ID를 설정
                
                // 💡 핵심: 유니티의 표준 인스턴싱 매크로 사용
                // mul(unity_ObjectToWorld, v.vertex)를 사용하면 
                // CPU에서 전달된 Matrix4x4 배열(instanceMatrices)의 변환이 자동으로 적용됩니다.
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                // 최종 클립 공간 위치 계산
                o.pos = mul(UNITY_MATRIX_VP, worldPos);
                
                o.uv = v.uv; // UV 좌표 전달

                return o;
            }
            // ----------------------------------------------------

            // ----------------------------------------------------
            // 프래그먼트 쉐이더: 색상 결정
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // 픽셀 아트에서는 보통 앤티앨리어싱을 제거하고 싶으므로
                // 텍스처의 Filter Mode를 Point로 설정해야 합니다.

                return col;
            }
            // ----------------------------------------------------
            
            ENDCG
        }
    }
}