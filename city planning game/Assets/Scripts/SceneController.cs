using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/**
 * This class manages what scene we are in
 */
public class SceneController : MonoBehaviour
{
    // Scene controller instance - signleton
    public static SceneController scInstance = null;

    // Current level object
    private static Level currLevel;

    // Create only one instance
    public void Awake()
    {
        if ( null == scInstance )
        {
            scInstance = this;
            DontDestroyOnLoad( this.gameObject );
        }
        else
        {
            Destroy( this.gameObject );
        }
    }

    // Get current level
    public static Level GetCurrentLevel()
    {
        return ( currLevel );
    }

    // Load new level/scene
    public static void LoadLevel( Level level )
    {
        currLevel = level;
        SceneManager.LoadScene( currLevel.GetScene() );
    }
}
