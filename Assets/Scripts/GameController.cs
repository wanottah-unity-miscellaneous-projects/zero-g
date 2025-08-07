
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

    // console control modes
    private int _consoleState;




    private void Awake()
    {
        instance = this;
    }


    
    private void Start()
    {
        Initialise();

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
            // display the minimap depending upon its previous state
            _consoleState = -_consoleState;

            MinimapCameraController.instance.SetConsoleState(_consoleState);

            // pause / unpause the game
            PauseUnpause();
        }
    }


    private void Initialise()
    {
        // get the current state the the mini map display
        _consoleState = MinimapCameraController.instance.consoleState;
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
        // if the game is pawzed
        if (gamePawzed)
        {
            // then close the pawz screen
            pawzScreen.SetActive(false);
        }

        // open the options screen
        optionsScreen.SetActive(true);
    }


    // if we are closing the options screen 
    public void CloseOptions()
    {
        // if the game is pawzed
        if (gamePawzed)
        {
            // close the options screen
            optionsScreen.SetActive(false);

            // load the pawz screen
            pawzScreen.SetActive(true);
        }
    }


} // end of class
