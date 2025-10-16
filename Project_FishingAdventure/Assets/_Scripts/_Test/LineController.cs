using UnityEngine;
using System.Collections.Generic;

public class LineController : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public float PPU = 32f; // Pixels Per Unit
    
    public Material instancingMaterial;
    public Mesh pixelMesh;

    // 🚨 Matrix4x4 리스트를 사용하여 바로 저장합니다.
    private List<Matrix4x4> instanceMatrices; 
    
    void OnEnable()
    {
        // Vector3 대신 Matrix4x4 리스트로 변경
        instanceMatrices = new List<Matrix4x4>();
        pixelMesh = CreatePixelQuad(PPU);
    }

    void Update()
    {
        if (startTransform == null || endTransform == null || instancingMaterial == null || pixelMesh == null) return;

        // 1. 픽셀 위치를 Matrix4x4로 계산하여 저장합니다.
        CalculatePixelLinePoints(startTransform.position, endTransform.position, PPU);

        // 2. GPU 인스턴싱 드로우 요청
        int count = instanceMatrices.Count;

        if (count > 0)
        {
            // Graphics.DrawMeshInstanced에 Matrix4x4 배열을 직접 전달합니다.
            Graphics.DrawMeshInstanced(
                pixelMesh,
                0,
                instancingMaterial,
                instanceMatrices.ToArray(), // ⬅️ Matrix4x4 배열
                count
            );
        }
    }

    // OnDisable(), ReleaseBuffer() 등 ComputeBuffer 관련 코드는 이제 필요 없으므로 제거됩니다.

    /// <summary>
    /// 두 좌표 사이의 픽셀 위치를 계산하고, 즉시 Matrix4x4 변환 행렬로 변환하여 저장합니다.
    /// </summary>
    private void CalculatePixelLinePoints(Vector3 startWorld, Vector3 endWorld, float ppu)
    {
        instanceMatrices.Clear();
        float scale = 1f / ppu;
        Vector3 scaleVector = new Vector3(scale, scale, 1f);
        Quaternion rotation = Quaternion.identity;

        // --- 픽셀 좌표 계산 로직 ---

        // 1. 월드 좌표를 픽셀 좌표로 변환
        Vector2 startPixel = startWorld * ppu;
        Vector2 endPixel = endWorld * ppu;

        // Bresenham 알고리즘을 위한 정수 좌표
        int x1 = Mathf.RoundToInt(startPixel.x);
        int y1 = Mathf.RoundToInt(startPixel.y);
        int x2 = Mathf.RoundToInt(endPixel.x);
        int y2 = Mathf.RoundToInt(endPixel.y);

        // Bresenham 알고리즘 변수 초기화
        int dx = Mathf.Abs(x2 - x1);
        int dy = Mathf.Abs(y2 - y1);
        int sx = (x1 < x2) ? 1 : -1;
        int sy = (y1 < y2) ? 1 : -1;
        int err = dx - dy;

        float halfPixelOffset = 0.5f / ppu; // 픽셀 중앙으로 위치 보정

        while (true)
        {
            // 2. 픽셀 좌표를 다시 월드 좌표로 변환
            // Z축은 startWorld의 Z값을 사용합니다.
            Vector3 worldPosition = new Vector3(
                (float)x1 / ppu + halfPixelOffset,
                (float)y1 / ppu + halfPixelOffset,
                startWorld.z
            );

            // 3. Matrix4x4로 변환하여 리스트에 저장
            Matrix4x4 matrix = Matrix4x4.TRS(worldPosition, rotation, scaleVector);
            instanceMatrices.Add(matrix);

            if (x1 == x2 && y1 == y2) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
        Debug.Log(instanceMatrices.Count);
    }
    
    public Mesh CreatePixelQuad(float ppu)
    {
        Mesh mesh = new Mesh();
        mesh.name = $"PixelQuad_PPU{ppu}";

        float size = 0.5f / ppu; // 중심을 (0,0)으로 맞추기 위해 1/PPU의 절반 크기 사용
        
        // 정점 (Vertices): 4개의 코너 정의
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-size, -size, 0); // 좌하
        vertices[1] = new Vector3(-size, size, 0);  // 좌상
        vertices[2] = new Vector3(size, size, 0);   // 우상
        vertices[3] = new Vector3(size, -size, 0);  // 우하

        // 삼각형 (Triangles): 2개의 삼각형으로 쿼드 형성
        int[] triangles = new int[] {
            0, 1, 2, // 첫 번째 삼각형 (좌하, 좌상, 우상)
            0, 2, 3  // 두 번째 삼각형 (좌하, 우상, 우하)
        };

        // UV 좌표 (UVs): 텍스처를 입힐 경우를 대비해 0~1 값 할당
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateBounds(); // 렌더링에 필요
        //mesh.RecalculateNormals(); // 3D 라이팅에 필요 (2D에서는 선택 사항)

        return mesh;
    }
}