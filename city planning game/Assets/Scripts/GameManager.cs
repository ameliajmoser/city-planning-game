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

    // Start is called before the first frame update
    private void Start()
    {
        mouseInput.OnMouseDown += HandleMouseClick;
        scoreUIManager.GetComponent<UI_ProgressBar>().SetUIScore( playerManager.GetComponent<PlayerManager>().GetScore() );
    }

    private void HandleMouseClick(Vector3 pos)
    {
        Debug.Log(pos);
        // PlaceGameObject(testCube, pos);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatePlayerScore( int amt )
    {
        playerManager.GetComponent<PlayerManager>().UpdateScore( amt );
        scoreUIManager.GetComponent<UI_ProgressBar>().SetUIScore( playerManager.GetComponent<PlayerManager>().GetScore() );
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
