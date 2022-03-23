using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Thanks to:
// https://unity3d.college/2017/08/02/how-to-create-a-unity3d-building-placement-system-for-rts-or-city-builders-let-your-player-place-a-3d-object-in-the-world/
// https://medium.com/codex/making-a-rts-game-1-placing-buildings-unity-c-c53c7180b630
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    [SerializeField]
    private KeyCode newObjectHotkey = KeyCode.A;

    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;

    void Update()
    {
        HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }        
    }

    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyDown(newObjectHotkey) && currentPlaceableObject == null )
        {
            currentPlaceableObject = Instantiate(placeableObjectPrefab);
        }
        else if ( Input.GetKeyDown( KeyCode.Escape ) )
        {
            Destroy( currentPlaceableObject );
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
        // TODO: add check here for collision
        if (Input.GetMouseButtonDown(0))
        {
            var building = currentPlaceableObject.GetComponent<Building>();
            
            if(!building.isColliding()) {
                building.PlaceBuilding();

                // TODO: get building points and send to game manager to update player score

                currentPlaceableObject = null;
            }
            

        }
    }
}
