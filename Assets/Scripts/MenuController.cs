using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject btnAgain;
    public GameObject btnNext;

    public GameObject anim1;
    public GameObject anim2;
    public GameObject anim3;
    public TextMesh stage;

    private const float MAX_TIME_CLICK = 1f;
    private bool click = false;
    private float tempoClick = MAX_TIME_CLICK;
    private string lastClickedTag = "";
    private const float DELAY_CLICK = 0.1f;
    private const float DELAY_ANIM_AMPLI = 0.1f;


    void OnEnable()
    {
        int stageNumber = PlayerPrefs.GetInt("stageNumber", 0);
        string text = "";
        if(stageNumber < 10)
        {
            text = "STAGE 0" + stageNumber;
        }
        else
        {
            text = "STAGE " + stageNumber;
        }
        stage.text = text;
        StartCoroutine(delayAnimaBonus());
    }

    IEnumerator delayAnimaBonus()
    {
        yield return new WaitForSeconds(0.5f);
        animaBonus();
        
    }

    void animaBonus()
    {
        int numBonus = Camera.main.GetComponent<RouteController>().numBonus;
        for (int i = 1; i <= numBonus; i++ )
        {
            if(i == 1)
            {
                StartCoroutine(delayAmpli1(anim1.GetComponent<Animator>(), DELAY_ANIM_AMPLI * i));
            } else if (i == 2)
            {
                StartCoroutine(delayAmpli2(anim2.GetComponent<Animator>(), DELAY_ANIM_AMPLI * i));
            }
            if (i == 3)
            {
                StartCoroutine(delayAmpli3(anim3.GetComponent<Animator>(), DELAY_ANIM_AMPLI * i));
            }
            
        }

    }

    IEnumerator delayAmpli1(Animator anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("aparece");
    }

    IEnumerator delayAmpli2(Animator anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("aparece");
    }

    IEnumerator delayAmpli3(Animator anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("aparece");
    }

    void Update()
    {
        if (click)
        {
            tempoClick -= Time.deltaTime;
            if (tempoClick <= 0f)
            {
                clickUp(lastClickedTag);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "btnNext")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnNext.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "btnAgain")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnAgain.GetComponent<Animator>().Play("click");
                }
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                clickUp(hit.collider.gameObject.tag);
            }
        }
    }

    private void clickUp(string tag)
    {

        if (tag == "btnNext")
        {
            click = false;
            btnNext.GetComponent<Animator>().Play("clickUp");
            StartCoroutine(delayClick(tag));
        }
        if (tag == "btnAgain")
        {
            click = false;
            btnAgain.GetComponent<Animator>().Play("clickUp");
            StartCoroutine(delayClick(tag));
        }

    }

    IEnumerator delayClick(string tag)
    {
        yield return new WaitForSeconds(DELAY_CLICK);

        if (tag == "btnNext")
        {
            Camera.main.GetComponent<StageSwitch>().next();
        }
        if (tag == "btnAgain")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
