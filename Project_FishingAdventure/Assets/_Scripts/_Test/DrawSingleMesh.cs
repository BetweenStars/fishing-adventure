using System.Collections.Generic;
using UnityEngine;

public class DrawSingleMesh : MonoBehaviour
{
    public Material quadMaterial; // 인스턴싱 활성화된 재질 (쉐이더 할당)
    private Mesh centerMesh;

    private List<Matrix4x4> matrixList;
    private Matrix4x4[] matrixBuffer;

    public Transform startPoint;
    public Transform endPoint;

    public float PPU = 32f;

    void OnEnable()
    {
        // 1. 1x1 쿼드 메쉬 생성
        centerMesh = CreateQuadMesh(PPU); // PPU=1f 가정 (월드 유닛 1x1 크기)

        CalculatePixelsBetweenPoints(startPoint.position, endPoint.position);
    }

    private List<Matrix4x4> SetMatrixList(List<Vector3> vectorList)
    {
        List<Matrix4x4> matrixList = new();

        for(int i = 0; i < vectorList.Count; i++)
        {
            Matrix4x4 matrix;

            Vector3 pos = vectorList[i];
            Quaternion rot = Quaternion.identity;
            Vector3 scale = Vector3.one;

            matrix = Matrix4x4.TRS(pos, rot, scale);
            matrixList.Add(matrix);
        }

        return matrixList;
    }

    void Update()
    {
        if (quadMaterial == null || centerMesh == null) return;

        matrixList = new();
        matrixList = CalculatePixelsBetweenPoints(startPoint.position, endPoint.position);

        int matrixCount = matrixList.Count;

        if (matrixCount == 0) return;

        if (matrixBuffer == null || matrixBuffer.Length < matrixCount)
        {
            matrixBuffer = new Matrix4x4[matrixCount];
        }

        matrixBuffer = matrixList.ToArray();

        // 단일 드로우 콜로 하나의 쿼드를 렌더링
        Graphics.DrawMeshInstanced(
            centerMesh,
            0,
            quadMaterial,
            matrixBuffer,
            matrixCount
        );
    }

    // 이전에 제공된 1x1 쿼드 메쉬 생성 함수를 재사용
    private Mesh CreateQuadMesh(float unitSize)
    {
        Mesh mesh = new Mesh();
        mesh.name = "CenterQuad";

        float halfSize = 1f/unitSize * 0.5f;

        // 정점: 중심 (0,0,0)을 기준으로 -0.5 ~ +0.5
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-halfSize, -halfSize, 0); // 좌하
        vertices[1] = new Vector3(-halfSize, halfSize, 0);  // 좌상
        vertices[2] = new Vector3(halfSize, halfSize, 0);   // 우상
        vertices[3] = new Vector3(halfSize, -halfSize, 0);  // 우하

        // 삼각형 (Triangles)
        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        // UV 좌표 (UVs)
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    void OnDisable()
    {
        // 런타임에 생성된 메쉬를 정리 (에셋 누수 방지)
        if (centerMesh != null)
        {
            Destroy(centerMesh);
            centerMesh = null;
        }
    }

    private List<Matrix4x4> CalculatePixelsBetweenPoints(Vector3 start, Vector3 end)
    {
        start = start * PPU;
        end = end * PPU;

        int x1 = Mathf.RoundToInt(start.x);
        int x2 = Mathf.RoundToInt(end.x);
        int y1 = Mathf.RoundToInt(start.y);
        int y2 = Mathf.RoundToInt(end.y);

        Vector3 pos1 = new Vector3(x1, y1, 0)/PPU;
        Vector3 pos2 = new Vector3(x2, y2, 0)/PPU;
        List<Vector3> list = new();
        list.Add(pos1);
        list.Add(pos2);

        //position 다 구하고 마지막에 벡터 리스트 매트릭스 리스트로 변환
        return SetMatrixList(list);
    }
}
