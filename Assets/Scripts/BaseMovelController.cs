using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BaseMovelController : MonoBehaviour
{
    private const float DELAY_ROWS = 0.05f;
    private const float DELAY_ANIMATOR = 0.1f;

    public GameObject rb1;
    public GameObject rb2;
    public GameObject rr1;
    public GameObject rr2;
    public GameObject rg1;
    public GameObject rg2;

    private List<SonarHexController> listRB1 = new List<SonarHexController>();
    private List<SonarHexController> listRB2 = new List<SonarHexController>();
    private List<SonarHexController> listRR1 = new List<SonarHexController>();
    private List<SonarHexController> listRR2 = new List<SonarHexController>();
    private List<SonarHexController> listRG1 = new List<SonarHexController>();
    private List<SonarHexController> listRG2 = new List<SonarHexController>();

    public string ru = "";
    public string r = "";
    public string rd = "";
    public string ld = "";
    public string l = "";
    public string lu = "";
  
    public Vector2 posStart;
    public Vector2 posInit;

    private int rotation = 0;

    private bool isRb1Anima = false;
    private bool isRb2Anima = false;
    private bool isRr1Anima = false;
    private bool isRr2Anima = false;
    private bool isRg1Anima = false;
    private bool isRg2Anima = false;

    private List<ActiveSonarBean> listSonarRb1 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRb2 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRg1 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRg2 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRr1 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRr2 = new List<ActiveSonarBean>();

    private bool deuHitRb1 = false;
    private bool deuHitRb2 = false;
    private bool deuHitRg1 = false;
    private bool deuHitRg2 = false;
    private bool deuHitRr1 = false;
    private bool deuHitRr2 = false;

    public GameObject animator;

    private ActiveSonarController scriptActiveSonarController;

    private RouteController routeController;

    public GameObject parent;

    void Start()
    {
        routeController = Camera.main.GetComponent<RouteController>();
        scriptActiveSonarController = Camera.main.GetComponent<ActiveSonarController>();
        rb1.SetActive(true);
        foreach(SonarHexController t in rb1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)
                listRB1.Add(t);
        }
        rb2.SetActive(true);
        foreach (SonarHexController t in rb2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)
                listRB2.Add(t);
        }
        rr1.SetActive(true);
        foreach (SonarHexController t in rr1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)
                listRR1.Add(t);
        }
        rr2.SetActive(true);
        foreach (SonarHexController t in rr2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)
                listRR2.Add(t);
        }
        rg1.SetActive(true);
        foreach (SonarHexController t in rg1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)
                listRG1.Add(t);
        }
        rg2.SetActive(true);
        foreach (SonarHexController t in rg2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)
                listRG2.Add(t);
        }
    }

    public void playAnim(string name)
    {
        GameObject objInstance = Instantiate(animator);
        objInstance.transform.SetParent(parent.transform);
        objInstance.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
        objInstance.transform.position = transform.position;
        objInstance.gameObject.tag = "sonarDestroy";
        objInstance.GetComponent<Animator>().Play(name);
    }

    public void Rotate(System.Action callback, GameObject copy, float delay)
    {
        transform.position = posInit;
        mudaLabels();
        StartCoroutine(rotate(callback, copy, delay));
    }

    IEnumerator rotate(System.Action callback, GameObject copy, float delay)
    {
        int iteracoes = 10;
        float passo = delay / iteracoes;
        for(int i = 0; i < iteracoes; i++)
        {
            yield return new WaitForSeconds(passo);
            rotation += (60 / iteracoes) * (-1);
            transform.eulerAngles = Vector3.forward * rotation;
            copy.transform.eulerAngles = Vector3.forward * rotation;
        }
        callback();
    }

    private void mudaLabels()
    {
        string aux_ru = ru;
        ru = lu;
        lu = l;
        l = ld;
        ld = rd;
        rd = r;
        r = aux_ru;
    }

    public void MouseUp(System.Action callback, bool mostraSonar)
    {
        isRb1Anima = true;
        isRb2Anima = true;
        isRr1Anima = true;
        isRr2Anima = true;
        isRg1Anima = true;
        isRg2Anima = true;
        deuHitRb1 = false;
        deuHitRb2 = false;
        deuHitRg1 = false;
        deuHitRg2 = false;
        deuHitRr1 = false;
        deuHitRr2 = false;
        listSonarRb1 = new List<ActiveSonarBean>();
        listSonarRb2 = new List<ActiveSonarBean>();
        listSonarRg1 = new List<ActiveSonarBean>();
        listSonarRg2 = new List<ActiveSonarBean>();
        listSonarRr1 = new List<ActiveSonarBean>();
        listSonarRr2 = new List<ActiveSonarBean>();
        StartCoroutine(Sonar(callback, mostraSonar));
    }

    IEnumerator Sonar(System.Action callback, bool mostraSonar)
    {
        yield return new WaitForSeconds(DELAY_ANIMATOR);
        StartCoroutine(Anima(callback, mostraSonar));
    }

    IEnumerator Anima(System.Action callback, bool mostraSonar)
    {
        for (int i = 0; i < listRB1.Count; i++)
        {
            if (isRb1Anima)
            {
                listRB1[i].Anima(callbackRb1, gameObject, mostraSonar);
            }
            if (isRb2Anima)
            {
                listRB2[i].Anima(callbackRb2, gameObject, mostraSonar);
            }
            if (isRr1Anima)
            {
                listRR1[i].Anima(callbackRr1, gameObject, mostraSonar);
            }
            if (isRr2Anima)
            {
                listRR2[i].Anima(callbackRr2, gameObject, mostraSonar);
            }
            if (isRg1Anima)
            {
                listRG1[i].Anima(callbackRg1, gameObject, mostraSonar);
            }
            if (isRg2Anima)
            {
                listRG2[i].Anima(callbackRg2, gameObject, mostraSonar);
            }
            if (isRb1Anima || isRb2Anima || isRr1Anima || isRr2Anima || isRg1Anima || isRg2Anima) yield return new WaitForSeconds(DELAY_ROWS);
        }
        if (deuHitRb1)
        {
            foreach(ActiveSonarBean sonar in listSonarRb1)
            {
                scriptActiveSonarController.addSonar(sonar);
            }
        }
        if (deuHitRb2)
        {
            foreach (ActiveSonarBean sonar in listSonarRb2)
            {
                scriptActiveSonarController.addSonar(sonar);
            }
        }
        if (deuHitRr1)
        {
            foreach (ActiveSonarBean sonar in listSonarRr1)
            {
                scriptActiveSonarController.addSonar(sonar);
            }
        }
        if (deuHitRr2)
        {
            foreach (ActiveSonarBean sonar in listSonarRr2)
            {
                scriptActiveSonarController.addSonar(sonar);
            }
        }
        if (deuHitRg1)
        {
            foreach (ActiveSonarBean sonar in listSonarRg1)
            {
                scriptActiveSonarController.addSonar(sonar);
            }
        }
        if (deuHitRg2)
        {
            foreach (ActiveSonarBean sonar in listSonarRg2)
            {
                scriptActiveSonarController.addSonar(sonar);
            }
        }
        callback();
    }

    public void callbackRb1(SonarHexBean sonarHex)
    {
        isRb1Anima = sonarHex.isAnima();
        deuHitRb1 = sonarHex.isHit();
        if(sonarHex.getSonar() != null)
            listSonarRb1.Add(sonarHex.getSonar());
    }
    public void callbackRb2(SonarHexBean sonarHex)
    {
        isRb2Anima = sonarHex.isAnima();
        deuHitRb2 = sonarHex.isHit();
        if (sonarHex.getSonar() != null)
            listSonarRb2.Add(sonarHex.getSonar());
    }
    public void callbackRr1(SonarHexBean sonarHex)
    {
        isRr1Anima = sonarHex.isAnima();
        deuHitRr1 = sonarHex.isHit();
        if (sonarHex.getSonar() != null)
            listSonarRr1.Add(sonarHex.getSonar());
    }
    public void callbackRr2(SonarHexBean sonarHex)
    {
        isRr2Anima = sonarHex.isAnima();
        deuHitRr2 = sonarHex.isHit();
        if (sonarHex.getSonar() != null)
            listSonarRr2.Add(sonarHex.getSonar());
    }
    public void callbackRg1(SonarHexBean sonarHex)
    {
        isRg1Anima = sonarHex.isAnima();
        deuHitRg1 = sonarHex.isHit();
        if (sonarHex.getSonar() != null)
            listSonarRg1.Add(sonarHex.getSonar());
    }
    public void callbackRg2(SonarHexBean sonarHex)
    {
        isRg2Anima = sonarHex.isAnima();
        deuHitRg2 = sonarHex.isHit();
        if (sonarHex.getSonar() != null)
            listSonarRg2.Add(sonarHex.getSonar());
    }
}
