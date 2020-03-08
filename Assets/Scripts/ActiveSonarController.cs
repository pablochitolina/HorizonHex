using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSonarController : MonoBehaviour
{
    private List<ActiveSonarBean> sonars = new List<ActiveSonarBean>();
    public GameObject redSonar;
    public GameObject greenSonar;
    public GameObject blueSonar;
    public List<GameObject> posAmplifics = new List<GameObject>();
    int contRed = 0;
    int contGreen = 0;
    int contBlue = 0;

    public Sprite spriteBlue;
    public Sprite spriteRed;
    public Sprite spriteGreen;
    public Sprite spriteAll;

    public GameObject parent;

    public void addSonar(ActiveSonarBean sonar)
    {
        bool contem = false;
        foreach(ActiveSonarBean son in sonars)
        {
            if(Vector2.Distance(son.getPosSonar(), sonar.getPosSonar()) < 0.1f)
            {
                contem = true;
                break;
            }
        }
        if(!contem) sonars.Add(sonar);
    }

    public void cleanSonar()
    {
        sonars = new List<ActiveSonarBean>();
        GameObject[] sonarsDestroy = GameObject.FindGameObjectsWithTag("sonarDestroy");
        foreach (GameObject sonarDestroy in sonarsDestroy)
        {
            Destroy(sonarDestroy);
        }
        foreach (GameObject pos in posAmplifics)
        {
            pos.GetComponent<SpriteRenderer>().sprite = spriteAll;
        }

    }

    public void contAmplific()
    {
        contRed = 0;
        contBlue = 0;
        contGreen = 0;
        foreach (ActiveSonarBean sonar in sonars)
        {
            foreach (GameObject pos in posAmplifics)
            {
                if (Vector2.Distance(sonar.getPosSonar(), pos.transform.position) < 0.1f)
                {
                    if (sonar.getColor() == "redSonar") contRed++;
                    if (sonar.getColor() == "greenSonar") contGreen++;
                    if (sonar.getColor() == "blueSonar") contBlue++;
                }
            }
        }
    }

    public int getContAmplific(string color)
    {
        if (color == "redSonar") return contRed;
        if (color == "greenSonar") return contGreen;
        if (color == "blueSonar") return contBlue;
        return 0;
    }

    public void AnimSonar(string color)
    {
        string cor = color;
        foreach (ActiveSonarBean sonar in sonars)
        {
            bool temAplific = false;
            GameObject ampliCor = null;
            foreach (GameObject pos in posAmplifics)
            {
                if (Vector2.Distance(pos.transform.position, sonar.getPosSonar()) < 0.1f){
                    temAplific = true;
                    ampliCor = pos;

                }
            }
            //if (temAplific) continue;
            GameObject objInstance = null;

            if (sonar.getColor() == "redSonar" && (cor != null ? sonar.getColor() == cor : true))
            {
                objInstance = Instantiate(redSonar);
                objInstance.transform.SetParent(parent.transform);
                objInstance.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
                objInstance.SetActive(true);
                objInstance.transform.position = sonar.getPosSonar();
                objInstance.gameObject.tag = "sonarDestroy";

                if (temAplific && ampliCor != null)
                {
                    ampliCor.GetComponent<SpriteRenderer>().sprite = spriteRed;
                }
                else
                {
                    if (cor != null)
                    {
                        objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                        objInstance.GetComponent<Animator>().Play("anim_loop_" + contRed);
                    }
                    else
                    {
                        objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                        objInstance.GetComponent<Animator>().Play("anim_s");
                    }
                    
                }

            }
            if (sonar.getColor() == "greenSonar" && (cor != null ? sonar.getColor() == cor : true))
            {
                objInstance = Instantiate(greenSonar);
                objInstance.transform.SetParent(parent.transform);
                objInstance.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
                objInstance.SetActive(true);
                objInstance.transform.position = sonar.getPosSonar();
                objInstance.gameObject.tag = "sonarDestroy";

                if (temAplific)
                {
                    ampliCor.GetComponent<SpriteRenderer>().sprite = spriteGreen;
                }
                else
                {
                    if (cor != null)
                    {
                        objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                        objInstance.GetComponent<Animator>().Play("anim_loop_" + contGreen);
                    }
                    else
                    {
                        objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                        objInstance.GetComponent<Animator>().Play("anim_s");
                    }
                }
            }
            if (sonar.getColor() == "blueSonar" && (cor != null ? sonar.getColor() == cor : true))
            {
                objInstance = Instantiate(blueSonar);
                objInstance.transform.SetParent(parent.transform);
                objInstance.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
                objInstance.SetActive(true);
                objInstance.transform.position = sonar.getPosSonar();
                objInstance.gameObject.tag = "sonarDestroy";
                if (temAplific)
                {
                    ampliCor.GetComponent<SpriteRenderer>().sprite = spriteBlue;
                }
                else
                {
                    if (cor != null)
                    {
                        objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                        objInstance.GetComponent<Animator>().Play("anim_loop_" + contBlue);
                    }
                    else
                    {
                        objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                        objInstance.GetComponent<Animator>().Play("anim_s");
                    }
                }
                
            }
        }
        

    }
}
