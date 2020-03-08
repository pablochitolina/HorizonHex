using UnityEngine;
using System.Collections.Generic;

public class SonarHexController : MonoBehaviour
{

    private TilesController scriptTiles;
    private RouteController scriptRouteController;
    private string setorCor = "";


    void Start()
    {
        scriptTiles = Camera.main.GetComponent<TilesController>();
        scriptRouteController = Camera.main.GetComponent<RouteController>();
    }

    public void Anima(System.Action<SonarHexBean> callback, GameObject objOrigem, bool mostraSonar)
    {
        bool isTile = false, isBase = false, isHit = false;
        ActiveSonarBean sonar = null;
        foreach (TileBean tb in scriptTiles.tiles)
        {
            if (Vector2.Distance(tb.getPos(), transform.position) < 0.1f)//ve se é pos do row de sonar
            {
                isTile = tb.getType() == TileBean.TILE_ACTIVE;
                if (tb.getType() == TileBean.BASE_MOVEL || tb.getType() == TileBean.BASE_INIT || tb.getType() == TileBean.BASE_END)
                {
                    isBase = true;
                    if (isValidHit(objOrigem, tb.getGameObject(), tb.getType()))
                    {
                        isHit = true;
                        scriptRouteController.addConnection(objOrigem, tb.getGameObject(), gameObject.tag, setorCor);
                        break;
                        //Debug.Log("origem " + objOrigem.name + " conectou com " + tb.getGameObject().name + " na cor " + gameObject.tag);
                    }
                }
            }
        }
        //if (isTile && !isBase && mostraSonar) gameObject.GetComponent<Animator>().Play("base_sonar");

        if (isTile && !isBase && mostraSonar && objOrigem.GetComponent<Drag>().allowAnima)
        {
            GameObject sonarCopy = null;
            if (gameObject.tag == "redSonar") sonarCopy = Instantiate(scriptTiles.redSonar);
            if (gameObject.tag == "blueSonar") sonarCopy = Instantiate(scriptTiles.blueSonar);
            if (gameObject.tag == "greenSonar") sonarCopy = Instantiate(scriptTiles.greenSonar);
            
            if(sonarCopy != null)
            {
                Color color = sonarCopy.GetComponent<SpriteRenderer>().color;
                color.a = 0.5f;
                sonarCopy.GetComponent<SpriteRenderer>().color = color;
                sonarCopy.SetActive(true);
                sonarCopy.transform.position = transform.position;
                sonarCopy.gameObject.tag = "sonarDestroy";
                sonarCopy.transform.localScale = new Vector3(0.8660256f/2, 0.5f, -1f);
                sonarCopy.GetComponent<Animator>().Play("base_sonar");
                Destroy(sonarCopy, 0.5f);
            }
            
        }

        if (isTile && !isBase && !isHit) sonar = new ActiveSonarBean(gameObject.transform.position, gameObject.tag);
  
        callback(new SonarHexBean(isTile && !isBase, isHit, sonar));
    }

    private bool isValidHit(GameObject objOrigem, GameObject objHit, string type)
    {
        setorCor = "";
        Vector2 posOrigem = objOrigem.transform.position;
        string ru = "", r = "", rd = "", ld = "", l = "", lu = "";
        if (type == TileBean.BASE_MOVEL)
        {
            BaseMovelController script = objHit.GetComponent<BaseMovelController>();
            ru = script.ru;
            r = script.r;
            rd = script.rd;
            ld = script.ld;
            l = script.l;
            lu = script.lu;
        }
        if (type == TileBean.BASE_END || type == TileBean.BASE_INIT)
        {
            BaseFixaController script = objHit.GetComponent<BaseFixaController>();
            ru = script.ru;
            r = script.r;
            rd = script.rd;
            ld = script.ld;
            l = script.l;
            lu = script.lu;
        }
        if (isBaseMovelRU(posOrigem) && isSameLabel(ru))
        {
            setorCor = "ld" + ru;
            return true;
        }
        if (isBaseMovelR(posOrigem) && isSameLabel(r))
        {
            setorCor = "l" + r;
            return true;
        }
        if (isBaseMovelRD(posOrigem) && isSameLabel(rd))
        {
            setorCor = "lu" + rd;
            return true;
        }
        if (isBaseMovelLD(posOrigem) && isSameLabel(ld))
        {
            setorCor = "ru" + ld;
            return true;
        }
        if (isBaseMovelL(posOrigem) && isSameLabel(l))
        {
            setorCor = "r" + l;
            return true;
        }
        if (isBaseMovelLU(posOrigem) && isSameLabel(lu))
        {
            setorCor = "rd" + lu;
            return true;
        }
        return false;
    }

    private bool isSameLabel(string label){
        return (gameObject.tag == "redSonar" && label == "r") || (gameObject.tag == "greenSonar" && label == "g") || (gameObject.tag == "blueSonar" && label == "b");
    }

    private bool isBaseMovelRU(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x > transform.position.x && posBaseMovel.y > transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelR(Vector2 posBaseMovel)
    {
        return posBaseMovel.x > transform.position.x && sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelRD(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x > transform.position.x && posBaseMovel.y < transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelLD(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x < transform.position.x && posBaseMovel.y < transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool isBaseMovelL(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x < transform.position.x && sameRowX(posBaseMovel.y));
    }

    private bool isBaseMovelLU(Vector2 posBaseMovel)
    {
        return (posBaseMovel.x < transform.position.x && posBaseMovel.y > transform.position.y) && !sameRowX(posBaseMovel.y);
    }

    private bool sameRowX(float y)
    {
        return (y < transform.position.y + 0.1f && y > transform.position.y - 0.1f);
    }
}
