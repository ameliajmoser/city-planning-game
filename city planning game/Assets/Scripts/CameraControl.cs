using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{    
    // Transforms for determining rotation and position
    public Transform target;
    public Transform cameraPos;

    // Camera relative position and speed
    public float X, Y, Z;
    public float angularSpeed = 100f;

    // Camera offset and target vectors
    Vector3 cameraOffset;
    Vector3 zoomOffset;
    Vector3 targetPos;

    private float minCameraDistance = 3;
    private float maxCameraDistance = 30;

    private int UILayer;

    // Start is called before the first frame update
    void Start()
    {
        // Set camera offset from object
        cameraOffset = new Vector3( 0, 5, -8);

        // Get target position to rotate around
        if ( target == null ) {
            targetPos = Vector3.zero;
        } else {
            targetPos = target.position;
        }

        // Set transform rotation and position
        transform.rotation = Quaternion.Euler( 25, 0, 0 );
        transform.position = targetPos + cameraOffset;

        // Set UILayer
        UILayer = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        // Set relative location
        X = cameraPos.position.x - targetPos.x;
        Y = cameraPos.position.y - targetPos.y;
        Z = cameraPos.position.z - targetPos.z;

        // Set zoom offset
        zoomOffset = new Vector3( ( X / 10 ), ( Y / 10 ), ( Z / 10 ) );

        // Get scroll input and update offset
        float mouseScroll = 0;
        if ( !Util.IsPointerOverLayer( UILayer ) )
        {
            mouseScroll = Input.GetAxis( "Mouse ScrollWheel" );
        }

        if (Mathf.Approximately(mouseScroll, 0)){
            //switch to key input if scroll wheel is none
            mouseScroll = Input.GetAxis("Vertical");
            //decrease speed to compensate for held keys
            zoomOffset /= 10;
        }

        if ( mouseScroll > 0 && cameraOffset.magnitude > minCameraDistance) {
            cameraOffset -= zoomOffset;
        } else if ( mouseScroll < 0 && cameraOffset.magnitude < maxCameraDistance) {
            cameraOffset += zoomOffset;
        }
        // Update camera position
        transform.position = targetPos + cameraOffset;
    }

    void LateUpdate() {
        // Get horizontal input
        float horizontalInput = Input.GetAxis( "Horizontal" ) * angularSpeed * Time.deltaTime;

        // We have moved significantly
        if ( !Mathf.Approximately( horizontalInput, 0f ) ) {
            // Update rotation and camera offset
            transform.RotateAround( targetPos, Vector3.up, horizontalInput );
            cameraOffset = transform.position - targetPos;
        }
    }
}
