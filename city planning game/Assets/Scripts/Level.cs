using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level object
 */
public class Level : MonoBehaviour
{
    // Defines scene name and point goal
    // TODO: ideally point goal would only be defined for a subclass of level that is for game levels only (not menus)
    private int mPointGoal;
    private string mScene;

    // Init level object
    public void init( int pointGoal, string scene )
    {
        mPointGoal = pointGoal;
        mScene = scene;
    }

    // Get point total
    public int GetPointGoal()
    {
        return ( mPointGoal );
    }

    // Get scene name
    public string GetScene()
    {
        return ( mScene );
    }
}
