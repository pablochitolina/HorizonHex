using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]

public class Drag : MonoBehaviour
{
    private Vector3 offset;

    private TilesController scriptTiles;
    private BaseMovelController scriptBaseMovelController;
    private RouteController routeController;
    private int numReturnedCallback = 0;
    private int numAddCallback = 0;
    public GameObject baseMovel;
    private Animator animBaseMovel;
    private GameObject baseMovelCopy;
    private bool isDraged = false;
    private bool showingCopy = false;
    private int contActions = 0;
    public GameObject parent;
    private const float DELAY_ROTATE = 0.1f;
    public bool allowAnima = false;

    void Start()
    {
        scriptTiles = Camera.main.GetComponent<TilesController>();
        routeController = Camera.main.GetComponent<RouteController>();
        scriptBaseMovelController = gameObject.GetComponent<BaseMovelController>();
        scriptBaseMovelController.posStart = transform.position;
        animBaseMovel = baseMovel.GetComponent<Animator>();
        //copy
        Color tempColor = baseMovel.GetComponent<SpriteRenderer>().color;
        tempColor.a = 0.3f;
        baseMovelCopy = Instantiate(baseMovel);
        baseMovelCopy.SetActive(false);
        baseMovelCopy.transform.position = transform.position;
        baseMovelCopy.transform.SetParent(parent.transform);
        baseMovelCopy.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
        baseMovelCopy.GetComponent<SpriteRenderer>().color = tempColor;
    }

    void OnMouseDown()
    {
        if (routeController.isPause) return;

        if(contActions == 2)
        {
            contActions = 0;
        }
        if (contActions == 0)
        {
            allowAnima = false;
            routeController.resetConnections();
            if(timeoutAnima <= 0) animBaseMovel.Play("zoom_in");
            timeoutAnima = DELAY_ANIMA;
            contActions = 1;
            scriptBaseMovelController.posInit = transform.position;
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            isDraged = false;
            showingCopy = false;
        }
    }

    void OnMouseDrag()
    {
        if (contActions == 1)
        {
            timeoutAnima = DELAY_ANIMA;
            Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = new Vector2(curPosition.x, curPosition.y);
            if(Vector2.Distance(scriptBaseMovelController.posInit, transform.position) >= 0.1f)
            {
                isDraged = true;
            }
            if (isDraged && !showingCopy)
            {
                showingCopy = true;
                baseMovelCopy.transform.position = new Vector3(scriptBaseMovelController.posInit.x, scriptBaseMovelController.posInit.y, 0.5f);
                baseMovelCopy.SetActive(true);
            }
        }
        
    }

    void OnMouseUp()
    {
        if (contActions == 1)
        {
            contActions = 0;
            allowAnima = true;
            baseMovelCopy.SetActive(false);
            showingCopy = false;
            if (!isDraged)
            {
                scriptBaseMovelController.Rotate(callbackRotate, baseMovelCopy, DELAY_ROTATE);
            }
            else
            {
                Vector2 fixedPosition = scriptBaseMovelController.posStart;
                float distance = 1;
                foreach (TileBean tile in scriptTiles.tiles)
                {
                    if (Vector2.Distance(transform.position, tile.getPos()) < distance && tile.getType() == TileBean.TILE_ACTIVE)
                    {
                        distance = Vector2.Distance(transform.position, tile.getPos());
                        fixedPosition = tile.getPos();
                    }
                }
                transform.position = fixedPosition;
                delayAnima(scriptTiles.ChangePosition(fixedPosition, gameObject, TileBean.BASE_MOVEL, scriptBaseMovelController.posInit));
            }
        }
    }

    private const float DELAY_ANIMA = 0.5f;
    private float timeoutAnima = -1;
    private bool anima = false;
    
    void Update()
    {
        if(timeoutAnima >= 0)
        {
            timeoutAnima -= Time.deltaTime;
            if(timeoutAnima <= 0)
            {
                AnimaSonar();
            }
        }
    }

    private void delayAnima(bool anim)
    {
        anima = anim;
        timeoutAnima = DELAY_ANIMA;
    }

    private void AnimaSonar()
    {
        animBaseMovel.Play("zoom_out");
        contActions = 2;
        numReturnedCallback = 0;
        numAddCallback = 0;
        foreach (TileBean tile in scriptTiles.tiles)
        {
            if (tile.getType() == TileBean.BASE_MOVEL)
            {
                numAddCallback++;
                tile.getGameObject().GetComponent<BaseMovelController>().MouseUp(callbackPosition, (tile.getGameObject().GetHashCode() == gameObject.GetHashCode() && anima));
            }
        }

    }

    public void callbackRotate()
    {
        delayAnima(true);
    }

    public void callbackPosition()
    {
        if (contActions == 2)
        {
            numReturnedCallback++;
            if (numAddCallback == numReturnedCallback)
            {
                StartCoroutine(delayCheck());
            }
        }
        else
        {
            numReturnedCallback = 0;
            numAddCallback = 0;
            routeController.resetConnections();
        }
    }

    IEnumerator delayCheck()
    {
        yield return new WaitForSeconds(0.2f);
        if(contActions == 2)
        {
            contActions = 0;
            routeController.checkRoute();
        }
        else
        {
            routeController.resetConnections();
        }
        
    }
}