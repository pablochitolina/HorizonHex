using UnityEngine;
using System.Collections.Generic;


public class TilesController : MonoBehaviour
{
    
    public float distance = 1;
    public List<TileBean> tiles = new List<TileBean>();
    private RouteController routeController;
    public GameObject redSonar;
    public GameObject greenSonar;
    public GameObject blueSonar;

    void Start()
    {
        routeController = gameObject.GetComponent<RouteController>();    
    }

    public bool ChangePosition(Vector2 newPos, GameObject baseMovel, string type, Vector2 posInit)
    {
        bool change = true;
        for(int i = 0; i < tiles.Count; i++)
        {
            //se tem outra base na posição manda base antiga para posstart
            if(Vector2.Distance(tiles[i].getPos(), newPos) < 0.1f && tiles[i].getType() == type && baseMovel.GetInstanceID() != tiles[i].getGameObject().GetInstanceID())
            {
                Vector2 start = tiles[i].getGameObject().GetComponent<BaseMovelController>().posStart;
                //routeController.limpaRoutes(tiles[i].getGameObject().GetHashCode());
                tiles[i].getGameObject().transform.position = start;
            }
            //se manda pra base fixa faz voltar para pos init
            if (Vector2.Distance(tiles[i].getPos(), newPos) < 0.1f && (tiles[i].getType() == TileBean.BASE_END || tiles[i].getType() == TileBean.BASE_INIT || tiles[i].getType() == TileBean.AMPLIFIC))
            {
                change = false;
                baseMovel.transform.position = posInit;
            }
            //continua na mesma base
            if (Vector2.Distance(newPos, posInit) < 0.1f && tiles[i].getGameObject().GetHashCode() == baseMovel.GetHashCode())
            {
                change = false;
                baseMovel.transform.position = posInit;
            }
        }
        return change;
    }
}
