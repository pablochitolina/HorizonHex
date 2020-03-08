using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]

public class Click : MonoBehaviour
{
    public Animator animator;

    private const float MAX_TIME_CLICK = 1f;
    private bool click = false;
    private float tempoClick = MAX_TIME_CLICK;
    private const float DELAY_CLICK = 0.1f;
    private bool isMenuInicial = false;
    private string sceneName = "";

    private void Update()
    {
        if (click)
        {
            tempoClick -= Time.deltaTime;
            if (tempoClick <= 0f)
            {
                clickUp();
            }
        }
    }

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        isMenuInicial = sceneName == "const_menu";
    }

    void OnMouseDown()
    {
        click = true;
        animator.Play("click");
        
    }

    void OnMouseUp()
    {
       if(click) clickUp();
    }

    private void clickUp()
    {
        click = false;
        animator.Play("clickUp");
        StartCoroutine(delayClick(tag));
    }

    IEnumerator delayClick(string tag)
    {
        yield return new WaitForSeconds(DELAY_CLICK);
        string name = "";
        if (isMenuInicial)
        {
            name = gameObject.name;
        }
        else
        {
            PlayerPrefs.SetInt("stageNumber", int.Parse(gameObject.name));
            PlayerPrefs.SetString("sceneName", sceneName);
            name = sceneName + "_stg";
        }
        SceneManager.LoadScene(name);
    }
}
