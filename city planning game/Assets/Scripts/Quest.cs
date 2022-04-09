using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Quest object
 */
public class Quest : MonoBehaviour
{
    public struct Message
    {
        public string character;
        public Character.Tweet tweet;
    }

    // What building am I looking at?
    [SerializeField]
    private Building.BuildingType targetBuilding;

    // How do I pass this quest? (what other buildings do I need to be near, building placement limit)
    [SerializeField]
    private List<Building.BuildingType> proximityBuildings;

    [SerializeField]
    private int expireLimit;

    // What character gets updated?
    [SerializeField]
    private List<String> characters;

    // What messages do we immediately send out?
    [SerializeField]
    private List<Message> triggerMessages;

    // What messages do we send on success?
    [SerializeField]
    private List<Message> successMessages;

    // What messages do we send on failure?
    [SerializeField]
    private List<Message> failureMessages;

    private int buildingsPlaced = 0;

    public bool passedQuest( List<GameObject> nearBuildings )
    {
        // TODO: check if passed by near buildings
        return ( false );
    }

    public bool failedQuest()
    {
        return ( expireLimit <= buildingsPlaced );
    }

    public Building.BuildingType getTargetBuilding()
    {
        return ( targetBuilding );
    }

    public List<Message> getTriggerMessages()
    {
        return ( triggerMessages );
    }

    public List<Message> getSuccessMessages()
    {
        return ( successMessages );
    }

    public List<Message> getFailureMessages()
    {
        return ( failureMessages );
    }

    public void increaseBuildingsPlaced()
    {
        buildingsPlaced += 1;
    }

    public List<String> getCharacters()
    {
        return ( characters );
    }
}