
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    // enable other scripts to access this script
    public static GameController instance;


    // time to wait after player dies before respawning player
    public float waitAfterDying = 2f;

    // when the level has been completed
    [HideInInspector] public bool levelEnding;



    // reference to options screen
    public GameObject optionsScreen;

    // reference to the pawz screen
    public GameObject pawzScreen;

    // reference to background panel
    public GameObject backgroundPanel;

    // are we playing the game
    public bool gamePawzed;


    // console states
    private const int CONSOLE_ACTIVE = 1;
    private const int CONSOLE_INACTIVE = -1;




    private void Awake()
    {
        instance = this;
    }


    
    private void Start()
    {
        InitialiseCursor();
    }


    // get player input
    private void Update()
    {
        GetPlayerInput();
    }


    private void GetPlayerInput()
    {
        // if the player presses the 'escape' key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if the options screen is not active
            if (!optionsScreen.activeInHierarchy)
            {
                // pause / unpause the game
                PauseUnpause();
            }
        }
    }


    private void InitialiseCursor()
    {
        // lock the mouse cursor to the game window and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // player has died
    public void PlayerDied()
    {
        // respawn player
        StartCoroutine(PlayerDiedCoroutine());
    }


    public IEnumerator PlayerDiedCoroutine()
    {
        // short pause before respawning player
        yield return new WaitForSeconds(waitAfterDying);

        // load the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // if the 'escape' key has been pressed, pause / unpause game play
    public void PauseUnpause()
    {
        // if the pause screen is active
        if (UIController.uiController.pauseScreen.activeInHierarchy)
        {
            // un-pawz the game
            gamePawzed = false;

            // deactivate the background
            backgroundPanel.SetActive(false);

            // hide the pause screen
            UIController.uiController.pauseScreen.SetActive(false);

            // show the mini map
            MinimapCameraController.instance.SetConsoleState(CONSOLE_ACTIVE);

            // hide and lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // unfreeze the game play
            Time.timeScale = 1f;

            // and play the player footsteps sound
            PlayerController.instance.footstepFast.Play();
            PlayerController.instance.footstepSlow.Play();
        }

        // otherwise
        else
        {
            // hide the mini map
            MinimapCameraController.instance.SetConsoleState(CONSOLE_INACTIVE);

            // pawz the game
            gamePawzed = true;

            // activate the background
            backgroundPanel.SetActive(true);

            // open the options screen
            optionsScreen.SetActive(false);

            // show the pause screen
            UIController.uiController.pauseScreen.SetActive(true);

            // unlock and show the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // stop playing the player footsteps sound
            PlayerController.instance.footstepFast.Stop();
            PlayerController.instance.footstepSlow.Stop();
        }
    }



    // if the options button is pressed
    public void OptionsButton()
    {
        // then close the pawz screen
        pawzScreen.SetActive(false);

        // open the options screen
        optionsScreen.SetActive(true);
    }


    // if we are closing the options screen 
    public void CloseOptions()
    {
        // close the options screen
        optionsScreen.SetActive(false);

        // load the pawz screen
        pawzScreen.SetActive(true);
    }


} // end of class
