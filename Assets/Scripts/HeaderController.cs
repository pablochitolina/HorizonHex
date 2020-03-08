using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderController : MonoBehaviour
{

    public TextMesh txtStage;
    void Start()
    {
        int stageNumber = PlayerPrefs.GetInt("stageNumber", 0);
        string text = "";
        if (stageNumber < 10)
        {
            text = "STAGE 0" + stageNumber;
        }
        else
        {
            text = "STAGE " + stageNumber;
        }
        txtStage.text = text;
    }

}
