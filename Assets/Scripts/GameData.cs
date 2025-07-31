
using UnityEngine;


[DefaultExecutionOrder(-1)]
public class GameData : MonoBehaviour
{
    // allow other scripts to access this script
    public static GameData gameDataInstance;


    // GAME DATA //

    #region PLAYER HEALTH

    public int currentHealth;

    public int maximumHealth;

    #endregion


    [HideInInspector] public int currentAmmo;

    [HideInInspector] public int maximumAmmo;

    [HideInInspector] public int currentGun;

    [HideInInspector] public Gun activeGun;








    private void Awake()
    {
        // if the game object containing game data script already exists
        if (gameDataInstance != null)
        {
            // then destroy the game object
            Destroy(gameObject);
        }

        // otherwise
        else
        {
            // create an instance of the game object
            gameDataInstance = this;

            // set the game object to be accessible when a new scene is loaded
            DontDestroyOnLoad(gameObject);
        }
    }


} // end of class
