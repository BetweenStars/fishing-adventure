using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    private Dictionary<Vector2Int, ChunkData> allWorldChunks = new();
    public MapGenerator mapGenerator=new();

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

    // 타일 ID를 Tile Asset으로 변환하는 Dictionary (런타임용)
    private Dictionary<byte, Tile> tilePalette;

    public void DrawAllChunks()
    {
        // 모든 청크가 Dictionary에 초기화되었다고 가정합니다.

        foreach (var kvp in allWorldChunks)
        {
            Vector2Int chunkCoord = kvp.Key;
            ChunkData chunk = kvp.Value;

            // 이 청크의 모든 타일 정보와 위치를 저장할 임시 리스트
            List<Vector3Int> seaPositions = new List<Vector3Int>();
            List<Tile> seaTiles = new List<Tile>();
            List<Vector3Int> landPositions = new List<Vector3Int>();
            List<Tile> landTiles = new List<Tile>();

            // 청크의 월드 좌표 시작점
            int startWorldX = chunkCoord.x * MapConstants.TilePerChunk;
            int startWorldY = chunkCoord.y * MapConstants.TilePerChunk;

            // 1. 청크 내부 순회 및 데이터 분류
            for (int localY = 0; localY < MapConstants.TilePerChunk; localY++)
            {
                for (int localX = 0; localX < MapConstants.TilePerChunk; localX++)
                {
                    int arrayIndex = localY * MapConstants.TilePerChunk + localX;
                    byte tileId = chunk.tileIDs[arrayIndex];

                    // 월드 좌표 계산
                    Vector3Int tilePos = new Vector3Int(startWorldX + localX, startWorldY + localY, 0);

                    if (tilePalette.TryGetValue(tileId, out Tile tileAsset))
                    {
                        // 2. 바다/육지 Tilemap에 따라 타일 정보를 분리
                        if (tileId <= MapConstants.TILE_SEA_DEEP) // 0, 1, 2는 바다
                        {
                            seaPositions.Add(tilePos);
                            seaTiles.Add(tileAsset);
                        }
                        else // 3 이상은 육지
                        {
                            landPositions.Add(tilePos);
                            landTiles.Add(tileAsset);
                        }
                    }
                }
            }

            // 3. Tilemap에 일괄 배치 (SetTiles 사용!)
            // Tilemap은 GameObject에 연결되어 있어야 합니다.
            if (seaPositions.Count > 0)
            {
                TilemapManager.Instance.oceanTilemap.SetTiles(seaPositions.ToArray(), seaTiles.ToArray());
            }
            if (landPositions.Count > 0)
            {
                TilemapManager.Instance.landTilemap.SetTiles(landPositions.ToArray(), landTiles.ToArray());
            }
        }

        // 육지 Tilemap에 Collider가 붙어 있다면 수동으로 업데이트
        if (TilemapManager.Instance.landTilemap.GetComponent<TilemapCollider2D>() != null)
        {
            // 400개 청크를 한 번에 그렸으므로, 콜라이더도 한 번에 업데이트합니다.
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