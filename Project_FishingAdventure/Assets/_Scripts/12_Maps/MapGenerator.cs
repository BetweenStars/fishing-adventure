using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MapGenerator
{
    public float noiseScale = 20f;
    public float persistence = 1f;
    public float lucunarity = 1.8f;

    public int octaves = 5;


    public float deepSeaThreshold = -0.2f;
    public float midSeaThreshold = 0.1f;

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = new float[MapConstants.TileInWorldWidth, MapConstants.TileInWorldHeight];

        for (int y = 0; y < MapConstants.TileInWorldHeight; y++)
        {
            for (int x = 0; x < MapConstants.TileInWorldWidth; x++)
            {
                float amplitude = 1.0f;
                float frequency = 1.0f;
                float noiseHeight = 0.0f;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (float)x / noiseScale + frequency;
                    float sampleY = (float)y / noiseScale + frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lucunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        return noiseMap;
    }

    private float landThreshold = 0.5f;
    public byte GetTileIDFromNoise(float noise)
    {
        if (noise >= landThreshold) { return MapConstants.TILE_LAND_PLAIN; }
        else
        {
            if (noise <= deepSeaThreshold) { return MapConstants.TILE_SEA_DEEP; }
            else if (noise <= midSeaThreshold) { return MapConstants.TILE_SEA_MID; }
            else { return MapConstants.TILE_SEA_SHALLOW; }
        }
    }
}
