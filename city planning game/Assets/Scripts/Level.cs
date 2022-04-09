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

    [SerializeField]
    private List<GameObject> quests;

    // Get point total
    public int GetPointGoal()
    {
        return ( mPointGoal );
    }

    public List<GameObject> GetLevelInventory()
    {
        return ( inventory );
    }

    public List<GameObject> GetQuests()
    {
        return ( quests );
    }
}
