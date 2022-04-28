using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
    private List<GameObject> finishedQuests;

    public enum GameState
    {
        STOPPED,
        PAUSED,
        RUNNING,
    }

    private GameState currGameState = GameState.STOPPED;

    private bool firstFail = false;
    private bool secondFail = false;
    private bool timeToQuit = false;

    private bool gameOver = false;

    public struct DialogueEntry
    {
        public Character character;
        public string key;

        public DialogueEntry( Character c, string k )
        {
            character = c;
            key = k;
        }
    };

    List<DialogueEntry> dialogueQueue;
    private bool inDialogue = false;

    // Start is called before the first frame update
    private void Start()
    {
        dialogueQueue = new List<DialogueEntry>();

        // Trigger introduction
        Character mayor = dialogueManager.GetComponent<DialogueManager>().getCharacter( "Mayor Wilson" );
        // dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( mayor, "Introduction" );
        dialogueQueue.Add( new DialogueEntry( mayor, "Introduction" ) );

        mouseInput.OnMouseDown += HandleMouseClick;
        quests = new List<GameObject>();
        finishedQuests = new List<GameObject>();

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
    }

    private void HandleMouseClick(Vector3 pos)
    {
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
            else if ( !gameOver )
            {
                // We have finished last level
                gameOver = true;

                // TODO: Show endings then quit! (Get dialogue from character ALSO show dialogue for mayor for ending?)
                List<Character> characters = dialogueManager.GetComponent<DialogueManager>().getCharacters();

                foreach ( Character character in characters )
                {
                    string dialogueKey = character.GetEndingDialogueKey();

                    if ( dialogueKey != null )
                    {
                        // Add dialogues to queue
                        dialogueQueue.Add( new DialogueEntry( character, dialogueKey ) );
                        // dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( character, dialogueKey );
                    }
                }
            }
        }
        else if ( buildingPannelUI.GetComponent<BuildingPanelUI>().numInInventory() == 0 )
        {
            // We have failed the level
            // We offer one time bailout by oil baron then we fail the game
            if ( !firstFail )
            {
                // oil baron
                Character oilBaron = dialogueManager.GetComponent<DialogueManager>().getCharacter( "Gulliver" );
                // dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( oilBaron, "BailOut" );
                dialogueQueue.Add( new DialogueEntry( oilBaron, "BailOut" ) );

                // Add oil rig to inventory
                int pointsNeeded = pointGoal - currScore;
                buildingPannelUI.GetComponent<BuildingPanelUI>().addOilRig( pointsNeeded );

                firstFail = true;
            }
            else if ( !secondFail )
            {
                Character mayor = dialogueManager.GetComponent<DialogueManager>().getCharacter( "Mayor Wilson" );
                dialogueQueue.Add( new DialogueEntry( mayor, "Failure" ) );
                // dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( mayor, "Failure" );

                secondFail = true;
                // Invoke("SetQuit", 2.0f); // Do this to handle thread stuff :(
            }

            // Go to main menu
            if ( secondFail && !isInDialogue() )
            {
                SceneManager.LoadScene("Main Menu");
            }
        }

        // Game over
        if ( gameOver && !isInDialogue() )
        {
            SceneManager.LoadScene("Main Menu");
        }

        // Display dialogues
        if ( dialogueQueue.Count > 0 && !inDialogue )
        {
            inDialogue = true;
            DialogueEntry entry = dialogueQueue[0];
            dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( entry.character, entry.key );
        }
    }

    // public void SetQuit()
    // {
    //     timeToQuit = true;
    // }

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
            if ( quest.getTargetBuilding() == type && !quest.isFinished() )
            {
                // Did we pass or fail the quest
                if ( quest.passedQuest( nearBuildings ) )
                {
                    // We have passed
                    finishQuest( q, true );
                    
                }
                else if ( quest.failedQuest() )
                {
                    // We have failed
                    finishQuest( q, false );
                }
            }

            if ( !quest.isFinished() && quest.failedQuest() )
            {
                // We failed the quest by placing too many other buildings
                finishQuest( q, false );
            }
        }

        foreach ( GameObject quest in finishedQuests )
        {
            quests.Remove( quest );
        }

        if ( finishedQuests.Count() > 0 )
        {
            finishedQuests = new List<GameObject>();
        }

        // Check if we should show dialogue for any character
        checkDialogue();
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

    public void finishQuest( GameObject q, bool passed )
    {
        Quest quest = q.GetComponent<Quest>();

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

        quest.finish();

        // Remove quest from list!
        finishedQuests.Add( q );
    }

    public void checkDialogue()
    {
        // Oil baron intro
        int numBuildingsPlaced = placementController.GetComponent<PlacementController>().GetBuildingsPlacedCount();
        if ( numBuildingsPlaced == 3 )
        {
            Character oilBaron = dialogueManager.GetComponent<DialogueManager>().getCharacter( "Gulliver" );
            // dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( oilBaron, "Introduction" );
            dialogueQueue.Add( new DialogueEntry( oilBaron, "Introduction" ) );
        }


        List<Character> characters = dialogueManager.GetComponent<DialogueManager>().getCharacters();

        foreach ( Character character in characters )
        {
            string dialogueKey = character.GetDialogueKey();

            if ( dialogueKey != null )
            {
                // TODO: add dialogues to queue
                // dialogueManager.GetComponent<DialogueManager>().QueueDialoguePopup( character, dialogueKey );
                dialogueQueue.Add( new DialogueEntry( character, dialogueKey ) );
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

        // return ( cost <= playerScore );
        return ( true );
    }

    public bool isInDialogue()
    {
        return ( dialogueQueue.Count > 0 );
    }

    public void DialogueOver()
    {
        // inDialogue = false;
        Invoke("SetDialogueOver", 2.0f); // Do this to handle thread stuff :(

        if ( dialogueQueue.Count > 0 )
        {
            dialogueQueue.RemoveAt( 0 );
        }
    }

    public void SetDialogueOver()
    {
        inDialogue = false;
    }

    // private void PlaceGameObject(GameObject obj, Vector3 pos){
    //     Instantiate(obj, pos, Quaternion.identity);
    // }
    // private void PlaceBuilding(Building building, Vector3 pos){
        
    // }
}
