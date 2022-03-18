using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: this class should contain info about the player. When the player gets new points it should be updated here, or if their inventory get's increased etc
// TODO: We will need to signal the UI controller that the points have been updated to rerender
public class PlayerManager : MonoBehaviour
{
    private int inventorySize = 8;

    [SerializeField]
    private int currScore = 0;

    // TODO: create inventory object here

    // TODO: create scene manager reference to get current level stats like point goal


    // Start is called before the first frame update
    void Start()
    {
        // TODO: instantiate inventory object here
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: ideally we would have a check here if game manager is currently running the game (ie. not in pause or main menu)
    }
}
