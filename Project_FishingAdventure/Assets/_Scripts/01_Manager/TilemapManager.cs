using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance;

    public Tilemap oceanTilemap;
    public Tilemap landTilemap;
    public Tilemap oceanBoundaryTilemap;
    public Tilemap landBoundaryTilemap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        oceanBoundaryTilemap.color = ImageUtils.GetTransparencyColor(oceanBoundaryTilemap.color, 0);
        landBoundaryTilemap.color = ImageUtils.GetTransparencyColor(landBoundaryTilemap.color, 0);
    }
}