using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum BiomeType : byte
{
    Plain = 0,
    Desert = 1,
    Tundra = 2,
    Jungle = 3
}

[Serializable]
public class ChunkData
{
    public const int ChunkSize = 16;
    public int chunkX;
    public int chunkY;

    public BiomeType chunkBiome;

    public byte[] tileIDs;

    public ChunkData(int x, int y, BiomeType biome)
    {
        chunkX = x;
        chunkY = y;
        chunkBiome = biome;
        tileIDs = new byte[ChunkSize * ChunkSize];
    }
    
    public void SetTileType(int tileX, int tileY, byte typeID)
    {
        if (tileX >= 0 && tileX < ChunkSize && tileY >= 0 && tileY < ChunkSize)
        {
            tileIDs[tileY * ChunkSize + tileX] = typeID;
        }
    }
}
