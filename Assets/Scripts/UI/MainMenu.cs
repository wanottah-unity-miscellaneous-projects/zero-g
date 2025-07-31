
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public string firstLevel;

    public GameObject continueButton;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            if (PlayerPrefs.GetString("CurrentLevel") == "")
            {
                continueButton.SetActive(false);
            }
        }

        else
        {
            continueButton.SetActive(false);
        }
    }


    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }


    public void PlayGame()
    {
        InitialiseGameData();


        SceneManager.LoadScene(firstLevel);

        PlayerPrefs.SetString("CurrentLevel", "");

        PlayerPrefs.SetString(firstLevel + "_cp", "");
    }


    private void InitialiseGameData()
    {
        GameData.gameDataInstance.currentHealth = 100;

        GameData.gameDataInstance.maximumHealth = 100;
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }


} // end of class
