using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    private Dictionary<Vector2Int, ChunkData> allWorldChunks = new();
    public MapGenerator mapGenerator = new();
    [SerializeField] private RuleTile LandRuleTile;

    private void Awake()
    {
        InitializeWorld();

        tilePalette = new Dictionary<byte, Tile>
    {
        { MapConstants.TILE_SEA_SHALLOW, SeaShallowTile },
        { MapConstants.TILE_SEA_MID, SeaMidTile },
        { MapConstants.TILE_SEA_DEEP, SeaDeepTile },
        { MapConstants.TILE_LAND_PLAIN, LandPlainTile },
        { MapConstants.TILE_LAND_DESERT, LandDesertTile },
        { MapConstants.TILE_LAND_TUNDRA, LandTundraTile },
        { MapConstants.TILE_LAND_JUNGLE, LandJungleTile }
    };
    }

    public void InitializeWorld()
    {
        allWorldChunks = new();

        float[,] noiseMap = mapGenerator.GenerateNoiseMap();

        for (int y = 0; y < MapConstants.ChunkPerHeight; y++)
        {
            for (int x = 0; x < MapConstants.ChunkPerWidth; x++)
            {
                Vector2Int chunkCoord = new Vector2Int(x, y);

                if (!allWorldChunks.ContainsKey(chunkCoord))
                {
                    allWorldChunks.Add(chunkCoord, InitializeChunk(chunkCoord, noiseMap));
                }
            }
        }
    }

    public ChunkData InitializeChunk(Vector2Int coord, float[,] noiseMap)
    {
        ChunkData chunk = new ChunkData(coord.x, coord.y, BiomeType.Plain);

        for (int localY = 0; localY < MapConstants.TilePerChunk; localY++)
        {
            for (int localX = 0; localX < MapConstants.TilePerChunk; localX++)
            {
                Vector2Int noiseCoord = new Vector2Int(coord.x * MapConstants.TilePerChunk + localX,
                coord.y * MapConstants.TilePerChunk + localY
                );
                byte tileID = mapGenerator.GetTileIDFromNoise(noiseMap[noiseCoord.x, noiseCoord.y]);

                chunk.tileIDs[localY * MapConstants.TilePerChunk + localX] = tileID;
            }
        }

        return chunk;
    }

    [Header("Tile Palette Mapping")]
    public Tile SeaShallowTile; // ID 0
    public Tile SeaMidTile;     // ID 1
    public Tile SeaDeepTile;    // ID 2
    public Tile LandPlainTile;  // ID 3
    public Tile LandDesertTile; // ID 4
    public Tile LandTundraTile; // ID 5
    public Tile LandJungleTile; // ID 6

    private Dictionary<byte, Tile> tilePalette;

    public void DrawAllChunks()
    {
        foreach (var kvp in allWorldChunks)
        {
            Vector2Int chunkCoord = kvp.Key;
            ChunkData chunk = kvp.Value;

            List<Vector3Int> seaPositions = new List<Vector3Int>();
            List<Tile> seaTiles = new List<Tile>();
            List<Vector3Int> landPositions = new List<Vector3Int>();
            List<Tile> landTiles = new List<Tile>();

            int startWorldX = chunkCoord.x * MapConstants.TilePerChunk;
            int startWorldY = chunkCoord.y * MapConstants.TilePerChunk;

            for (int localY = 0; localY < MapConstants.TilePerChunk; localY++)
            {
                for (int localX = 0; localX < MapConstants.TilePerChunk; localX++)
                {
                    int arrayIndex = localY * MapConstants.TilePerChunk + localX;
                    byte tileId = chunk.tileIDs[arrayIndex];

                    Vector3Int tilePos = new Vector3Int(startWorldX + localX, startWorldY + localY, 0);

                    if (tilePalette.TryGetValue(tileId, out Tile tileAsset))
                    {
                        if (tileId <= MapConstants.TILE_SEA_DEEP)
                        {
                            seaPositions.Add(tilePos);
                            seaTiles.Add(tileAsset);
                        }
                        else
                        {
                            landPositions.Add(tilePos);
                            landTiles.Add(tileAsset);
                        }
                    }
                }
            }

            if (seaPositions.Count > 0)
            {
                TilemapManager.Instance.oceanTilemap.SetTiles(seaPositions.ToArray(), seaTiles.ToArray());
            }
            if (landPositions.Count > 0)
            {
                TilemapManager.Instance.landTilemap.SetTiles(landPositions.ToArray(), landTiles.ToArray());
            }
        }

        if (TilemapManager.Instance.landTilemap.GetComponent<TilemapCollider2D>() != null)
        {
            TilemapManager.Instance.landTilemap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
        }

        Debug.Log("전체 맵 타일 배치 완료.");
    }

    private bool mapDraw = false;
    void Update()
    {
        if (TilemapManager.Instance != null && !mapDraw)
        {
            DrawAllChunks();
            mapDraw = true;
        }
    }
}