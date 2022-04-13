using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Quest object
 */
public class Quest : MonoBehaviour
{
    [System.Serializable]
    public struct Message
    {
        [SerializeField]
        public string character;

        [SerializeField]
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
    private List<string> characters;

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
        // check if placed near all target buildings
        bool passed = true;

        foreach ( Building.BuildingType targetType in proximityBuildings )
        {
            bool typeNear = false;

            foreach ( GameObject building in nearBuildings )
            {
                Building.BuildingType type = building.GetComponent<Building>().buildingType;

                if ( type == targetType )
                {
                    typeNear = true;
                    break;
                }
            }

            passed = passed && typeNear;

            if ( !passed )
            {
                break;
            }
        }

        return ( passed );
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

    public List<string> getCharacters()
    {
        return ( characters );
    }
}