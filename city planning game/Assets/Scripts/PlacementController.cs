using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Thanks to:
// https://unity3d.college/2017/08/02/how-to-create-a-unity3d-building-placement-system-for-rts-or-city-builders-let-your-player-place-a-3d-object-in-the-world/
// https://medium.com/codex/making-a-rts-game-1-placing-buildings-unity-c-c53c7180b630
public class PlacementController : MonoBehaviour
{
    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;

    [SerializeField]
    private GameObject gameManager;

    private Transform buttonTransform;

    private int buildingsPlaced = 0;

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Escape )  || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if ( currentPlaceableObject )
            {
                var building = currentPlaceableObject.GetComponent<Building>();
                building.ClearIndicators();

                Destroy( currentPlaceableObject );
            }
        }

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }        
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        int layerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hitInfo, 100.0f, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        // Don't place when over UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() )
        {
            var building = currentPlaceableObject.GetComponent<Building>();
            
            if(!building.isColliding()) {
                int points = building.PlaceBuilding();
                gameManager.GetComponent<GameManager>().updatePlayerScore( points );

                currentPlaceableObject = null;

                // Remove button from inventory
                gameManager.GetComponent<GameManager>().removeButton( buttonTransform );
                buttonTransform = null;

                buildingsPlaced += 1;

                gameManager.GetComponent<GameManager>().updateQuests();
                gameManager.GetComponent<GameManager>().checkQuests( building.buildingType, building.getActiveBuildings() );
            }
        }
    }

    public void SetActiveBuildingType( GameObject placeableObjectPrefab, Transform transform )
    {
        if ( currentPlaceableObject == null 
            && gameManager.GetComponent<GameManager>().canPlaceObject( placeableObjectPrefab ) )
        {
            currentPlaceableObject = Instantiate( placeableObjectPrefab );
            buttonTransform = transform;
        }
    }

    public void ResetBuildingCount()
    {
        buildingsPlaced = 0;
    }

    public int GetBuildingsPlacedCount()
    {
        return ( buildingsPlaced );
    }

    public bool IsHoldingBuilding(){
        return currentPlaceableObject != null;
    }
}
