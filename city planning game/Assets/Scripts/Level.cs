using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level object
 */
public class Level : MonoBehaviour
{
    [SerializeField]
    private int mPointGoal;

    [SerializeField]
    private List<GameObject> inventory;

    // Get point total
    public int GetPointGoal()
    {
        return ( mPointGoal );
    }

    public List<GameObject> GetLevelInventory()
    {
        return ( inventory );
    }

    // TODO: contain quests -> contain failure and pass conditions with builiding placement limit count character relation etc.
}
