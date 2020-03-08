using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSwitch : MonoBehaviour
{
    public GameObject[] stagesAquarius;
    void Start()
    {
        string sceneName = PlayerPrefs.GetString("sceneName");

        if (sceneName == ConstelattionsController.AQUARIUS)
        {
            int stage = PlayerPrefs.GetInt("stageNumber", 0) - 1;
            stagesAquarius[stage].SetActive(true);
        }
        
    }

    public void next()
    {
        string sceneName = PlayerPrefs.GetString("sceneName");
        if (sceneName == ConstelattionsController.AQUARIUS)
        {
            int stage = PlayerPrefs.GetInt("stageNumber", 0);
            if(stage < stagesAquarius.Length)
            {
                PlayerPrefs.SetInt("stageNumber", ++stage);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.LoadScene(sceneName + "_stg");
            }
        }

    }
}
