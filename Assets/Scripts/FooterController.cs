using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FooterController : MonoBehaviour
{
    public GameObject btnAgain;
    public GameObject btnBack;
    public GameObject btnExit;

    private const float MAX_TIME_CLICK = 1f;
    private bool click = false;
    private float tempoClick = MAX_TIME_CLICK;
    private string lastClickedTag = "";
    private const float DELAY_CLICK = 0.1f;
    public string menuBack;

    void Update()
    {
        if (click)
        {
            tempoClick -= Time.deltaTime;
            if(tempoClick <= 0f)
            {
                clickUp(lastClickedTag);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "btnBack" )
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnBack.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "btnAgain")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnAgain.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "btnExit")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnExit.GetComponent<Animator>().Play("click");
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
        
        if (tag == "btnBack")
        {
            click = false;
            btnBack.GetComponent<Animator>().Play("clickUp");
            StartCoroutine(delayClick(tag));
        }
        if (tag == "btnAgain")
        {
            click = false;
            btnAgain.GetComponent<Animator>().Play("clickUp");
            StartCoroutine(delayClick(tag));
        }
        if (tag == "btnExit")
        {
            click = false;
            btnExit.GetComponent<Animator>().Play("clickUp");
            StartCoroutine(delayClick(tag));
        }
    }

    IEnumerator delayClick(string tag)
    {
        yield return new WaitForSeconds(DELAY_CLICK);

        if (tag == "btnBack")
        {
            SceneManager.LoadScene(menuBack);
        }
        if (tag == "btnAgain")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (tag == "btnExit")
        {
            Application.Quit();
        }
    }
}
