using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTile : MonoBehaviour
{

    [System.Serializable]
    public class TileArray
    {
        [Tooltip("First tile used as detector for sequence of following animation frames")]
        public TileBase[] tiles;
    }

    [SerializeField] private TileArray[] Tiles;
    [SerializeField] private string[] beats;
    [SerializeField] private string TilemapName = "Walls"; // Fairly weak assumption

    [Tooltip("Per frame")]
    [SerializeField] private float Speed;

    private Tilemap Tilemap;

    private void Awake()
    {
        GameObject obj = GameObject.Find(TilemapName);
        if (obj == null) Debug.Log("ERROR: Object of name \"" + TilemapName + "\" not found");
        Tilemap = obj.GetComponent<Tilemap>();
        if (Tilemap == null) Debug.Log("ERROR: Object of name \"" + TilemapName + "\" does not have Tilemap component");
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += OnTileChange;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= OnTileChange;
    }

    private Vector3Int GetTilePos()
    {
        return Tilemap.WorldToCell(transform.position);
    }

    private void OnTileChange(string beat)
    {

        for (int j = 0; j < beats.Length; j++)
        {
            if (beats[j] != beat) continue;

            Vector3Int pos = GetTilePos();
            TileBase currTile = Tilemap.GetTile(pos);

            for (int i = 0; i < Tiles.Length; i++)
            {
                if (currTile == Tiles[i].tiles[0])
                {
                    StartCoroutine(TileChangeSequence(i));
                }
            }
        }
    }

    private IEnumerator TileChangeSequence(int i)
    {
        Vector3Int pos = GetTilePos();
        for (int j = 1; j < Tiles[i].tiles.Length; j++)
        {
            Tilemap.SetTile(pos, Tiles[i].tiles[j]);

            Vector3 worldPos = Tilemap.GetCellCenterWorld(pos);
            Bounds bounds = new Bounds(worldPos, Vector3.one);

            AstarPath.active.UpdateGraphs(bounds);

            yield return new WaitForSeconds(Speed);
        }
    }

}
