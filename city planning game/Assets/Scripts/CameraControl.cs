using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{    
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = 2 * Input.GetAxis("Horizontal");
    }

    void FixedUpdate() { //physics update - 100hz
        transform.RotateAround(Vector3.zero, Vector3.up, horizontalInput);
    }
        
}
