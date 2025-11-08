using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PixelLineDrawer : MonoBehaviour
{
    public Material quadMaterial;
    private Mesh centerMesh;

    private List<Matrix4x4> matrixList;
    private Matrix4x4[] matrixBuffer;

    public Transform startPoint;
    public Transform endPoint;

    void OnEnable()
    {
        centerMesh = CreateQuadMesh(ResolutionConstants.PPU);

        matrixList = new();
    }
    void OnDisable()
    {
        if (centerMesh != null)
        {
            Destroy(centerMesh);
            centerMesh = null;
        }
    }

    void Update()
    {
        if (quadMaterial == null || centerMesh == null || startPoint == null || endPoint == null) return;

        CalculatePixelsBetweenPoints(startPoint.position, endPoint.position, ResolutionConstants.PPU);

        int matrixCount = matrixList.Count;
        if (matrixCount == 0) return;

        if (matrixBuffer == null || matrixBuffer.Length < matrixCount)
        {
            matrixBuffer = new Matrix4x4[matrixCount];
        }

        matrixList.CopyTo(matrixBuffer, 0);

        var renderParams = new RenderParams(quadMaterial){};
        renderParams.renderingLayerMask = 3;
        renderParams.camera = Camera.main;
        
        
        Graphics.RenderMeshInstanced(renderParams, centerMesh, 0, matrixBuffer, matrixCount);
    }

    public void SetStartEndTransforms(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
    }

    private Mesh CreateQuadMesh(float unitSize)
    {
        Mesh mesh = new Mesh();
        mesh.name = "CenterQuad";

        float halfSize = 1f / unitSize * 0.5f;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-halfSize, -halfSize, 0); // 좌하
        vertices[1] = new Vector3(-halfSize, halfSize, 0);  // 좌상
        vertices[2] = new Vector3(halfSize, halfSize, 0);   // 우상
        vertices[3] = new Vector3(halfSize, -halfSize, 0);  // 우하

        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

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

    private Matrix4x4 GetMatrixFromVector(Vector3 vector)
    {
        Vector3 pos = vector;
        Quaternion rot = Quaternion.identity;
        Vector3 scale = Vector3.one;

        Matrix4x4 matrix = Matrix4x4.TRS(pos, rot, scale);

        return matrix;
    }

    private void CalculatePixelsBetweenPoints(Vector3 start, Vector3 end, float PPU)
    {
        matrixList.Clear();

        Vector2 startPixel = start * PPU;
        Vector2 endPixel = end * PPU;

        int x1 = Mathf.RoundToInt(startPixel.x);
        int x2 = Mathf.RoundToInt(endPixel.x);
        int y1 = Mathf.RoundToInt(startPixel.y);
        int y2 = Mathf.RoundToInt(endPixel.y);

        int dx = Mathf.Abs(x2 - x1);
        int dy = Mathf.Abs(y2 - y1);
        int sx = (x1 < x2) ? 1 : -1;
        int sy = (y1 < y2) ? 1 : -1;
        int err = dx - dy;

        float pixelOffset = 0.5f / PPU;

        while (true)
        {
            Vector3 worldPosition = new Vector3(
            (float)x1 / PPU + pixelOffset,
            (float)y1 / PPU + pixelOffset,
            -0.1f
            );

            matrixList.Add(GetMatrixFromVector(worldPosition));

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
    }
}
