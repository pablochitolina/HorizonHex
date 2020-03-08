using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public GameObject backAmpli1;
    public GameObject backAmpli2;
    public GameObject backAmpli3;
    public GameObject ampliActive;

    void Start()
    {
        string name = SceneManager.GetActiveScene().name + "-" + gameObject.name;
        int amplis = PlayerPrefs.GetInt(name, 0);
        if(amplis > 0)
        {
            GameObject objInstance1 = Instantiate(ampliActive);
            objInstance1.transform.SetParent(transform);
            objInstance1.transform.position = new Vector3(backAmpli1.transform.position.x, backAmpli1.transform.position.y, -0.2f);
            objInstance1.SetActive(true);
        }
        if (amplis > 1)
        {
            GameObject objInstance2 = Instantiate(ampliActive);
            objInstance2.transform.SetParent(transform);
            objInstance2.transform.position = new Vector3(backAmpli2.transform.position.x, backAmpli2.transform.position.y, -0.2f);
            objInstance2.SetActive(true);
        }
        if (amplis  > 2)
        {
            GameObject objInstance3 = Instantiate(ampliActive);
            objInstance3.transform.SetParent(transform);
            objInstance3.transform.position = new Vector3(backAmpli3.transform.position.x, backAmpli3.transform.position.y, -0.2f);
            objInstance3.SetActive(true);
        }
    }
}
