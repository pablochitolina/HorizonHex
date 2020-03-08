using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RouteController : MonoBehaviour
{
    public GameObject baseInit;
    public GameObject baseEnd;
    private ActiveSonarController scriptActiveSonarController;

    public GameObject menu;
    public GameObject footer;
    public GameObject header;

    public string stageMenu;

    public bool isPause = false;

    private List<ConnectionBean> connections = new List<ConnectionBean>();
    private List<GameObject> anim = new List<GameObject>();

    public int numBonus = 0;


    private void Start()
    {
        scriptActiveSonarController = gameObject.GetComponent<ActiveSonarController>();
    }

    public void checkRoute()
    {
        List<ConnectionBean> listCon = new List<ConnectionBean>();
        List<HashColor> hashColorInit = new List<HashColor>();
        List<HashColor> hashColorEnd = new List<HashColor>();
        List<HashColor> hashColorInitOK = new List<HashColor>();
        List<HashColor> hashColorEndOK = new List<HashColor>();
        bool connectou = false;
        numBonus = 0;

        scriptActiveSonarController.contAmplific(); //reseta numero de aplificadores

        foreach (ConnectionBean con in connections)
        {
            if (con.getHashDestino() == baseInit.GetHashCode())
            {
                hashColorInit.Add(new HashColor(con.getHashOrigem(), con.getColor()));
            }
            if (con.getHashDestino() == baseEnd.GetHashCode())
            {
                hashColorEnd.Add(new HashColor(con.getHashOrigem(), con.getColor()));
            }
        }

        foreach (HashColor colori in hashColorInit)
        {
            foreach (HashColor colore in hashColorEnd)
            {
                if(colori.getColor() == colore.getColor())
                {
                    hashColorInitOK.Add(colori);
                    hashColorEndOK.Add(colore);
                }
            }
        }
        for (int cor = 0; cor < hashColorInitOK.Count; cor++)
        {
            string colorConnectionInit = hashColorInitOK[cor].getColor();
            string colorConnectionEnd = hashColorEndOK[cor].getColor();
            int hashOrigemAtual = hashColorInitOK[cor].getHash();
            int hashConnectionBaseEnd = hashColorInitOK[cor].getHash();
            bool conectouTentativa = false;

            List<Vector2> listPosLine = new List<Vector2>();

            int indexParent = 0;
            int tentativas = 0;

            //TODO melhorar performance
            while(tentativas < connections.Count)
            {
                if (connections[indexParent].getHashOrigem() == hashOrigemAtual && connections[indexParent].getColor() == colorConnectionInit) // é unico e descobre o destino da proxima con
                {
                    foreach (ConnectionBean conChild in connections) // percorre novamente o laço con
                    {
                        if (connections[indexParent].getHashDestino() == hashConnectionBaseEnd && connections[indexParent].getColor() == colorConnectionEnd) // achou fim
                        {
                            connectou = true;
                            conectouTentativa = true;
                            int contBonus = scriptActiveSonarController.getContAmplific(colorConnectionInit);
                            numBonus = contBonus > numBonus ? contBonus : numBonus;
                            break;
                        }
                        if (connections[indexParent].getHashDestino() == conChild.getHashOrigem() && conChild.getHashDestino() != connections[indexParent].getHashOrigem())
                        {
                            hashOrigemAtual = connections[indexParent].getHashDestino();
                        }
                    }
                }
                indexParent++;
                if (indexParent == connections.Count)
                {
                    indexParent = 0;
                    tentativas++;
                }
                if (connectou) break;
            }
            if (conectouTentativa)
            {
                scriptActiveSonarController.AnimSonar(colorConnectionInit);
                foreach (ConnectionBean con in connections)
                {
                    if(con.getColor() == colorConnectionInit) con.getOrigem().GetComponent<BaseMovelController>().playAnim(con.getSetorCorOrigem());
                }
            }

        }

        if (connectou)
        {
            string stageName = SceneManager.GetActiveScene().name;
            if (numBonus > PlayerPrefs.GetInt(stageName, 0))
            {
                PlayerPrefs.SetInt(stageName, numBonus);
            }
            isPause = true;
            StartCoroutine(delayMostraMenu());
            footer.SetActive(false);
            header.SetActive(false);
        }
        else
        {
            scriptActiveSonarController.AnimSonar(null);
            foreach (ConnectionBean con in connections)
            {
                con.getOrigem().GetComponent<BaseMovelController>().playAnim(con.getSetorCorOrigem());
            }
        }
    }

    IEnumerator delayMostraMenu()
    {
        yield return new WaitForSeconds(2.5f);
        menu.SetActive(true);
    }

    public void addConnection(GameObject origem, GameObject destino, string cor, string setorCor)
    {
        connections.Add(new ConnectionBean(origem, destino, cor, setorCor));
    }

    public void resetConnections()
    {
        connections = new List<ConnectionBean>();
        scriptActiveSonarController.cleanSonar();
    }

    public class HashColor
   {
        private int hash;
        private string color;

        public HashColor(int hash, string color)
        {
            this.hash = hash;
            this.color = color;
        }

        public int getHash()
        {
            return this.hash;
        }

        public string getColor()
        {
            return this.color;
        }

   }
}
