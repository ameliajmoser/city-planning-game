using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MouseInput mouseInput;
    public GameObject testCube;

    public Building heldBuilding;

    [SerializeField]
    private GameObject scoreUIManager;

    [SerializeField]
    private GameObject playerManager;

    [SerializeField]
    private GameObject buildingPannelUI;

    [SerializeField]
    private GameObject dialogueManager;

    [SerializeField]
    private GameObject placementController;

    [SerializeField]
	private List<Level> levels;

    private int currLevel = 0;

    // Start is called before the first frame update
    private void Start()
    {
        mouseInput.OnMouseDown += HandleMouseClick;

        // UI Set up
        scoreUIManager.GetComponent<UI_ProgressBar>().SetPointGoal( levels[currLevel].GetPointGoal() );
        scoreUIManager.GetComponent<UI_ProgressBar>().SetUIScore( playerManager.GetComponent<PlayerManager>().GetScore() );

        // Inventory
        buildingPannelUI.GetComponent<BuildingPanelUI>().addBuildingSet( levels[currLevel].GetLevelInventory() );
    }

    private void HandleMouseClick(Vector3 pos)
    {
        Debug.Log(pos);
        // PlaceGameObject(testCube, pos);

        
    }

    // Update is called once per frame
    void Update()
    {
        // check curr level stuff
        int currScore = playerManager.GetComponent<PlayerManager>().GetScore();
        int pointGoal = levels[currLevel].GetPointGoal();

        if ( currScore >= pointGoal )
        {
            // We have passed the level
            if ( currScore + 1 < levels.Count )
            {
                // Switch level
                currScore += 1;

                // Set up any quests for this level
                List<Quest> quests = levels[currLevel].GetQuests();
                foreach( Quest quest in quests )
                {
                    List<Message> messages = quest.getTriggerMessages();

                    foreach( Quest.Message message in messages )
                    {
                        // Send out tweets!
                        dialogueManager.GetComponent<DialogueManager>().AddMessage( message );
                    }
                }
            }
        }
        else if ( buildingPannelUI.GetComponent<BuildingPanelUI>().numInInventory() == 0 )
        {
            // We have failed the level
        }
    }

    // Call when place building
    public void checkQuests( Building.BuildingType type, List<GameObject> nearBuildings )
    {
        // Did we pass any quests? (fail any?) -> update relations
        List<Quest> quests = levels[currLevel].GetQuests();
        foreach( Quest quest in quests )
        {
            // TODO: we should also make sure there are no other buildings of the same type?
            if ( quest.getTargetBuilding() == type )
            {
                // Did we pass or fail the quest
                if ( quest.passedQuest( nearBuildings ) )
                {
                    // We have passed
                    finishQuest( quest, true );
                    
                }
                else if ( quest.failedQuest() )
                {
                    // We have failed
                    finishQuest( quest, false );
                }
            }
        }
    }

    public void finishQuest( Quest quest, bool passed )
    {
        // Send out victory or failure tweets
        List<Message> messages = passed ? quest.getSuccessMessages() : quest.getFailureMessages();

        foreach( Quest.Message message in messages )
        {
            // Send out tweets!
            dialogueManager.GetComponent<DialogueManager>().AddMessage( message );
        }

        // Update character relations
        List<String> characters = quest.getCharacters();

        foreach ( String characterName in characters )
        {
            Character character = dialogueManager.GetComponent<DialogueManager>().getCharacter( characterName );

            if ( character != null )
            {
                if ( passed )
                {
                    character.passQuest();
                }
                else
                {
                    character.failQuest();
                }
            }
        }
    }

    // Call when placing bulding
    public void updateQuests()
    {
        List<Quest> quests = levels[currLevel].GetQuests();
        foreach( Quest quest in quests )
        {
            quest.increaseBuildingsPlaced();
        }
    }

    public void updatePlayerScore( int amt )
    {
        playerManager.GetComponent<PlayerManager>().UpdateScore( amt );
        scoreUIManager.GetComponent<UI_ProgressBar>().SetUIScore( playerManager.GetComponent<PlayerManager>().GetScore() );
    }

    public void removeButton( Transform transform )
    {
        buildingPannelUI.GetComponent<BuildingPanelUI>().removeButton( transform );
    }

    public bool canPlaceObject( GameObject buildingPrefab )
    {
        Building building = buildingPrefab.GetComponent<Building>();
        int cost = building.cost;
        int playerScore = playerManager.GetComponent<PlayerManager>().GetScore();

        return ( cost < playerScore );
    }

    // private void PlaceGameObject(GameObject obj, Vector3 pos){
    //     Instantiate(obj, pos, Quaternion.identity);
    // }
    // private void PlaceBuilding(Building building, Vector3 pos){
        
    // }
}
