using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class WindowCreator : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private TileBase[] windowTiles;

    private Tilemap shadowTilemap;

    private void Awake()
    {
        shadowTilemap = GetComponent<Tilemap>();
        GenerateShadowMap(); // SHould maybe be in Start
    }

    public void GenerateShadowMap()
    {
        shadowTilemap.ClearAllTiles();

        BoundsInt bounds = wallTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = wallTilemap.GetTile(pos);

            if (tile == null)
                continue;

            if (IsWindow(tile))
                continue;

            shadowTilemap.SetTile(pos, tile);

            // Copy rotation / flip / scale
            shadowTilemap.SetTransformMatrix(
                pos,
                wallTilemap.GetTransformMatrix(pos)
            );

            shadowTilemap.SetTileFlags(
                pos,
                wallTilemap.GetTileFlags(pos)
            );
        }
    }

    private bool IsWindow(TileBase tile)
    {
        foreach (TileBase window in windowTiles)
        {
            if (tile == window)
                return true;
        }

        return false;
    }
}
