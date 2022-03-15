using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{    
    // TODO: change to game object first then get transform from object?
    public Transform target;
    public Transform cameraPos;

    public float X, Y, Z;
    public float angularSpeed = 100f;

    Vector3 cameraOffset;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        // Set camera offset from object
        cameraOffset = new Vector3( 0, 5, -8 );

        // Get target position to rotate around
        if ( target == null ) {
            targetPos = Vector3.zero;
        } else {
            targetPos = target.position;
        }

        // Set transform rotation and position
        transform.rotation = Quaternion.Euler( 15, 0, 0 );
        transform.position = targetPos + cameraOffset;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate() {
        float horizontalInput = Input.GetAxis( "Horizontal" ) * angularSpeed * Time.deltaTime;

        // We have moved significantly
        if ( !Mathf.Approximately( horizontalInput, 0f ) ) {
            transform.RotateAround( targetPos, Vector3.up, horizontalInput );
        }
    }
}
