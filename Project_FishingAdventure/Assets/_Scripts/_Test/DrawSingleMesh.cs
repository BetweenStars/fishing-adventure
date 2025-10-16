using UnityEngine;

public class DrawSingleMesh : MonoBehaviour
{
    public Material quadMaterial; // 인스턴싱 활성화된 재질 (쉐이더 할당)
    private Mesh centerMesh;
    private Matrix4x4[] instanceMatrix; // 단일 인스턴스를 위한 행렬 배열

    void OnEnable()
    {
        // 1. 1x1 쿼드 메쉬 생성
        centerMesh = CreateQuadMesh(1f); // PPU=1f 가정 (월드 유닛 1x1 크기)

        instanceMatrix = new Matrix4x4[5];

        SetMatrixArray();
    }

    private void SetMatrixArray()
    {
        int x = -2;
        int y = -2;
        for (int i = 0; i < instanceMatrix.Length; i++)
        {
            Vector3 pos = new Vector3(x, y, 0);
            Quaternion rot = Quaternion.identity;
            Vector3 scale = Vector3.one;

            instanceMatrix[i] = Matrix4x4.TRS(pos, rot, scale);

            x += 1;
            y += 1;
        }
    }

    void Update()
    {
        if (quadMaterial == null || centerMesh == null) return;

        // 단일 드로우 콜로 하나의 쿼드를 렌더링
        Graphics.DrawMeshInstanced(
            centerMesh,
            0,
            quadMaterial,
            instanceMatrix,
            instanceMatrix.Length
        );
    }

    // 이전에 제공된 1x1 쿼드 메쉬 생성 함수를 재사용
    private Mesh CreateQuadMesh(float unitSize)
    {
        Mesh mesh = new Mesh();
        mesh.name = "CenterQuad";

        float halfSize = unitSize * 0.5f;

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
}
