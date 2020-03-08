using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstelattionsController : MonoBehaviour
{

    public const string AQUARIUS = "aquarius";

    public int sizeAquarius;
    public GameObject[] listConst;
    public TextMesh[] listText;
    
    void Start()
    {
        for(int i = 0; i < listConst.Length; i++)
        {
            if(listConst[i].gameObject.name == AQUARIUS)
            {
                listText[i].text = contAmpli(AQUARIUS, sizeAquarius) + "/" + (sizeAquarius * 3);
            }
        }
    }

    private int contAmpli(string name, int tam)
    {
        int cont = 0;
        for (int j = 1; j <= sizeAquarius; j++)
        {
            cont += PlayerPrefs.GetInt(AQUARIUS + "-" + j, 0);
        }
        return cont;
    }

}
