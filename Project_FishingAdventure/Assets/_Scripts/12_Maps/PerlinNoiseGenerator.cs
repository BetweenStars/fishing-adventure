using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public int width = 320;
    public int height = 1600;

    public float scale = 50f;
    public float persistence = 0.5f;
    public float lacunarity = 2.5f;
    public int octaves = 5;

    public int offsetX = 0;
    public int offsetY = 0;

    public float[,] GetNoiseMap()
    {
        float[,] noise = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float frequency = 1.0f;
                float amplitude = 1.0f;
                float noiseHeight = 0.0f;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = x / scale * frequency + offsetX;
                    float yCoord = y / scale * frequency + offsetY;

                    float perlinNoise = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;

                    noiseHeight += perlinNoise * amplitude;
                    frequency *= lacunarity;
                    amplitude *= persistence;
                }

                noise[x, y] = noiseHeight*0.8f;
            }
        }

        return noise;
    }
}
