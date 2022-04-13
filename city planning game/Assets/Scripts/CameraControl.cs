using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{    
    // Transforms for determining rotation and position
    public Transform target;
    public Transform cameraPos;
    public PlacementController placementController;

    // Camera relative position and speed
    private float X, Y, Z;
    public float angularSpeed = 100f;
    public float panSpeedNormal = 30f;
    public float panSpeedFast = 60f;
    private float panSpeed;
    public int outOfBoundsDistance = 62;
    private Vector2 panVector;
    // Camera offset and target vectors
    Vector3 cameraOffset;
    Vector3 zoomOffset;
    Vector3 targetPos;
    Vector3 rightDirection;
    Vector3 forwardDirection;
    Vector3 panDirection;

    private float minCameraDistance = 5;
    private float maxCameraDistance = 60;

    private int UILayer;

    // Start is called before the first frame update
    void Start()
    {
        // Set camera offset from object
        cameraOffset = new Vector3( 0, 5, -8);

        // Set transform rotation and position
        transform.rotation = Quaternion.Euler( 25, 0, 0 );
        transform.position = targetPos + cameraOffset;

        // Set UILayer
        UILayer = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        // Get target position to rotate around
        if ( target == null ) {
            targetPos = Vector3.zero;
        } else {
            targetPos = target.position;
        }

        //use fast speed if holding shift
        if (Input.GetKey (KeyCode.LeftShift)){
            panSpeed = panSpeedFast;
        } else {
            panSpeed = panSpeedNormal;
        }

        // Set relative location
        X = cameraPos.position.x - targetPos.x;
        Y = cameraPos.position.y - targetPos.y;
        Z = cameraPos.position.z - targetPos.z;

        
        // // Get input axes
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        forwardDirection = transform.forward;
        rightDirection = transform.right;
        forwardDirection.y = 0;
        rightDirection.y = 0;

        Vector3 panDirection = forwardDirection*verticalAxis + rightDirection*horizontalAxis;
        
        target.Translate(panDirection * panSpeed * Time.deltaTime);


        
        //scroll only zooms if not holding a building
        if (!placementController.IsHoldingBuilding()){
            zoomOffset = new Vector3( ( X / 10 ), ( Y / 10 ), ( Z / 10 ) );
            float zoomInput = Input.mouseScrollDelta.y; 
            //update offset
            if ( zoomInput > 0 && cameraOffset.magnitude > minCameraDistance) {
                cameraOffset -= zoomOffset;
            } else if ( zoomInput < 0 && cameraOffset.magnitude < maxCameraDistance) {
                cameraOffset += zoomOffset;
            }
        }


        // Update camera position
        transform.position = targetPos + cameraOffset;
    }

    void LateUpdate() {
        // Get horizontal input
        float rotateInput = Input.GetAxis("Rotate") * angularSpeed * Time.deltaTime;

        // We have moved significantly
        if ( !Mathf.Approximately( rotateInput, 0f ) ) {
            // Update rotation and camera offset
            transform.RotateAround( targetPos, Vector3.up, rotateInput);
            cameraOffset = transform.position - targetPos;
        }
    }
    
}
