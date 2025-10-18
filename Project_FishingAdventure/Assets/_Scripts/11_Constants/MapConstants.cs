using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Timeline;

public static class MapConstants
{
    public const int TilePerChunk = 16;

    public const int ChunkPerWidth = 20;
    public const int ChunkPerHeight = 20;

    public const int TileInWorldWidth = ChunkPerWidth * TilePerChunk;
    public const int TileInWorldHeight = ChunkPerHeight * TilePerChunk;

    //SEA TILE
    public const byte TILE_SEA_SHALLOW = 0;
    public const byte TILE_SEA_MID = 1;
    public const byte TILE_SEA_DEEP = 2;
    //LAND TILE
    public const byte TILE_LAND_PLAIN = 3;
    public const byte TILE_LAND_DESERT = 4;
    public const byte TILE_LAND_TUNDRA = 5;
    public const byte TILE_LAND_JUNGLE = 6;
}