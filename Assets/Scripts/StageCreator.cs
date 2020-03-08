using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageCreator : MonoBehaviour
{
    
    private TilesController tilesController;
    private RouteController routeController;
    private ActiveSonarController activeSonarController;
    public GameObject base_init;
    public GameObject base_end;
    public Transform basesMoveis;
    public Transform amplificadores;
    public GameObject backBase;
    public Tilemap tilemapActive;
    public GameObject parent;

    void OnEnable()
    {
        tilesController = Camera.main.GetComponent<TilesController>();
        routeController = Camera.main.GetComponent<RouteController>();
        activeSonarController = Camera.main.GetComponent<ActiveSonarController>();

        routeController.baseEnd = base_end;
        routeController.baseInit = base_init;

        tilesController.tiles.Add(new TileBean(base_init, TileBean.BASE_INIT));
        tilesController.tiles.Add(new TileBean(base_end, TileBean.BASE_END));

        activeSonarController.parent = parent;

        foreach (Transform go in basesMoveis)
        {
            go.gameObject.GetComponent<Drag>().parent = parent;
            go.gameObject.GetComponent<BaseMovelController>().parent = parent;
            tilesController.tiles.Add(new TileBean(go.gameObject, TileBean.BASE_MOVEL));
            GameObject bbase = Instantiate(backBase);
            bbase.transform.SetParent(backBase.transform.parent);
            bbase.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
            bbase.transform.position = new Vector3(go.position.x, go.position.y, 1f);
            bbase.SetActive(true);
        }
        foreach (Transform a in amplificadores)
        {
            GameObject ampli = a.GetChild(0).gameObject;
            tilesController.tiles.Add(new TileBean(ampli, TileBean.AMPLIFIC));
            activeSonarController.posAmplifics.Add(ampli);
        }

        
        foreach (Vector3Int localPlace in tilemapActive.cellBounds.allPositionsWithin)
        {
            if (tilemapActive.HasTile(localPlace))
            {
                Vector2 place = tilemapActive.CellToWorld(localPlace);
                GameObject obj = new GameObject();
                obj.transform.SetParent(parent.transform);
                obj.transform.position = place;
                tilesController.tiles.Add(new TileBean(obj, TileBean.TILE_ACTIVE));
            }
        }
    }
}
