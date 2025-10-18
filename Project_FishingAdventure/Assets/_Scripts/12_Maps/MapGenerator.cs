using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class MapGenerator
{
    public float noiseScale = 50f;
    public float offsetX = 0f;
    public float offsetY = 0f;

    public float landThreshold = 0.8f;
    public float deepSeaThreshold = 0.5f;
    public float midSeaThreshold = 0.7f;

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = new float[MapConstants.TileInWorldWidth, MapConstants.TileInWorldHeight];

        for (int y = 0; y < MapConstants.TileInWorldHeight; y++)
        {
            for (int x = 0; x < MapConstants.TileInWorldWidth; x++)
            {
                float sampleX = (float)x / noiseScale + offsetX;
                float sampleY = (float)y / noiseScale + offsetY;

                float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

                noiseMap[x, y] = noiseValue;
            }
        }

        return noiseMap;
    }

    public byte GetTileIDFromNoise(float noise)
    {
        if (noise >= landThreshold)
        {
            return MapConstants.TILE_LAND_PLAIN;
        }
        else
        {
            if (noise <= deepSeaThreshold) { return MapConstants.TILE_SEA_DEEP; }
            else if(noise<=midSeaThreshold){ return MapConstants.TILE_SEA_MID; }
            else { return MapConstants.TILE_SEA_SHALLOW; }
        }
    }
}
