
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public string firstLevel;

    // reference to the menu 'continue' button
    public GameObject continueButton;



    void Start()
    {
        // if 'playerprefs' has a 'continue' key entry
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            // if the 'playerprefs' key entry does not have a value
            if (PlayerPrefs.GetString("CurrentLevel") == "")
            {
                // disable the 'continue' button
                continueButton.SetActive(false);
            }
        }

        // otherwise
        // if 'playerprefs' doesn't have a 'continue' key entry
        else
        {
            // disable the 'continue' button
            continueButton.SetActive(false);
        }
    }


    // if the player has clicked the 'continue' button
    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }


    // if the player has clicked the 'play'button
    // start playing the game
    public void PlayGame()
    {
        // initialise the game data
        InitialiseGameData();


        // load the 'level1' game scene
        SceneManager.LoadScene(firstLevel);

        PlayerPrefs.SetString("CurrentLevel", "");

        PlayerPrefs.SetString(firstLevel + "_cp", "");
    }


    // initialise the game data
    private void InitialiseGameData()
    {
        // set the player's current health in the game data instance
        GameData.gameDataInstance.currentHealth = 100;

        // set the player's maximum health in the game data instance
        GameData.gameDataInstance.maximumHealth = 100;
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }


} // end of class
