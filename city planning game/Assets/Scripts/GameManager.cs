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
        buildingPannelUI.GetComponent<BuildingPanelUI>().addBuildingSet( levels[currLevel] );
    }

    private void HandleMouseClick(Vector3 pos)
    {
        Debug.Log(pos);
        // PlaceGameObject(testCube, pos);

        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: check curr level stuff
        // TODO: did we reach point goal?
        // TODO: did we pass any quests? (fail any?) -> update relations
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
