using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	private List<GameObject> levels;

    private int currLevel = 0;

    private List<GameObject> quests;

    public enum GameState
    {
        STOPPED,
        PAUSED,
        RUNNING,
    }

    private GameState currGameState = GameState.STOPPED;

    // Start is called before the first frame update
    private void Start()
    {
        // Trigger introduction
        Character mayor = dialogueManager.GetComponent<DialogueManager>().getCharacter( "Mayor Wilson" );
        dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( mayor, "Introduction" );

        mouseInput.OnMouseDown += HandleMouseClick;
        quests = new List<GameObject>();

        setUp();
    }

    private void setUp()
    {
        // Update score and inventory
        // UI Set up
        scoreUIManager.GetComponent<UI_ProgressBar>().SetPointGoal( levels[currLevel].GetComponent<Level>().GetPointGoal() );
        scoreUIManager.GetComponent<UI_ProgressBar>().SetUIScore( playerManager.GetComponent<PlayerManager>().GetScore() );

        // Inventory
        buildingPannelUI.GetComponent<BuildingPanelUI>().clearInventory();
        buildingPannelUI.GetComponent<BuildingPanelUI>().addBuildingSet( levels[currLevel].GetComponent<Level>().GetLevelInventory() );


        // Set up quests for this level
        quests = quests.Concat( levels[currLevel].GetComponent<Level>().GetQuests() ).ToList();
        triggerQuests( Building.BuildingType.Default );

        // TODO: HOW DO WE HANDLE PLAYER POINTS WHEN MOVING FROM ONE LEVEL TO NEXT?
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
        int pointGoal = levels[currLevel].GetComponent<Level>().GetPointGoal();

        if ( currScore >= pointGoal )
        {
            // We have passed the level
            if ( currLevel + 1 < levels.Count )
            {
                // Switch level
                currLevel += 1;

                setUp();
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
        // Trigger any quests that need to be triggered
        triggerQuests( type );

        // Did we pass any quests? (fail any?) -> update relations
        foreach( GameObject q in quests )
        {
            Quest quest = q.GetComponent<Quest>();

            // TODO: add check to see reverse (ie. school placed near low income OR income placed near school)
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

    public void triggerQuests( Building.BuildingType triggerType )
    {
        foreach( GameObject q in quests )
        {
            Quest quest = q.GetComponent<Quest>();

            if ( !quest.wasTriggered() && quest.getTriggerBuilding() == triggerType )
            {
                quest.triggerQuest();
                List<Quest.Message> messages = quest.getTriggerMessages();

                foreach( Quest.Message message in messages )
                {
                    // Send out tweets!
                    dialogueManager.GetComponent<DialogueManager>().AddMessage( message );
                }
            }
        }
    }

    public void finishQuest( Quest quest, bool passed )
    {
        // Send out victory or failure tweets
        List<Quest.Message> messages = passed ? quest.getSuccessMessages() : quest.getFailureMessages();

        foreach( Quest.Message message in messages )
        {
            // Send out tweets!
            dialogueManager.GetComponent<DialogueManager>().AddMessage( message );
        }

        // Update character relations
        List<string> characters = quest.getCharacters();

        foreach ( string characterName in characters )
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
        foreach( GameObject quest in quests )
        {
            quest.GetComponent<Quest>().increaseBuildingsPlaced();
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
