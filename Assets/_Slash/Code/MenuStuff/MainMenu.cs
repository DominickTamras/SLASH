using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
//Credit to Brackeys youtube tutorial on Audio managers, as the majority of this code and learning how to use it was made by him.


public partial class MainMenu : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;
    public bool isReturn;
    public bool isCredits;
    public bool isReset;
    public AudioSource audio;

    private bool mouseIn;
    public GameObject holdCred;
    public GameObject holdMenu;
    private Vector3 ogScale;
    private void Start()
    {
        ogScale = transform.localScale;   
    }

    public void StartMove()
    {
        SceneManager.LoadScene("Tut");
        audio.Play();
    }

    public void Credits()
    {
        if (holdCred && holdMenu != null)

        audio.Play();
        transform.localScale = ogScale;
        holdCred.SetActive(true);
        holdMenu.SetActive(false);
    }

    public void StartReturn()
    {
        if(holdCred && holdMenu != null)
        transform.localScale = ogScale;
        holdCred.SetActive(false);
        holdMenu.SetActive(true);
        audio.Play();

    }

    public void StartReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        audio.Play();
    }

    public void StartQuit()
    {
        Application.Quit();
        audio.Play();
    }

    public void StartPause()
    {
        Time.timeScale = 0;
    }

    public void OnEnter()
    {
        if(mouseIn == false)
        transform.DOScale(new Vector2(transform.localScale.x + 0.25f, transform.localScale.y + 0.25f), 0.2f);
        mouseIn = true;
    }

    public void OnExit()
    {
        if(mouseIn == true)
        transform.DOScale(new Vector2(ogScale.x, ogScale.y), 0.2f);
        mouseIn = false;
    }



}