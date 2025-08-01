﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;


    public float waitAfterDying = 2f;

    [HideInInspector] public bool levelEnding;




    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitialiseCursor();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }


    private void InitialiseCursor()
    {
        // lock the mouse cursor to the game window and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void PlayerDied()
    {
        StartCoroutine(PlayerDiedCo());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);

        GameData.gameDataInstance.currentHealth = 100;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void PauseUnpause()
    {
        if (UIController.uiController.pauseScreen.activeInHierarchy)
        {
            UIController.uiController.pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;

            PlayerController.instance.footstepFast.Play();
            PlayerController.instance.footstepSlow.Play();
        } 
        
        else
        {
            UIController.uiController.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0f;

            PlayerController.instance.footstepFast.Stop();
            PlayerController.instance.footstepSlow.Stop();
        }
    }


} // end of class
